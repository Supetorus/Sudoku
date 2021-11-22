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
using System.Windows.Shapes;

namespace Sudoku
{
	public partial class GameWindow : Window
	{
		Button[,] shownButtons = new Button[9, 9];

		Button selectedButton = null;

		Board board;

		static Theme[] themes =
		{
			new Theme
			(
				new SolidColorBrush(Color.FromRgb(200, 200, 255)),
				new SolidColorBrush(Color.FromRgb(255, 200, 200)),
				new SolidColorBrush(Color.FromRgb(200, 200, 215)),
				new SolidColorBrush(Color.FromRgb(200, 200, 200))
			)
		};

		// Set the default theme
		Theme theme = themes[0];

		struct Theme
		{
			public Brush selectedColor;
			public Brush matchingColor;
			public Brush areaColor;
			public Brush unselectedColor;

			public Theme(Brush selectedColor, Brush matchingColor, Brush areaColor, Brush unselectedColor)
			{
				this.selectedColor = selectedColor;
				this.matchingColor = matchingColor;
				this.areaColor = areaColor;
				this.unselectedColor = unselectedColor;
			}
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

		public GameWindow()
		{
			InitializeComponent();

			GenerateButtons();
			NewGame();
		}

		private void NewGame()
		{
			board = new Board();
			board.Generate();
			for (int x = 0; x < 9; x++)
			{
				for (int y = 0; y < 9; y++)
				{
					int num = board.GetNum(x, y);
					shownButtons[x, y].Content = num == 0 ? "" : num;
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

		public void AddNumber(Vector position)
		{
			//Attempts to add a number to the board, updates the display based on whether the number was right, wrong, or attempted in a place where a number already existed.
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
				selectedButton.Content = num;

				int x = ((CellInfo)selectedButton.Tag).x;
				int y = ((CellInfo)selectedButton.Tag).y;
				if (board.CheckNum(x, y, num))
				{
					selectedButton.Foreground = Brushes.Green;
					board.SetNum(x, y, num);
				}
				else { selectedButton.Foreground = Brushes.Red; }
			}
		}

		private void KeyPad(object sender, RoutedEventArgs e)
		{
			int num = int.Parse((sender as Button).Content.ToString());

			if (selectedButton != null && !((CellInfo)selectedButton.Tag).correct)
			{
				selectedButton.Content = num;

				int x = ((CellInfo)selectedButton.Tag).x;
				int y = ((CellInfo)selectedButton.Tag).y;
				if (board.CheckNum(x, y, num))
				{
					selectedButton.Foreground = Brushes.Green;
					board.SetNum(x, y, num);
				}
				else { selectedButton.Foreground = Brushes.Red; }
			}
		}

		private void Reset_Board(object sender, RoutedEventArgs e)
		{
			board.ResetBoard();
			for (int x = 0; x < 9; x++)
			{
				for (int y = 0; y < 9; y++)
				{
					int num = board.GetNum(x, y);
					shownButtons[x, y].Content = num == 0 ? "" : num;
				}
			}
		}

		private void New_Game(object sender, RoutedEventArgs e)
		{
			NewGame();
		}
	}
}

