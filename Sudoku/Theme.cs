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
		public Brush selectedColor;
		public Brush matchingColor;
		public Brush areaColor;
		public Brush DefaultTileColor;
		public Brush WrongColor;
		public Brush RightColor;
		public Brush BorderColor;
		public Brush BackgroundColor;
		public Brush DefaultText;

		public Theme(Brush selectedColor, Brush matchingColor, Brush areaColor,
			Brush DefaultTileColor, Brush WrongColor, Brush RightColor, Brush BorderColor, Brush BackgroundColor, Brush DefaultText)
		{
			this.selectedColor = selectedColor;
			this.matchingColor = matchingColor;
			this.areaColor = areaColor;
			this.DefaultTileColor = DefaultTileColor;
			this.WrongColor = WrongColor;
			this.RightColor = RightColor;
			this.BorderColor = BorderColor;
			this.BackgroundColor = BackgroundColor;
			this.DefaultText = DefaultText;
		}

		public readonly static Theme[] themes =
		{
			new Theme // default / testing
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
			new Theme // dark theme
			(
				new SolidColorBrush(Color.FromRgb(255, 032, 110)),// selected color
				new SolidColorBrush(Color.FromRgb(065, 234, 200)),// matching color
				new SolidColorBrush(Color.FromRgb(255, 255, 255)),// area color
				new SolidColorBrush(Color.FromRgb(012, 015, 010)),// default tile color
				new SolidColorBrush(Color.FromRgb(255, 000, 000)),// wrong color
				new SolidColorBrush(Color.FromRgb(251, 255, 018)),// right color
				new SolidColorBrush(Color.FromRgb(065, 234, 200)),// bordercolor
				new SolidColorBrush(Color.FromRgb(077, 077, 077)),// background color
				new SolidColorBrush(Color.FromRgb(145, 145, 145)) // default text color

			),
			new Theme // mocha theme
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
			new Theme // the bluez theme
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
			)
		};

		public static Theme selectedTheme = themes[1];
	}
}
