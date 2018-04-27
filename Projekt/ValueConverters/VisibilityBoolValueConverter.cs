using System;
using System.Globalization;
using System.Windows;

namespace Projekt
{
    public class VisibilityBoolValueConverter : BaseValueConverter<VisibilityBoolValueConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (bool) value ? Visibility.Visible : Visibility.Hidden;
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}