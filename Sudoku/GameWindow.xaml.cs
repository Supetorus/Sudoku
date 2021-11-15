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

            board = new Board();

            board.Generate();

            for (int x = 0; x < 9; x++)
            {
                for (int y = 0; y < 9; y++)
                {
                    int num = board.GetNum(x, y);
                    shownButtons[x, y] = new Button();
                    shownButtons[x, y].Content = num == 0 ? "" : num;
                    shownButtons[x, y].Click += BoardClick;
                    shownButtons[x, y].Tag = new CellInfo(x, y, num != 0);
                    Grid.SetRow(shownButtons[x, y], x);
                    Grid.SetColumn(shownButtons[x, y], y);
                    gridView.Children.Add(shownButtons[x, y]);
                }
            }
        }

        private void BoardClick(object sender, RoutedEventArgs e)
        {
            selectedButton = sender as Button;
        }
        
		private void Window_KeyDown(object sender, KeyEventArgs e)
		{
            if (e.Key >= Key.D1 && e.Key <= Key.D9 && selectedButton != null && !((CellInfo)selectedButton.Tag).correct)
			{
                int num = int.Parse(e.Key.ToString().Replace('D', ' ').Trim());
                selectedButton.Content = num;

                //correct num checking here
			}
		}
	}
            }

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
    }
}
