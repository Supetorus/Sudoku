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
	{// These must be listed in the same order as they are in SettingsView
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
		[Serializable]
		public class Settings
		{
			public Skin Skin { get; set; } = Skin.Default;
			public int Symbol { get; set; } = 0;
			public bool SoundsOn { get; set; } = true;
			public bool CheckMistakes { get; set; } = true;
		}

		public static Settings settings;

		//public static Skin Skin { get; set; }

		public void ChangeSkin(Skin newSkin)
		{
			settings.Skin = newSkin;

			foreach (ResourceDictionary dict in Resources.MergedDictionaries)
			{

				if (dict is SkinResourceDictionary skinDict)
					skinDict.UpdateSource();
				else
					dict.Source = dict.Source;
			}
		}

		public App()
		{
			if (File.Exists("Settings.bin"))
				settings = FileIO.Load<Settings>("Settings.bin");
			else settings = new Settings();
		}

		//private void Application_Startup(object sender, StartupEventArgs e)
		//{
		//	if (File.Exists("Settings.bin"))
		//		settings = FileIO.Load<Settings>("Settings.txt");
		//	else settings = new Settings();

		//	//if (File.Exists("Theme.txt"))
		//	//{
		//	//	string theme = FileIO.LoadText("Theme.txt");
		//	//	Enum.TryParse(theme, out Skin skin);
		//	//	ChangeSkin(skin);
		//	//}
		//}

		private void Application_Exit(object sender, ExitEventArgs e)
		{
			FileIO.Save("Settings.bin", settings);
			//await FileIO.SaveText(Skin.ToString(), "Theme.txt");
		}
	}
}
