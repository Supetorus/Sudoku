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

		Page SettingsView = new SettingsView();
		Page GameView = new GameView();
		Page HomeView = new HomeView();
		Page NewGameView = new NewGameView();
		public GameWindow()
		{
			InitializeComponent();
		}

		private void btnSettings_Click(object sender, RoutedEventArgs e)
		{
			mainFrame.Content = SettingsView;
		}

		private void btnHome_Click(object sender, RoutedEventArgs e)
		{
			mainFrame.Content = HomeView;
		}

		private void btnGame_Click(object sender, RoutedEventArgs e)
		{
			mainFrame.Content = GameView;
		}

		private void btnNewGame_Click(object sender, RoutedEventArgs e)
		{
			mainFrame.Content = NewGameView;
		}
	}
}
