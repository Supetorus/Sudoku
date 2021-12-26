using System;
using System.Media;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;

namespace Sudoku
{
	public partial class GameView : Page
	{
		struct Vector2
		{
			public int x, y;

			public Vector2(int x, int y)
			{
				this.x = x;
				this.y = y;
			}
		}

		struct Move
		{
			public int x, y;
			public object prev; // Whatever was previously in the square, whether it was a notes grid or a number.

			public Move(int x, int y, object prev)
			{
				this.x = x;
				this.y = y;
				this.prev = prev;
			}
		}

		class CellInfo
		{
			public int x;
			public int y;
			public bool correct;
			public bool original;
			public Grid grid;

			public CellInfo(int x, int y, bool correct, bool original)
			{
				this.x = x;
				this.y = y;
				this.correct = correct;
				this.original = original;
			}
		}

		Random rng = new Random();

		Button[,] shownButtons = new Button[9, 9];
		Button selectedButton = null;

		Game game;

		System.Windows.Threading.DispatcherTimer dispatcherTimer;

		SoundPlayer click1 = new SoundPlayer(Properties.Resources.click1);
		SoundPlayer click2 = new SoundPlayer(Properties.Resources.click2);
		SoundPlayer click3 = new SoundPlayer(Properties.Resources.click3);
		SoundPlayer note = new SoundPlayer(Properties.Resources.note);

		List<Vector2> unsolved = new List<Vector2>();

		bool notes = false;
		bool hint = false;
		bool started = false;
		bool paused = false;

		Stack<Move> moves = new Stack<Move>();

		//public static int symbol = 0;

		string[][] symbols =
		{
			new string[] { "", "1", "2", "3", "4", "5", "6", "7", "8", "9" }, //Numbers
			new string[] { "", "A", "B", "C", "D", "E", "F", "G", "H", "I" }, //Letters
			new string[] { "", "■", "▲", "◆", "⬤", "⬟", "⬣", "🞧", "🞮", "⯊" }  //Shapes
		};

		public GameView()
		{
			InitializeComponent();
			GenerateGrid();
			txtMistakes.Text = "0 / " + Game.maxMistakes + " Mistakes";

			dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
			dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
			dispatcherTimer.Interval = new TimeSpan(0, 0, 1);

			if (File.Exists("Game.bin")) game = FileIO.Load<Game>("Game.bin");
			else game = new Game();
		}

		public void NewGame(int difficulty)
		{
			game = new();
			game.board = new Board(difficulty);
			game.board.Generate();
			unsolved.Clear();
			for (int x = 0; x < 9; x++)
			{
				for (int y = 0; y < 9; y++)
				{
					int num = game.board.GetNum(x, y);
					shownButtons[x, y].Tag = new CellInfo(x, y, game.board.CheckNum(x, y, num), game.board.CheckNum(x, y, num));
					shownButtons[x, y].Click += BoardClick;
					shownButtons[x, y].SetResourceReference(Control.ForegroundProperty, "brushText");
					shownButtons[x, y].SetResourceReference(Control.BackgroundProperty, "brushBackground");

					if (num == 0)
					{
						unsolved.Add(new Vector2(x, y));

						Grid notesGrid = new Grid();
						notesGrid.VerticalAlignment = VerticalAlignment.Center;
						notesGrid.HorizontalAlignment = HorizontalAlignment.Center;
						notesGrid.Width = 33.33;
						notesGrid.Height = 33.33;
						notesGrid.ColumnDefinitions.Add(new ColumnDefinition());
						notesGrid.ColumnDefinitions.Add(new ColumnDefinition());
						notesGrid.ColumnDefinitions.Add(new ColumnDefinition());
						notesGrid.RowDefinitions.Add(new RowDefinition());
						notesGrid.RowDefinitions.Add(new RowDefinition());
						notesGrid.RowDefinitions.Add(new RowDefinition());
						//notesGrid.ShowGridLines = true;

						GetCellInfo(shownButtons[x, y]).grid = notesGrid;
						shownButtons[x, y].Content = notesGrid;

						for (int i = 0; i < 9; ++i)
						{
							TextBlock txt = new TextBlock();
							txt.SetResourceReference(Control.ForegroundProperty, "brushRightText");
							txt.FontSize = 10;
							txt.VerticalAlignment = VerticalAlignment.Center;
							txt.HorizontalAlignment = HorizontalAlignment.Center;
							Grid.SetColumn(txt, (i) % 3);
							Grid.SetRow(txt, (i) / 3);
							GetGrid(shownButtons[x, y]).Children.Add(txt);
						}
					}
					else
					{
						shownButtons[x, y].Content = symbols[App.settings.Symbol][num];
					}
				}
			}
			RemoveUsedUpNums();

			started = true;
			game.Time = 0;
			Timer.Text = "00:00";
			dispatcherTimer.Start();
			game.ResetMistakes();
			moves.Clear();
			txtMistakes.Text = "0 / " + Game.maxMistakes + " Mistakes";
			txtHints.Text = game.HintNum.ToString() + " Hints";
			btnUndo.IsEnabled = true;
			btnErase.IsEnabled = true;
		}

		public void LoadGame()
		{
			game = FileIO.Load<Game>("Game.bin");
			for (int x = 0; x < 9; x++)
			{
				for (int y = 0; y < 9; y++)
				{
					int num = game.board.GetNum(x, y);
					shownButtons[x, y].Tag = new CellInfo(x, y, game.board.CheckNum(x, y, num), game.board.CheckNum(x, y, num));
					shownButtons[x, y].Click += BoardClick;
					if (game.board.GetUnsolvedNum(x, y) == num || num == 0)
					{
						// The number is in the original board so it gets original color. (or it's blank)
						shownButtons[x, y].SetResourceReference(Control.ForegroundProperty, "brushText");
						shownButtons[x, y].SetResourceReference(Control.BackgroundProperty, "brushBackground");
					}
					else if (game.board.GetCorrectNum(x, y) == num)
					{
						// The number is not in the original board but it is in the final board, so it
						// gets correct color.
						shownButtons[x, y].SetResourceReference(Control.ForegroundProperty, "brushRightText");
						shownButtons[x, y].SetResourceReference(Control.BackgroundProperty, "brushRightBackground");
					}
					else
					{
						// The number is not correct.
						shownButtons[x, y].SetResourceReference(Control.ForegroundProperty, "brushWrongText");
						shownButtons[x, y].SetResourceReference(Control.BackgroundProperty, "brushWrongBackground");
					}

					if (num == 0)
					{
						unsolved.Add(new Vector2(x, y));

						Grid notesGrid = new Grid();
						notesGrid.VerticalAlignment = VerticalAlignment.Center;
						notesGrid.HorizontalAlignment = HorizontalAlignment.Center;
						notesGrid.Width = 33.33;
						notesGrid.Height = 33.33;
						notesGrid.ColumnDefinitions.Add(new ColumnDefinition());
						notesGrid.ColumnDefinitions.Add(new ColumnDefinition());
						notesGrid.ColumnDefinitions.Add(new ColumnDefinition());
						notesGrid.RowDefinitions.Add(new RowDefinition());
						notesGrid.RowDefinitions.Add(new RowDefinition());
						notesGrid.RowDefinitions.Add(new RowDefinition());
						//notesGrid.ShowGridLines = true;

						GetCellInfo(shownButtons[x, y]).grid = notesGrid;
						shownButtons[x, y].Content = notesGrid;

						for (int i = 0; i < 9; ++i)
						{
							TextBlock txt = new TextBlock();
							txt.SetResourceReference(Control.ForegroundProperty, "brushRightText");
							txt.FontSize = 10;
							txt.VerticalAlignment = VerticalAlignment.Center;
							txt.HorizontalAlignment = HorizontalAlignment.Center;
							Grid.SetColumn(txt, (i) % 3);
							Grid.SetRow(txt, (i) / 3);
							GetGrid(shownButtons[x, y]).Children.Add(txt);
						}
					}
					else
					{
						shownButtons[x, y].Content = num;
					}
				}
			}
			RemoveUsedUpNums();

			started = true;
			Timer.Text = (game.Time / 60 < 10 ? "0" : "") + game.Time / 60 + ":" + (game.Time % 60 < 10 ? "0" : "") + game.Time % 60;
			dispatcherTimer.Start();
			txtHints.Text = game.HintNum.ToString() + " Hints";
		}

		private void GenerateGrid()
		{
			for (int x = 0; x < 9; x++)
			{
				for (int y = 0; y < 9; y++)
				{
					shownButtons[x, y] = new Button();
					Grid.SetRow(shownButtons[x, y], x);
					Grid.SetColumn(shownButtons[x, y], y);
					gridView.Children.Add(shownButtons[x, y]);
					shownButtons[x, y].SetResourceReference(Control.ForegroundProperty, "brushText");
					shownButtons[x, y].SetResourceReference(Control.BackgroundProperty, "brushBackground");
					shownButtons[x, y].FontSize = 24;

					double thickness = 2;
					double left = 0;
					double top = 0;
					double right = 0;
					double bottom = 0;

					if ((x + 1) % 3 == 0 && x < 8) bottom = thickness;
					if ((x) % 3 == 0 && x > 0) top = thickness;
					if ((y + 1) % 3 == 0 && y < 8) right = thickness;
					if (y % 3 == 0 && y > 0) left = thickness;
					Border border = new();
					border.BorderThickness = new Thickness(left, top, right, bottom);
					border.SetResourceReference(Control.BorderBrushProperty, "brushBorder");
					Grid.SetColumn(border, y);
					Grid.SetRow(border, x);
					gridView.Children.Add(border);
				}
			}
		}

		private void BoardClick(object sender, RoutedEventArgs e)
		{
			if (App.settings.SoundsOn) { click1.Play(); }
			if (selectedButton != null)
			{
				Highlight(((CellInfo)selectedButton.Tag).x, ((CellInfo)selectedButton.Tag).y, false);
			}

			selectedButton = sender as Button;

			Highlight(((CellInfo)selectedButton.Tag).x, ((CellInfo)selectedButton.Tag).y, true);
		}

		private void Highlight(int x, int y, bool highlight)
		{
			string selected = highlight ? "brushSelectedBackground" : "brushBackground";
			string area = highlight ? "brushAreaBackground" : "brushBackground";
			string matching = highlight ? "brushMatchingBackground" : "brushBackground";

			//Highlight row and column
			for (int i = 0; i < 9; ++i)
			{
				shownButtons[i, y].SetResourceReference(Control.BackgroundProperty, GetHighlight(i, y, highlight, area));
				shownButtons[x, i].SetResourceReference(Control.BackgroundProperty, GetHighlight(x, i, highlight, area));
			}

			//Highlight box
			for (int bx = x - (x % 3); bx < x - (x % 3) + 3; ++bx)
			{
				for (int by = y - (y % 3); by < y - (y % 3) + 3; ++by)
				{
					shownButtons[bx, by].SetResourceReference(Control.BackgroundProperty, GetHighlight(bx, by, highlight, area));
				}
			}

			//Highlight matching numbers
			if (HasNum(shownButtons[x, y]) && ((CellInfo)shownButtons[x, y].Tag).correct) // Wrong numbers are not highlighted.
			{
				int num = game.board.GetNum(x, y);
				for (int i = 0; i < 9; ++i)
				{
					for (int j = 0; j < 9; ++j)
					{
						if (HasNum(shownButtons[i, j]) && game.board.GetNum(i, j) == num && GetCellInfo(shownButtons[i, j]).correct)
						{
							shownButtons[i, j].SetResourceReference(Control.BackgroundProperty, GetHighlight(i, j, highlight, matching));
						}
					}
				}
			}

			shownButtons[x, y].SetResourceReference(Control.BackgroundProperty, GetHighlight(x, y, highlight, selected));
		}

		private string GetHighlight(int x, int y, bool highlight, string def)
		{
			return !highlight && !GetCellInfo(shownButtons[x, y]).original && HasNum(shownButtons[x, y]) ? 
				(GetCellInfo(shownButtons[x, y]).correct ? "brushRightBackground" : "brushWrongBackground") : def;
		}

		public bool HasNum(Button b)
		{
			return b.Content.GetType() != typeof(Grid) && b.Content.ToString() != "";
		}

		private CellInfo GetCellInfo(Button b)
		{
			return (CellInfo)b.Tag;
		}

		private Grid GetGrid(Button b)
		{
			return ((CellInfo)b.Tag).grid;
		}

		public void AddNumber(int x, int y, int num)
		{
			if (!notes || hint)
			{
				if(game.board.GetNum(x, y) == num) { return; }
				moves.Push(new Move(x, y, shownButtons[x, y].Content));
				game.board.SetNum(x, y, num);

				if (game.board.CheckNum(x, y, num)) // It's the right number
				{
					if (App.settings.SoundsOn) { click2.Play(); }
					moves.Pop();
					shownButtons[x, y].SetResourceReference(Control.ForegroundProperty, "brushRightText");
					shownButtons[x, y].SetResourceReference(Control.BackgroundProperty, "brushRightBackground");
					shownButtons[x, y].Content = symbols[App.settings.Symbol][num];
					((CellInfo)shownButtons[x, y].Tag).correct = true;
					unsolved.Remove(new Vector2(x, y));

					//remove this number from notes in area
					for (int i = 0; i < 9; ++i)
					{
						if (!HasNum(shownButtons[x, i])) { ((TextBlock)GetGrid(shownButtons[x, i]).Children[num - 1]).Text = ""; }

						if (!HasNum(shownButtons[i, y])) { ((TextBlock)GetGrid(shownButtons[i, y]).Children[num - 1]).Text = ""; }
					}

					for (int bx = x - (x % 3); bx < x - (x % 3) + 3; ++bx)
					{
						for (int by = y - (y % 3); by < y - (y % 3) + 3; ++by)
						{
							if (!HasNum(shownButtons[bx, by])) { ((TextBlock)GetGrid(shownButtons[bx, by]).Children[num - 1]).Text = ""; }
						}
					}

					if (!hint) { Highlight(x, y, true); }
					hint = false;
				}
				else // It's the wrong number
				{
					if (App.settings.SoundsOn) { click3.Play(); }
					shownButtons[x, y].SetResourceReference(Control.ForegroundProperty, "brushWrongText");
					shownButtons[x, y].SetResourceReference(Control.BackgroundProperty, "brushWrongBackground");
					shownButtons[x, y].Content = symbols[App.settings.Symbol][num];
					game.board.SetNum(x, y, num);
					game.IncrementMistakes();
					txtMistakes.Text = game.Mistakes + " / " + Game.maxMistakes + " Mistakes";
					Highlight(x, y, true);
				}
				RemoveUsedUpNums();
			}
			else
			{
				AddNote(x, y, num);
			}

			if(game.Mistakes == Game.maxMistakes)
			{
				dispatcherTimer.Stop();
				MessageBox.Show("Sorry, you've used up all your mistakes!");
				btnUndo.IsEnabled = false;
				btnErase.IsEnabled = false;
			}
			else if (game.board.IsGameWon())
			{
				dispatcherTimer.Stop();
				MessageBox.Show("Congratulations, you win!");
			}
		}

		public void EraseNotes(int x, int y)
		{
			for (int i = 0; i < 9; ++i)
			{
				((TextBlock)GetGrid(shownButtons[x, y]).Children[i]).Text = "";
			}
		}

		public void AddNote(int x, int y, int num)
		{
			//if (board.CheckSafety(x, y, num))
			//{
			if (App.settings.SoundsOn) { note.Play(); }
			TextBlock txt = (TextBlock)GetGrid(shownButtons[x, y]).Children[num - 1];
			if (txt.Text != "") { txt.Text = ""; }
			else { txt.Text = symbols[App.settings.Symbol][num]; }
			//}
		}

		public void RemoveUsedUpNums()
		{
			for (int i = 1; i <= 9; i++)
			{
				if (game.board.IsNumFull(i))
				{
					foreach (Button btn in gridKeypad.Children)
					{
						if (btn.Content.ToString() == i.ToString())
						{
							btn.Visibility = Visibility.Hidden;
							break;
						}
					}
				}
				else
				{
					foreach (Button btn in gridKeypad.Children)
					{
						if (btn.Content.ToString() == i.ToString())
						{
							btn.Visibility = Visibility.Visible;
							break;
						}
					}
				}
			}
		}

		public void Erase(object sender, RoutedEventArgs e)
		{
			// Correct numbers cannot be removed.
			if (selectedButton != null && !GetCellInfo(selectedButton).correct && !game.board.IsGameWon() && game.Mistakes < Game.maxMistakes)
			{
				RemoveUsedUpNums();
				if (selectedButton.Content.GetType() == typeof(Grid))
				{
					for (int i = 0; i < 9; ++i)
					{
						((TextBlock)GetGrid(selectedButton).Children[i]).Text = "";
					}
				}
				else
				{
					selectedButton.Content = GetGrid(selectedButton);
				}
			}
		}

		private void Window_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.Key >= Key.D1 && e.Key <= Key.D9 && selectedButton != null && !GetCellInfo(selectedButton).correct && game.Mistakes < Game.maxMistakes)
			{
				int num = int.Parse(e.Key.ToString().Replace('D', ' ').Trim());

				AddNumber(GetCellInfo(selectedButton).x, GetCellInfo(selectedButton).y, num);
			}
			else if(e.Key == Key.Escape)
			{
				//pause timer and hide grid
				if (paused = !paused)
				{
					dispatcherTimer.Stop();
					gridView.Visibility = Visibility.Hidden;
					Pause.SetResourceReference(Control.BackgroundProperty, "brushSelectedBackground");
				}
				else
				{
					dispatcherTimer.Start();
					gridView.Visibility = Visibility.Visible;
					Pause.SetResourceReference(Control.BackgroundProperty, "brushBackground");
				}
			}
		}

		private void KeyPad(object sender, RoutedEventArgs e)
		{
			if (selectedButton != null && !GetCellInfo(selectedButton).correct && game.Mistakes < Game.maxMistakes)
			{
				int num = int.Parse((sender as Button).Name.Replace("Keypad", ""));

				AddNumber(GetCellInfo(selectedButton).x, GetCellInfo(selectedButton).y, num);
			}
		}

		private void Undo(object sender, RoutedEventArgs e)
		{
			if (moves.Count > 0 && !game.board.IsGameWon() && game.Mistakes < Game.maxMistakes)
			{
				Move move = moves.Pop();

				if (move.prev.GetType() == typeof(Grid))
				{
					for (int k = 0; k < 9; ++k)
					{
						((TextBlock)((Grid)move.prev).Children[k]).Text =
							((TextBlock)((Grid)move.prev).Children[k]).Text != "" ? symbols[App.settings.Symbol][k + 1] : "";
					}

					GetCellInfo(shownButtons[move.x, move.y]).correct = false;
					shownButtons[move.x, move.y].Content = move.prev;
					game.board.SetNum(move.x, move.y, 0);
					shownButtons[move.x, move.y].SetResourceReference(Control.ForegroundProperty, "brushText");
					shownButtons[move.x, move.y].SetResourceReference(Control.BackgroundProperty, "brushBackground");
				}
				else
				{
					for(int i = 0; i < 3; i++)
					{
						for (int j = 1; j < 10; j++)
						{
							if (move.prev.ToString() == symbols[i][j])
							{
								shownButtons[move.x, move.y].Content = symbols[App.settings.Symbol][j];
								game.board.SetNum(move.x, move.y, j);
								GetCellInfo(shownButtons[move.x, move.y]).correct = false;
								shownButtons[move.x, move.y].SetResourceReference(Control.ForegroundProperty, "brushWrongText");
								shownButtons[move.x, move.y].SetResourceReference(Control.BackgroundProperty, "brushWrongBackground");
								RemoveUsedUpNums();
							}
						}
					}
				}
			}
		}

		private void Hint(object sender, RoutedEventArgs e)
		{
			if (game.HintNum > 0 && unsolved.Count > 0)
			{
				hint = true;
				int i = rng.Next(unsolved.Count - 1);
				Vector2 v = unsolved[i];
				AddNumber(v.x, v.y, game.board.GetCorrectNum(v.x, v.y));
				--game.HintNum;
			}
			txtHints.Text = game.HintNum + " Hints";
		}

		private void Notes(object sender, RoutedEventArgs e)
		{
			if (notes)
			{
				(sender as Button).SetResourceReference(Control.BackgroundProperty, "brushBackground");
			}
			else
			{
				(sender as Button).SetResourceReference(Control.BackgroundProperty, "brushSelectedBackground");
			}

			notes = !notes;
		}

		private void Reset_Board(object sender, RoutedEventArgs e)
		{
			selectedButton = null;
			game.board.ResetBoard();
			game.ResetMistakes();
			moves.Clear();
			unsolved.Clear();
			txtMistakes.Text = "0 / " + Game.maxMistakes + " Mistakes";
			for (int x = 0; x < 9; x++)
			{
				for (int y = 0; y < 9; y++)
				{
					int num = game.board.GetNum(x, y);
					if (GetGrid(shownButtons[x, y]) != null) { EraseNotes(x, y); unsolved.Add(new Vector2(x, y)); }
					shownButtons[x, y].Content = num > 0 ? symbols[App.settings.Symbol][num] : GetGrid(shownButtons[x, y]);
					shownButtons[x, y].SetResourceReference(Control.ForegroundProperty, "brushText");
					shownButtons[x, y].SetResourceReference(Control.BackgroundProperty, "brushBackground");
					GetCellInfo(shownButtons[x, y]).correct = num > 0;
				}
			}
			RemoveUsedUpNums();
			game.HintNum = game.TotalHints;
			txtHints.Text = game.HintNum + " Hints";

			game.Time = 0;
			Timer.Text = "00:00";
			btnUndo.IsEnabled = true;
			btnErase.IsEnabled = true;
		}

		private void Pause_Game(object sender, RoutedEventArgs e)
		{
			//pause timer and hide grid
			if (paused = !paused)
			{
				dispatcherTimer.Stop();
				gridView.Visibility = Visibility.Hidden;
				Pause.SetResourceReference(Control.BackgroundProperty, "brushSelectedBackground");
			}
			else
			{
				dispatcherTimer.Start();
				gridView.Visibility = Visibility.Visible;
				Pause.SetResourceReference(Control.BackgroundProperty, "brushBackground");
			}
		}

		private void cmbxiHome_Selected(object sender, RoutedEventArgs e)
		{
			ViewManager.SetView(ViewManager.HomeView);
		}

		private void cmbxiSettings_Selected(object sender, RoutedEventArgs e)
		{
			ViewManager.SetView(ViewManager.SettingsView);
		}

		private void cmbxiNewGame_Selected(object sender, RoutedEventArgs e)
		{
			ViewManager.SetView(ViewManager.NewGameView);
		}

		private void cmbxNav_Selection_Changed(object sender, RoutedEventArgs e)
		{
			(sender as ComboBox).SelectedIndex = 0;
		}

		private void Print_Current(object sender, RoutedEventArgs e)
		{ // This method can be hooked up to a button and will print the current board.
			Debug.WriteLine(game.board);
		}

		private void dispatcherTimer_Tick(object sender, EventArgs e)
		{
			++game.Time;
			Timer.Text = (game.Time / 60 < 10 ? "0" : "") + game.Time / 60 + ":" + (game.Time % 60 < 10 ? "0" : "") + game.Time % 60;
		}

		private void Page_Loaded(object sender, RoutedEventArgs e)
		{
			if (started)
			{
				dispatcherTimer.Start();
			}
		
			Keypad1.Content = symbols[App.settings.Symbol][1];
			Keypad2.Content = symbols[App.settings.Symbol][2];
			Keypad3.Content = symbols[App.settings.Symbol][3];
			Keypad4.Content = symbols[App.settings.Symbol][4];
			Keypad5.Content = symbols[App.settings.Symbol][5];
			Keypad6.Content = symbols[App.settings.Symbol][6];
			Keypad7.Content = symbols[App.settings.Symbol][7];
			Keypad8.Content = symbols[App.settings.Symbol][8];
			Keypad9.Content = symbols[App.settings.Symbol][9];

			for (int i = 0; i < 9; i++)
			{
				for (int j = 0; j < 9; j++)
				{
					if(HasNum(shownButtons[i, j]))
					{
						shownButtons[i, j].Content = symbols[App.settings.Symbol][game.board.GetNum(i, j)];
					}
					else
					{
						for (int k = 0; k < 9; ++k)
						{
							((TextBlock)GetGrid(shownButtons[i, j]).Children[k]).Text = 
								((TextBlock)GetGrid(shownButtons[i, j]).Children[k]).Text != "" ? symbols[App.settings.Symbol][k + 1] : "";
						}
					}
				}
			}
		}

		private void Page_Unloaded(object sender, RoutedEventArgs e)
		{
			dispatcherTimer.Stop();
		}

		public void SaveGame()
		{
			FileIO.Save("Game.bin", game);
		}
	}
}
