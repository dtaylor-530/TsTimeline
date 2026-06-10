using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows;
using System.Windows.Data;

namespace TsTimeline
{
    public enum Direction
    {
        Up, Down
    }

    internal class DirectionConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Direction direction)
            {
                return direction == Direction.Down ? 1 : -1;
            }
            return DependencyProperty.UnsetValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    internal class DirectionVerticalAlignmentConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Direction direction)
            {
                //    return direction == Direction.Down ? VerticalAlignment.Bottom : VerticalAlignment.Top;

                bool pinToBottom = direction == Direction.Up;

                return pinToBottom
                    ? 0.0
                    : DependencyProperty.UnsetValue;
            }
            return DependencyProperty.UnsetValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
