using System;
using System.Collections.Generic;
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
		Random rng = new Random();

		Button[,] shownButtons = new Button[9, 9];

		Button selectedButton = null;

		Game game;

		// Set the default theme
		Theme theme = Theme.themes[0];

		struct Vector2
		{
			public int x, y;

			public Vector2(int x, int y)
			{
				this.x = x;
				this.y = y;
			}
		}

		List<Vector2> unsolved = new List<Vector2>();

		int hintNum = 3;

		bool notes = false;

		public GameView()
		{
			InitializeComponent();
			GenerateGrid();
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

		Stack<Move> moves = new Stack<Move>();

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
					shownButtons[x, y].FontSize = 15;

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
					}
					else
					{
						shownButtons[x, y].Content = num;
					}
				}
			}
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
					shownButtons[x, y].Background = theme.unselectedColor;

					double thickness = 1;
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
					border.BorderBrush = theme.BorderColor;
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
				Highlight(false);
			}

			selectedButton = sender as Button;

			Highlight(true);
		}

		private void Highlight(bool highlight)
		{
			Brush selectedBrush = highlight ? theme.selectedColor : theme.unselectedColor;
			Brush areaBrush = highlight ? theme.areaColor : theme.unselectedColor;
			Brush matchingBrush = highlight ? theme.matchingColor : theme.unselectedColor;

			//Highlight row and column
			for (int i = 0; i < 9; ++i)
			{
				shownButtons[i, GetCellInfo(selectedButton).y].Background = areaBrush;
				shownButtons[GetCellInfo(selectedButton).x, i].Background = areaBrush;
			}

			int x = GetCellInfo(selectedButton).x;
			int y = GetCellInfo(selectedButton).y;

			//Highlight box
			for (int bx = x - (x % 3); bx < x - (x % 3) + 3; ++bx)
			{
				for (int by = y - (y % 3); by < y - (y % 3) + 3; ++by)
				{
					shownButtons[bx, by].Background = areaBrush;
				}
			}

			//Highlight matching numbers
			if (HasNum(selectedButton))
			{
				int num = int.Parse(selectedButton.Content.ToString());
				for (int i = 0; i < 9; ++i)
				{
					for (int j = 0; j < 9; ++j)
					{
						if (HasNum(shownButtons[i, j]) && int.Parse(shownButtons[i, j].Content.ToString()) == num)
						{
							shownButtons[i, j].Background = matchingBrush;
						}
					}
				}
			}

			selectedButton.Background = selectedBrush;
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
			shownButtons[x, y].Content = num > 0 ? num : "";
			((CellInfo)shownButtons[x, y].Tag).correct = true;
			game.board.SetNum(x, y, num);
			unsolved.Remove(new Vector2(x, y));

			//remove this number from notes in area
			for (int i = 0; i < 9; ++i)
			{
				if (!HasNum(shownButtons[x, i]))
				{
					foreach (UIElement e in GetGrid(shownButtons[x, i]).Children)
					{
						if (Grid.GetColumn(e) == (num - 1) % 3 && Grid.GetRow(e) == (num - 1) / 3)
						{
							GetGrid(shownButtons[x, i]).Children.Remove(e);
							shownButtons[x, i].Content = GetGrid(shownButtons[x, i]);
							break;
						}
					}
				}

				if (!HasNum(shownButtons[i, y]))
				{
					foreach (UIElement e in GetGrid(shownButtons[i, y]).Children)
					{
						if (Grid.GetColumn(e) == (num - 1) % 3 && Grid.GetRow(e) == (num - 1) / 3)
						{
							GetGrid(shownButtons[i, y]).Children.Remove(e);
							shownButtons[i, y].Content = GetGrid(shownButtons[i, y]);
							break;
						}
					}
				}
			}

			for (int bx = x - (x % 3); bx < x - (x % 3) + 3; ++bx)
			{
				for (int by = y - (y % 3); by < y - (y % 3) + 3; ++by)
				{
					foreach (UIElement e in GetGrid(shownButtons[x, y]).Children)
					{
						if (Grid.GetColumn(e) == (num - 1) % 3 && Grid.GetRow(e) == (num - 1) / 3)
						{
							GetGrid(shownButtons[x, y]).Children.Remove(e);
							break;
						}
					}
				}
			}
		}

		public void AddNote(int x, int y, int num)
		{
			//if (board.CheckSafety(x, y, num))
			//{
				TextBlock txt = new TextBlock();
				txt.Text = num.ToString();
				txt.Foreground = theme.RightColor;
				txt.FontSize = 10;
				txt.VerticalAlignment = VerticalAlignment.Center;
				txt.HorizontalAlignment = HorizontalAlignment.Center;
				Grid.SetColumn(txt, (num - 1) % 3);
				Grid.SetRow(txt, (num - 1) / 3);
				GetGrid(selectedButton).Children.Add(txt);
			//}
		}

		public void Erase(object sender, RoutedEventArgs e)
		{
			if(selectedButton != null && !GetCellInfo(selectedButton).correct)
			{
				if(selectedButton.Content.GetType() == typeof(Grid))
				{
					GetCellInfo(selectedButton).grid.Children.Clear();
					selectedButton.Content = GetCellInfo(selectedButton).grid;
				}
				else
				{
					selectedButton.Content = "";
				}
			}
		}

		private void Window_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.Key >= Key.D1 && e.Key <= Key.D9 && selectedButton != null && !GetCellInfo(selectedButton).correct)
			{
				int num = int.Parse(e.Key.ToString().Replace('D', ' ').Trim());

				int x = GetCellInfo(selectedButton).x;
				int y = GetCellInfo(selectedButton).y;

				if(!notes)
				{
					moves.Push(new Move(x, y, selectedButton.Content));

				if (game.board.CheckNum(x, y, num))
				{
						selectedButton.Foreground = theme.RightColor;
						AddNumber(x, y, num);
					}
					else 
					{ 
						selectedButton.Foreground = theme.WrongColor;
						selectedButton.Content = num;
					}
				}
				else
				{
					AddNote(x, y, num);
				}
			}
		}

		private void KeyPad(object sender, RoutedEventArgs e)
		{
			if (selectedButton != null && !GetCellInfo(selectedButton).correct)
			{
				int num = int.Parse((sender as Button).Content.ToString());

				int x = GetCellInfo(selectedButton).x;
				int y = GetCellInfo(selectedButton).y;

				if (!notes)
				{
					moves.Push(new Move(x, y, selectedButton.Content));

				if (game.board.CheckNum(x, y, num))
				{
						selectedButton.Foreground = theme.RightColor;
						AddNumber(x, y, num);
					}
					else 
					{
						selectedButton.Foreground = theme.WrongColor;
						selectedButton.Content = num;
					}
				}
				else if(!HasNum(selectedButton))
				{
					AddNote(x, y, num);
				}
			}
		}

		private void Undo(object sender, RoutedEventArgs e)
		{
			if (moves.Count > 0)
			{
				Move move = moves.Pop();
				
				shownButtons[move.x, move.y].Content = move.prev;
				GetCellInfo(shownButtons[move.x, move.y]).correct = false;
			}
		}

		private void Hint(object sender, RoutedEventArgs e)
		{
			if (hintNum > 0 && unsolved.Count > 0)
			{
				int i = rng.Next(unsolved.Count - 1);
				Vector2 v = unsolved[i];
				moves.Push(new Move(v.x, v.y, shownButtons[v.x, v.y].Content.ToString() != "" ? int.Parse(shownButtons[v.x, v.y].Content.ToString()) : 0));
				AddNumber(v.x, v.y, game.board.GetCorrectNum(v.x, v.y));
				--hintNum;
			}
		}

		private void Notes(object sender, RoutedEventArgs e)
		{
			if(notes)
			{
				(sender as Button).Background = theme.unselectedColor;
			}
			else
			{
				(sender as Button).Background = theme.selectedColor;
			}

			notes = !notes;
		}

		private void Reset_Board(object sender, RoutedEventArgs e)
		{
			game.board.ResetBoard();
			for (int x = 0; x < 9; x++)
			{
				for (int y = 0; y < 9; y++)
				{
					int num = game.board.GetNum(x, y);
					shownButtons[x, y].Content = num == 0 ? "" : num;
				}
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
	}
}
