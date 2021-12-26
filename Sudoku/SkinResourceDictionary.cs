using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Sudoku
{
	public class SkinResourceDictionary : ResourceDictionary
	{
		private Uri _defaultSource;
		private Uri _darkSource;
		private Uri _neonSource;
		private Uri _mochaSource;
		private Uri _dreamSource;
		private Uri _coralSource;
		private Uri _mintSource;
		private Uri _phoniexSource;

		public Uri DefaultSource
		{
			get { return _defaultSource; }
			set
			{
				_defaultSource = value;
				UpdateSource();
			}
		}
		public Uri DarkSource
		{
			get { return _darkSource; }
			set
			{
				_darkSource = value;
				UpdateSource();
			}
		}
		public Uri NeonSource
		{
			get { return _neonSource; }
			set
			{
				_neonSource = value;
				UpdateSource();
			}
		}

		public Uri MochaSource
		{
			get { return _mochaSource; }
			set
			{
				_mochaSource = value;
				UpdateSource();
			}
		}
		public Uri DreamSource
		{
			get { return _dreamSource; }
			set
			{
				_dreamSource = value;
				UpdateSource();
			}
		}
		public Uri CoralSource
		{
			get { return _coralSource; }
			set
			{
				_coralSource = value;
				UpdateSource();
			}
		}
		public Uri MintSource
		{
			get { return _mintSource; }
			set
			{
				_mintSource = value;
				UpdateSource();
			}
		}

		public Uri PhoniexSource
		{
			get { return _phoniexSource; }
			set
			{
				_phoniexSource = value;
				UpdateSource();
			}
		}

		public void UpdateSource()
		{
			Uri val;
			switch (App.settings.Skin)
			{
				case Skin.Default:
					val = DefaultSource;
					break;
				case Skin.Dark:
					val = DarkSource;
					break;
				case Skin.Neon:
					val = NeonSource;
					break;
				case Skin.Mocha:
					val = MochaSource;
					break;
				case Skin.Dream:
					val = DreamSource;
					break;
				case Skin.Coral:
					val = CoralSource;
					break;
				case Skin.Mint:
					val = MintSource;
					break;
				case Skin.Phoenix:
					val = PhoniexSource;
					break;
				default:
					val = DefaultSource;
					break;
			}
			if (val != null && base.Source != val)
			{
				Source = val;
			}
		}
	}
}
