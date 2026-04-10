using System;
using System.Globalization;

namespace Wordclock
{
    /// <summary>
    /// A converter that takes in a topmost boolean and returns the matching icon
    /// </summary>
    public class BoolToTopmostURI : BaseValueConverter<BoolToTopmostURI>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool isTopmost && isTopmost)
                return new Uri("/Images/OnTopmostOn.ico", UriKind.Relative);

            return new Uri("/Images/OnTopmostOff.ico", UriKind.Relative);
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
