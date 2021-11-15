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
    /// <summary>
    /// Interaction logic for GameWindow.xaml
    /// </summary>
    public partial class GameWindow : Window
    {
        Button[,] shownButtons = new Button[9, 9];
        public GameWindow()
        {
            InitializeComponent();

            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    shownButtons[i,j] = new Button();
                    shownButtons[i, j].Content = i.ToString() +":"+ j.ToString();
                    Grid.SetRow(shownButtons[i, j], i);
                    Grid.SetColumn(shownButtons[i, j], j);
                    gridView.Children.Add(shownButtons[i, j]);
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
