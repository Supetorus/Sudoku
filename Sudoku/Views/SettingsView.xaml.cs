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
			cmbxTheme.SelectedIndex = (int) App.settings.Skin;
			soundToggle.Content = App.settings.SoundsOn ? "On" : "Off";
			cmbxSymbols.SelectedIndex = (int) App.settings.Symbol;
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
			App.settings.Symbol = 0;
		}

		private void letters_Selected(object sender, RoutedEventArgs e)
		{
			App.settings.Symbol = 1;
		}

		private void shapes_Selected(object sender, RoutedEventArgs e)
		{
			App.settings.Symbol = 2;
		}

		private void Sound_Checked(object sender, RoutedEventArgs e)
		{
			soundToggle.Content = "On";
			//Game.sound = true;
			App.settings.SoundsOn = true;
		}

		private void Sound_Unchecked(object sender, RoutedEventArgs e)
		{
			soundToggle.Content = "Off";
			//Game.sound = false;
			App.settings.SoundsOn = false;
		}

		private void Mistakes_Checked(object sender, RoutedEventArgs e)
		{
			mistakesToggle.Content = "On";
		}

		private void Mistakes_Unchecked(object sender, RoutedEventArgs e)
		{
			mistakesToggle.Content = "Off";

		}
	}
}
