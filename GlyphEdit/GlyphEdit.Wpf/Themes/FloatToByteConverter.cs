using System;
using System.Globalization;
using System.Windows.Data;

namespace GlyphEdit.Wpf.Themes
{
    public class FloatToByteConverter
    : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ((int)((float) value * byte.MaxValue)).ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (float)int.Parse((string)value) / byte.MaxValue;
        }
    }
}
