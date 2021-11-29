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

		private void dark_Selected(object sender, RoutedEventArgs e)
		{
			(Application.Current as App).ChangeSkin(Skin.Dark);
		}

		private void default_Selected(object sender, RoutedEventArgs e)
		{
			(Application.Current as App).ChangeSkin(Skin.Default);
		}
	}
}
