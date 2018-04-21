
using System;
using System.Globalization;
using System.Windows;

namespace Projekt

{
    /// <summary>
    /// A converter that takes in the core horizontal alignment enum and converts it to a WPF alignment
    /// </summary>
    public class VerticalAlignmentConverter : BaseValueConverter<HorizontalAlignmentConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (VerticalAlignment)value;
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
