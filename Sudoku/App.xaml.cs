using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Sudoku
{
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public enum Skin 
	{
		Default = 0,
		Dark,
		Neon,
		Mocha,
		Dream,
		Coral,
		Mint,
		Phoenix
	}

	public partial class App : Application
	{
		public static Skin Skin { get; set; }

		public void ChangeSkin(Skin newSkin)
		{
			Skin = newSkin;

			foreach (ResourceDictionary dict in Resources.MergedDictionaries)
			{

				if (dict is SkinResourceDictionary skinDict)
					skinDict.UpdateSource();
				else
					dict.Source = dict.Source;
			}
		}

		private void Application_Startup(object sender, StartupEventArgs e)
		{
			if (File.Exists("Theme.txt"))
			{
				string theme = FileIO.LoadText("Theme.txt");
				Enum.TryParse(theme, out Skin skin);
				ChangeSkin(skin);
			}
		}

		private async void Application_Exit(object sender, ExitEventArgs e)
		{
			await FileIO.SaveText(Skin.ToString(), "Theme.txt");
		}
	}
}
