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
	/// <summary>
	/// Interaction logic for SettingsView.xaml
	/// </summary>
	public partial class SettingsView : Page
	{
		public SettingsView()
		{
			InitializeComponent();
		}

		private void neon_Selected(object sender, RoutedEventArgs e)
		{
			(Application.Current as App).ChangeSkin(Skin.Neon);
		}

		private void mocha_Selected(object sender, RoutedEventArgs e)
		{
			(Application.Current as App).ChangeSkin(Skin.Mocha);
		}

		private void dark_Selected(object sender, RoutedEventArgs e)
		{
			(Application.Current as App).ChangeSkin(Skin.Dark);
		}

		private void default_Selected(object sender, RoutedEventArgs e)
		{
			(Application.Current as App).ChangeSkin(Skin.Default);
		}

		private void dream_Selected(object sender, RoutedEventArgs e)
		{
			(Application.Current as App).ChangeSkin(Skin.Dream);
		}

		private void coral_Selected(object sender, RoutedEventArgs e)
		{
			(Application.Current as App).ChangeSkin(Skin.Coral);
		}

		private void mint_Selected(object sender, RoutedEventArgs e)
		{
			(Application.Current as App).ChangeSkin(Skin.Mint);
		}

		private void phoenix_Selected(object sender, RoutedEventArgs e)
		{
			(Application.Current as App).ChangeSkin(Skin.Phoenix);
		}

		private void Back_Click(object sender, RoutedEventArgs e)
		{
			ViewManager.Back();
		}

		private void numbers_Selected(object sender, RoutedEventArgs e)
		{
			GameView.symbol = 0;
		}

		private void letters_Selected(object sender, RoutedEventArgs e)
		{
			GameView.symbol = 1;
		}

		private void shapes_Selected(object sender, RoutedEventArgs e)
		{
			GameView.symbol = 2;
		}

		private void ToggleButton_Checked(object sender, RoutedEventArgs e)
		{
			soundToggle.Content = "On";
			Game.sound = true;
		}

		private void ToggleButton_Unchecked(object sender, RoutedEventArgs e)
		{
			soundToggle.Content = "Off";
			Game.sound = false;
		}
	}
}
