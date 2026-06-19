using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows;
using System.Windows.Data;

namespace TsTimeline
{
    internal class ViewportDirectionConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values[0] is Direction direction)
            {
                if (direction == Direction.Left || direction == Direction.Right)
                {
                    return values[2];
                }
                if (direction == Direction.Up || direction == Direction.Down)
                {
                    return values[1];
                }
            }
            return DependencyProperty.UnsetValue;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
