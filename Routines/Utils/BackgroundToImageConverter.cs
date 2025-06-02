using System;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Storage;
using System.Globalization;

namespace Routines.Utils
{
    public class BackgroundToImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var bg = Session.UsuarioActual?.Background ?? "Blue";

            return bg switch
            {
                "Blue" => "blue.png",
                "Green" => "green.png",
                "Red" => "red.png",
                "Yellow" => "yellow.png",
                "Orange" => "orange.png",
                "Purple" => "purple.png",
                _ => "blue.png"
            };
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
