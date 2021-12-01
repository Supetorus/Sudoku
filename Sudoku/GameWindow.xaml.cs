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

		public GameWindow()
		{
			InitializeComponent();
			ViewManager.RegisterWindow(this);
			//Background = Theme.selectedTheme.BackgroundColor;
		}

		internal void SetView(Page view)
		{
			mainFrame.Content = view;
		}

		public void Back()
		{
			if (mainFrame.NavigationService.CanGoBack)
			{
				mainFrame.NavigationService.GoBack();
			}
		}

		//These are temporary for debugging and quickly navigating.
		private void btnSettings_Click(object sender, RoutedEventArgs e)
		{
			mainFrame.Content = ViewManager.SettingsView;
		}

		private void btnHome_Click(object sender, RoutedEventArgs e)
		{
			mainFrame.Content = ViewManager.HomeView;
		}

		private void btnGame_Click(object sender, RoutedEventArgs e)
		{
			mainFrame.Content = ViewManager.GameView;
		}

		private void btnNewGame_Click(object sender, RoutedEventArgs e)
		{
			mainFrame.Content = ViewManager.NewGameView;
		}
	}
}