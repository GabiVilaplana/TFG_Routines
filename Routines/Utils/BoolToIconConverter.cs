using System;
using System.Globalization;
using Microsoft.Maui.Controls;

namespace Routines.Utils
{
    public class BoolToIconConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value is bool hecho && hecho ? "❌" : "✅";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) =>
            throw new NotImplementedException();
    }
}
