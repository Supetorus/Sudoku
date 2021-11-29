using System;
using System.Collections.Generic;
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

        private void UpdateSource()
        {
            Uri val;
            switch (App.Skin)
            {
                case Skin.Default:
                    val = DefaultSource;
                    break;
                case Skin.Dark:
                    val = DarkSource;
                    break;
                default:
                    val = DefaultSource;
                    break;
            }
            if (val != null && base.Source != val)
                base.Source = val;
        }
    }
}
