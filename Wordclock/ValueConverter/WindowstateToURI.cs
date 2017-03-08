using System;
using System.Globalization;
using System.Windows;
using PropertyChanged;

namespace Wordclock
{
    /// <summary>
    /// A converter that takes in a window state and returns a image
    /// </summary>
    public class WindowstateToURI : BaseValueConverter<WindowstateToURI>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var MinWMaxImage = new Uri("Images/maximize.png", UriKind.Relative);

            if (value == null)
                return MinWMaxImage;

            if(((WindowViewModel)value).StateOfWindow == WindowState.Maximized)
                MinWMaxImage = new Uri("Images/minimize.png", UriKind.Relative);

            return MinWMaxImage;
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
