using System;
using System.Globalization;
using Microsoft.Maui.Controls;

namespace Routines.Utils
{
    public class BoolToBackgroundConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value is bool hecho && hecho ? new Color(0.8f, 0.9f, 1f) : Colors.Transparent;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) =>
            throw new NotImplementedException();
    }
}
