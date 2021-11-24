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
		public Brush unselectedColor;
		public Brush WrongColor;
		public Brush RightColor;
		public Brush BorderColor;

		public Theme(Brush selectedColor, Brush matchingColor, Brush areaColor,
			Brush unselectedColor, Brush WrongColor, Brush RightColor, Brush BorderColor)
		{
			this.selectedColor = selectedColor;
			this.matchingColor = matchingColor;
			this.areaColor = areaColor;
			this.unselectedColor = unselectedColor;
			this.WrongColor = WrongColor;
			this.RightColor = RightColor;
			this.BorderColor = BorderColor;
		}
		public readonly static Theme[] themes =
		{
			new Theme
			(
				new SolidColorBrush(Color.FromRgb(200, 200, 255)),
				new SolidColorBrush(Color.FromRgb(255, 200, 200)),
				new SolidColorBrush(Color.FromRgb(200, 200, 215)),
				new SolidColorBrush(Color.FromRgb(200, 200, 200)),
				new SolidColorBrush(Color.FromRgb(255, 000, 000)),
				new SolidColorBrush(Color.FromRgb(000, 100, 200)),
				new SolidColorBrush(Color.FromRgb(000, 000, 000))
			),
			//dark theme
				new Theme
			(
					//selected color
				new SolidColorBrush(Color.FromRgb(1, 22, 39)),
				//matching color
				new SolidColorBrush(Color.FromRgb(65, 234, 200)),
				//area color
				new SolidColorBrush(Color.FromRgb(255, 255, 255)),
				// unseleted color
				new SolidColorBrush(Color.FromRgb(12, 15, 10)),
				//wrong color
				new SolidColorBrush(Color.FromRgb(255, 000, 000)),
				//right color
				new SolidColorBrush(Color.FromRgb(251, 255, 18)),
				//bordercolor
				new SolidColorBrush(Color.FromRgb(65, 234, 200))
			),
				//mocha theme
				new Theme
			(
					//selected color
				new SolidColorBrush(Color.FromRgb(117, 79, 91)),
				//matching color
				new SolidColorBrush(Color.FromRgb(93, 73, 84)),
				//area color
				new SolidColorBrush(Color.FromRgb(249, 224, 217)),
				// unseleted color
				new SolidColorBrush(Color.FromRgb(125, 97, 103)),
				//wrong color
				new SolidColorBrush(Color.FromRgb(255, 000, 000)),
				//right color
				new SolidColorBrush(Color.FromRgb(230, 219, 208)),
				//border color
				new SolidColorBrush(Color.FromRgb(93, 73, 84))
			),
				//the bluez theme
				new Theme
			(
					//selected color
				new SolidColorBrush(Color.FromRgb(231, 236, 239)),
				//matching color
				new SolidColorBrush(Color.FromRgb(139, 140, 137)),
				//area color
				new SolidColorBrush(Color.FromRgb(96, 150, 186)),
				// unseleted color
				new SolidColorBrush(Color.FromRgb(36, 76, 119)),
				//wrong color
				new SolidColorBrush(Color.FromRgb(255, 000, 000)),
				//right color
				new SolidColorBrush(Color.FromRgb(163, 206, 241)),
				//border color
				new SolidColorBrush(Color.FromRgb(139, 140, 137))
			),
				//the neon theme
				new Theme
			(
					//selected color
				new SolidColorBrush(Color.FromRgb(255, 254, 255)),
				//matching color
				new SolidColorBrush(Color.FromRgb(250, 255, 0)),
				//area color
				new SolidColorBrush(Color.FromRgb(255, 1, 251)),
				// unseleted color
				new SolidColorBrush(Color.FromRgb(2, 196, 234)),
				//wrong color
				new SolidColorBrush(Color.FromRgb(255, 000, 000)),
				//right color
				new SolidColorBrush(Color.FromRgb(163, 206, 241)),
				//border color
				new SolidColorBrush(Color.FromRgb(250, 255, 0))
			)
		};
	}
}
