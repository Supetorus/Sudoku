using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Sudoku
{
	struct Theme
	{
		public Brush SelectedTileColor;
		public Brush MatchingTileColor;
		public Brush AreaTileColor;
		public Brush DefaultTileColor;
		public Brush WrongColor;
		public Brush RightColor;
		public Brush BorderColor;
		public Brush BackgroundColor;
		public Brush DefaultText;

		public Theme(Brush SelectedTileColor, Brush MatchingTileColor, Brush AreaTileColor,
			Brush DefaultTileColor, Brush WrongColor, Brush RightColor, Brush BorderColor, Brush BackgroundColor, Brush DefaultText)
		{
			this.SelectedTileColor = SelectedTileColor;
			this.MatchingTileColor = MatchingTileColor;
			this.AreaTileColor = AreaTileColor;
			this.DefaultTileColor = DefaultTileColor;
			this.WrongColor = WrongColor;
			this.RightColor = RightColor;
			this.BorderColor = BorderColor;
			this.BackgroundColor = BackgroundColor;
			this.DefaultText = DefaultText;
		}

		public readonly static Theme[] themes =
		{
			new Theme // [0] default / testing
			(
				new SolidColorBrush(Color.FromRgb(200, 200, 255)),// selected color
				new SolidColorBrush(Color.FromRgb(255, 200, 200)),// matching color
				new SolidColorBrush(Color.FromRgb(200, 200, 215)),// area color
				new SolidColorBrush(Color.FromRgb(200, 200, 200)),// default tile color
				new SolidColorBrush(Color.FromRgb(255, 000, 000)),// wrong color
				new SolidColorBrush(Color.FromRgb(000, 100, 200)),// right color
				new SolidColorBrush(Color.FromRgb(000, 000, 000)),// border color
				new SolidColorBrush(Color.FromRgb(255, 255, 255)),// background color
				new SolidColorBrush(Color.FromRgb(000, 000, 000)) // default text color
			),
			new Theme // [1] dark theme
			(
				new SolidColorBrush(Color.FromRgb(001, 022, 039)),// selected color
				new SolidColorBrush(Color.FromRgb(065, 234, 200)),// matching color
				new SolidColorBrush(Color.FromRgb(255, 255, 255)),// area color
				new SolidColorBrush(Color.FromRgb(012, 015, 010)),// default tile color
				new SolidColorBrush(Color.FromRgb(255, 000, 000)),// wrong color
				new SolidColorBrush(Color.FromRgb(251, 255, 018)),// right color
				new SolidColorBrush(Color.FromRgb(065, 234, 200)),// bordercolor
				new SolidColorBrush(Color.FromRgb(077, 077, 077)),// background color
				new SolidColorBrush(Color.FromRgb(145, 145, 145)) // default text color

			),
			new Theme // [2] mocha theme
			(
				new SolidColorBrush(Color.FromRgb(117, 079, 091)),// selected color
				new SolidColorBrush(Color.FromRgb(093, 073, 084)),// matching color
				new SolidColorBrush(Color.FromRgb(249, 224, 217)),// area color
				new SolidColorBrush(Color.FromRgb(125, 097, 103)),// default tile color
				new SolidColorBrush(Color.FromRgb(255, 000, 000)),// wrong color
				new SolidColorBrush(Color.FromRgb(230, 219, 208)),// right color
				new SolidColorBrush(Color.FromRgb(093, 073, 084)),// border color
				new SolidColorBrush(Color.FromRgb(102, 075, 037)),// background color
				new SolidColorBrush(Color.FromRgb(158, 127, 084)) // default text color
			),
			new Theme // [3] the bluez theme
			(
				new SolidColorBrush(Color.FromRgb(231, 236, 239)),// selected color
				new SolidColorBrush(Color.FromRgb(139, 140, 137)),// matching color
				new SolidColorBrush(Color.FromRgb(096, 150, 186)),// area color
				new SolidColorBrush(Color.FromRgb(036, 076, 119)),// default tile color
				new SolidColorBrush(Color.FromRgb(255, 000, 000)),// wrong color
				new SolidColorBrush(Color.FromRgb(163, 206, 241)),// right color
				new SolidColorBrush(Color.FromRgb(139, 140, 137)),// border color
				new SolidColorBrush(Color.FromRgb(019, 081, 148)),// background color
				new SolidColorBrush(Color.FromRgb(071, 135, 204)) // default text color
			),
				new Theme // [4] watermelon theme
			(
				new SolidColorBrush(Color.FromRgb(164, 194, 168)),//selected color
				new SolidColorBrush(Color.FromRgb(255, 234, 208)),//matching color
				new SolidColorBrush(Color.FromRgb(244, 91, 105)),//area color
				new SolidColorBrush(Color.FromRgb(135, 255, 101)),// default tile color
				new SolidColorBrush(Color.FromRgb(255, 000, 000)),//wrong color
				new SolidColorBrush(Color.FromRgb(90, 90, 102)),//right color
				new SolidColorBrush(Color.FromRgb(255, 234, 208)),//border color
				new SolidColorBrush(Color.FromRgb(247, 111, 142)),// background color
				new SolidColorBrush(Color.FromRgb(8, 45, 15)) // default text color
			),
				
				new Theme // [5] coral theme
			(
				new SolidColorBrush(Color.FromRgb(252, 177, 149)),//selected color
				new SolidColorBrush(Color.FromRgb(177, 182, 149)),//matching color
				new SolidColorBrush(Color.FromRgb(83, 145, 126)),//area color
				new SolidColorBrush(Color.FromRgb(234, 84, 111)),// unseleted color
				new SolidColorBrush(Color.FromRgb(255, 000, 000)),//wrong color
				new SolidColorBrush(Color.FromRgb(90, 90, 102)),//right color
				new SolidColorBrush(Color.FromRgb(244, 91, 105)),//border color
				new SolidColorBrush(Color.FromRgb(109, 26, 54)),// background color
				new SolidColorBrush(Color.FromRgb(91, 114, 109)) // default text color
			)
		};

		public static Theme selectedTheme = themes[0];
	}
}
