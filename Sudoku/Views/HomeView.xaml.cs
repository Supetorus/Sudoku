using System;
using System.Collections.Generic;
using System.IO;
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
	/// <summary>
	/// Interaction logic for HomeView.xaml
	/// </summary>
	public partial class HomeView : Page
	{
		public HomeView()
		{
			InitializeComponent();
		}

		private void btnNewGame_Click(object sender, RoutedEventArgs e)
		{
			ViewManager.SetView(ViewManager.NewGameView);
		}

		private void btnSettings_Click(object sender, RoutedEventArgs e)
		{
			ViewManager.SetView(ViewManager.SettingsView);
		}

		private void btnLoadGame_Click(object sender, RoutedEventArgs e)
		{
			if (!File.Exists("Game.bin")) MessageBox.Show("No previous game exists.");
			else
			{
				((GameView)ViewManager.GameView).LoadGame();
				ViewManager.SetView(ViewManager.GameView);
			}
		}
	}
}
