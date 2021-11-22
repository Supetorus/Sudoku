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

		public Theme(Brush selectedColor, Brush matchingColor, Brush areaColor, 
			Brush unselectedColor, Brush WrongColor, Brush RightColor)
		{
			this.selectedColor = selectedColor;
			this.matchingColor = matchingColor;
			this.areaColor = areaColor;
			this.unselectedColor = unselectedColor;
			this.WrongColor = WrongColor;
			this.RightColor = RightColor;
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
				new SolidColorBrush(Color.FromRgb(000, 100, 200))
			)
		};
	}
}
