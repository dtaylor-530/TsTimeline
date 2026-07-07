using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Data;

namespace Views
{
    public enum Direction
    {
        None, Up, Down, Left, Right
    }

    public class DirectionConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Direction direction)

                if (targetType == typeof(double))
                {
                    if (parameter.ToString().Equals("Angle", StringComparison.InvariantCultureIgnoreCase))
                    {
                        return direction switch
                        {
                            Direction.Down => 0d,
                            Direction.Up => 180d,
                            Direction.Right => -90d,
                            Direction.Left => 90d,
                            _ => 0d
                        };
                    }               
                }

            return DependencyProperty.UnsetValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }


        public static DirectionConverter Instance { get; } = new();
    }
}
