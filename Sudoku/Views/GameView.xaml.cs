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

		public GameView()
		{
			InitializeComponent();
			GenerateButtons();
		}

		class CellInfo
		{
			public int x;
			public int y;
			public bool correct;

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
			public int prev;

			public Move(int x, int y, int prev)
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
					shownButtons[x, y].Content = num == 0 ? "" : num;
					if (num == 0) { unsolved.Add(new Vector2(x, y)); }
					shownButtons[x, y].Click += BoardClick;
					shownButtons[x, y].Tag = new CellInfo(x, y, num != 0);
				}
			}
		}

		private void GenerateButtons()
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
				shownButtons[i, ((CellInfo)selectedButton.Tag).y].Background = areaBrush;
				shownButtons[((CellInfo)selectedButton.Tag).x, i].Background = areaBrush;
			}

			int x = ((CellInfo)selectedButton.Tag).x;
			int y = ((CellInfo)selectedButton.Tag).y;

			//Highlight box
			for (int bx = x - (x % 3); bx < x - (x % 3) + 3; ++bx)
			{
				for (int by = y - (y % 3); by < y - (y % 3) + 3; ++by)
				{
					shownButtons[bx, by].Background = areaBrush;
				}
			}

			//Highlight matching numbers
			if (selectedButton.Content.ToString() != "")
			{
				int num = int.Parse(selectedButton.Content.ToString());
				for (int i = 0; i < 9; ++i)
				{
					for (int j = 0; j < 9; ++j)
					{
						if (shownButtons[i, j].Content.ToString() != "" &&
							int.Parse(shownButtons[i, j].Content.ToString()) == num)
						{
							shownButtons[i, j].Background = matchingBrush;
						}
					}
				}
			}

			selectedButton.Background = selectedBrush;
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
		}

		public void ErasePosition(Vector position)
		{
			// Calls Board.Erase(position) then updates the display
		}

		private void Window_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.Key >= Key.D1 && e.Key <= Key.D9 && selectedButton != null && !((CellInfo)selectedButton.Tag).correct)
			{
				int num = int.Parse(e.Key.ToString().Replace('D', ' ').Trim());

				int x = ((CellInfo)selectedButton.Tag).x;
				int y = ((CellInfo)selectedButton.Tag).y;

				moves.Push(new Move(x, y, selectedButton.Content.ToString() != "" ? int.Parse(selectedButton.Content.ToString()) : 0));
				AddNumber(x, y, num);

				if (game.board.CheckNum(x, y, num))
				{
					selectedButton.Foreground = theme.RightColor;
				}
				else { selectedButton.Foreground = theme.WrongColor; }
			}
		}

		private void KeyPad(object sender, RoutedEventArgs e)
		{
			if (selectedButton != null && !((CellInfo)selectedButton.Tag).correct)
			{
				int num = int.Parse((sender as Button).Content.ToString());

				int x = ((CellInfo)selectedButton.Tag).x;
				int y = ((CellInfo)selectedButton.Tag).y;

				moves.Push(new Move(x, y, selectedButton.Content.ToString() != "" ? int.Parse(selectedButton.Content.ToString()) : 0));
				AddNumber(x, y, num);

				if (game.board.CheckNum(x, y, num))
				{
					selectedButton.Foreground = theme.RightColor;
				}
				else { selectedButton.Foreground = theme.WrongColor; }
			}
		}

		private void Undo(object sender, RoutedEventArgs e)
		{
			if (moves.Count > 0)
			{
				Move move = moves.Pop();
				AddNumber(move.x, move.y, move.prev);
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
