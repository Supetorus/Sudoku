using System;
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
			public object prev;

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
			public Grid grid;

			public CellInfo(int x, int y, bool correct)
			{
				this.x = x;
				this.y = y;
				this.correct = correct;
			}
		}

		Random rng = new Random();

		Button[,] shownButtons = new Button[9, 9];
		Button selectedButton = null;

		Game game;

		System.Windows.Threading.DispatcherTimer dispatcherTimer;
		int time = 0;

		List<Vector2> unsolved = new List<Vector2>();

		int hintNum = 3;
		int totalHints = 3;

		bool notes = false;
		bool hint = false;
		bool started = false;

		Stack<Move> moves = new Stack<Move>();

		public GameView()
		{
			InitializeComponent();
			GenerateGrid();
			txtMistakes.Text = "0 / " + Game.maxMistakes + " Mistakes";

			dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
			dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
			dispatcherTimer.Interval = new TimeSpan(0, 0, 1);
		}

		public void NewGame(int difficulty)
		{
			game = new();
			game.board = new Board(difficulty);
			game.board.Generate();
			for (int x = 0; x < 9; x++)
			{
				for (int y = 0; y < 9; y++)
				{
					int num = game.board.GetNum(x, y);
					shownButtons[x, y].Tag = new CellInfo(x, y, game.board.CheckNum(x, y, num));
					shownButtons[x, y].Click += BoardClick;

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
							txt.Foreground = (SolidColorBrush)FindResource("brushRightText");
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
			time = 0;
			Timer.Text = "0:0";
			dispatcherTimer.Start();
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
					shownButtons[x, y].Style = (Style)FindResource("styleSudokuSquare");

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
					border.BorderBrush = (SolidColorBrush)FindResource("brushBorder");
					Grid.SetColumn(border, y);
					Grid.SetRow(border, x);
					gridView.Children.Add(border);
				}
			}
		}

		private void BoardClick(object sender, RoutedEventArgs e)
		{
			if (selectedButton != null)
			{
				Highlight(((CellInfo)selectedButton.Tag).x, ((CellInfo)selectedButton.Tag).y, false);
			}

			selectedButton = sender as Button;

			Highlight(((CellInfo)selectedButton.Tag).x, ((CellInfo)selectedButton.Tag).y, true);
		}

		private void Highlight(int x, int y, bool highlight)
		{
			Style selectedStyle = highlight ? (Style)FindResource("styleSudokuSquareSelected") : (Style)FindResource("styleSudokuSquare");
			Style areaStyle = highlight ? (Style)FindResource("styleSudokuSquareArea") : (Style)FindResource("styleSudokuSquare");
			Style matchingStyle = highlight ? (Style)FindResource("styleSudokuSquareMatching") : (Style)FindResource("styleSudokuSquare");

			//Highlight row and column
			for (int i = 0; i < 9; ++i)
			{
				shownButtons[i, y].Style = areaStyle;
				shownButtons[x, i].Style = areaStyle;
			}

			//Highlight box
			for (int bx = x - (x % 3); bx < x - (x % 3) + 3; ++bx)
			{
				for (int by = y - (y % 3); by < y - (y % 3) + 3; ++by)
				{
					shownButtons[bx, by].Style = areaStyle;
				}
			}

			//Highlight matching numbers
			if (HasNum(shownButtons[x, y]) && ((CellInfo)shownButtons[x, y].Tag).correct)
			{
				int num = int.Parse(shownButtons[x, y].Content.ToString());
				for (int i = 0; i < 9; ++i)
				{
					for (int j = 0; j < 9; ++j)
					{
						if (HasNum(shownButtons[i, j]) && int.Parse(shownButtons[i, j].Content.ToString()) == num)
						{
							shownButtons[i, j].Style = matchingStyle;
						}
					}
				}
			}

			shownButtons[x, y].Style = selectedStyle;
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

		public void Update()
		{
			// Updates every part of the view that needs to be updated
		}

		public void AddNumber(int x, int y, int num)
		{
			if (!notes || hint)
			{
				moves.Push(new Move(x, y, shownButtons[x, y].Content));

				if (game.board.CheckNum(x, y, num)) // It's the right number
				{
					//shownButtons[x, y].Foreground = (SolidColorBrush)FindResource("brushRightText");
					shownButtons[x, y].Style = (Style)FindResource("styleSudokuSquareRight");
					shownButtons[x, y].Content = num;
					((CellInfo)shownButtons[x, y].Tag).correct = true;
					game.board.SetNum(x, y, num);
					unsolved.Remove(new Vector2(x, y));

					//remove this number from notes in area
					for (int i = 0; i < 3; ++i)
					{
						if (!HasNum(shownButtons[x, i])) { EraseNotes(x, i); }

						if (!HasNum(shownButtons[i, y])) { EraseNotes(i, y); }
					}

					for (int bx = x - (x % 3); bx < x - (x % 3) + 3; ++bx)
					{
						for (int by = y - (y % 3); by < y - (y % 3) + 3; ++by)
						{
							if (!HasNum(shownButtons[x, y])) { EraseNotes(x, y); }
						}
					}

					if (!hint) { Highlight(x, y, true); }
					hint = false;
				}
				else // It's the wrong number
				{
					//selectedButton.Foreground = (SolidColorBrush)FindResource("brushWrongText");
					shownButtons[x, y].Style = (Style)FindResource("styleSudokuSquareWrong");
					shownButtons[x, y].Content = num;
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
			TextBlock txt = (TextBlock)GetGrid(shownButtons[x, y]).Children[num - 1];
			if (txt.Text != "") { txt.Text = ""; }
			else { txt.Text = num.ToString(); }
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
			if (selectedButton != null && !GetCellInfo(selectedButton).correct)
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
		}

		private void KeyPad(object sender, RoutedEventArgs e)
		{
			if (selectedButton != null && !GetCellInfo(selectedButton).correct && game.Mistakes < Game.maxMistakes)
			{
				int num = int.Parse((sender as Button).Content.ToString());

				AddNumber(GetCellInfo(selectedButton).x, GetCellInfo(selectedButton).y, num);
			}
		}

		private void Undo(object sender, RoutedEventArgs e)
		{
			if (moves.Count > 0)
			{
				RemoveUsedUpNums();
				Move move = moves.Pop();

				shownButtons[move.x, move.y].Content = move.prev;
				GetCellInfo(shownButtons[move.x, move.y]).correct = false;
			}
		}

		private void Hint(object sender, RoutedEventArgs e)
		{
			if (hintNum > 0 && unsolved.Count > 0)
			{
				hint = true;
				int i = rng.Next(unsolved.Count - 1);
				Vector2 v = unsolved[i];
				AddNumber(v.x, v.y, game.board.GetCorrectNum(v.x, v.y));
				--hintNum;
			}
			txtHints.Text = hintNum + " Hints";
		}

		private void Notes(object sender, RoutedEventArgs e)
		{
			if (notes)
			{
				(sender as Button).Background = (SolidColorBrush)FindResource("brushBackground");
			}
			else
			{
				(sender as Button).Background = (SolidColorBrush)FindResource("brushSelectedText");
			}

			notes = !notes;
		}

		private void Reset_Board(object sender, RoutedEventArgs e)
		{
			game.board.ResetBoard();
			game.ResetMistakes();
			txtMistakes.Text = "0 / " + Game.maxMistakes + " Mistakes";
			for (int x = 0; x < 9; x++)
			{
				for (int y = 0; y < 9; y++)
				{
					int num = game.board.GetNum(x, y);
					if (GetGrid(shownButtons[x, y]) != null) { EraseNotes(x, y); }
					shownButtons[x, y].Content = num > 0 ? num.ToString() : GetGrid(shownButtons[x, y]);
					GetCellInfo(shownButtons[x, y]).correct = num > 0;
				}
			}
			RemoveUsedUpNums();
			hintNum = totalHints;
			txtHints.Text = hintNum + " Hints";

			time = 0;
			Timer.Text = "0:0";
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
			++time;
			Timer.Text = time / 60 + ":" + time % 60;
		}

		private void Page_Loaded(object sender, RoutedEventArgs e)
		{
			if (started)
			{
				dispatcherTimer.Start();
			}
		}

		private void Page_Unloaded(object sender, RoutedEventArgs e)
		{
			dispatcherTimer.Stop();
		}
	}
}
