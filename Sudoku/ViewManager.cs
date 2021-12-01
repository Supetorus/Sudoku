using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Sudoku
{
	public static class ViewManager
	{
		public static Page SettingsView = new SettingsView();
		public static Page GameView = new GameView();
		public static Page HomeView = new HomeView();
		public static Page NewGameView = new NewGameView();

		private static GameWindow gameWindow;

		public static void RegisterWindow(GameWindow window)
		{
			if (gameWindow == null) gameWindow = window;
		}

		public static void SetView(Page view)
		{
			gameWindow.SetView(view);
		}

		public static void Back()
		{
			gameWindow.Back();
		}
	}
}
