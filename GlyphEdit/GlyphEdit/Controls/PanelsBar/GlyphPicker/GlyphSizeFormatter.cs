using System;
using System.Globalization;
using System.Windows.Data;
using Microsoft.Xna.Framework;

namespace GlyphEdit.Controls.PanelsBar.GlyphPicker
{
    public class GlyphSizeFormatter
    : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Point point)
                return $"({point.X}x{point.Y})";
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
