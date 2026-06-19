using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace SandBox
{
    public class WidthConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is double d)
                return new Size(d, 0);
            return DependencyProperty.UnsetValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Size d)
                return d.Width;
            return DependencyProperty.UnsetValue;
        }
    }
}
