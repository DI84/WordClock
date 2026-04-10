using System;
using System.Globalization;

namespace Wordclock
{
    /// <summary>
    /// A converter that takes in a maximized boolean and returns the matching icon
    /// </summary>
    public class WindowstateToURI : BaseValueConverter<WindowstateToURI>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool isMaximized && isMaximized)
                return new Uri("Images/minimize.png", UriKind.Relative);

            return new Uri("Images/maximize.png", UriKind.Relative);
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
