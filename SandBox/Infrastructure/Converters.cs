using Flags.Icons;

namespace SandBox
{
    internal class DefaultConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        public static DefaultConverter Instance { get; } = new();
    }

    internal class ChildrenConverter : IValueConverter
    {
        public class CountryName
        {
            public string Name { get; set; }
        }
        public class Flag
        {
            public string Name { get; set; }
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (targetType == typeof(IEnumerable))
            {
                if (value is Country country)
                {
                    return new object[]
                    {
                        new Flag() { Name = country.ISO2 } ,
                               new CountryName() { Name = country.Name },
                    };
                }
            }
            else
            {

            }
            return DependencyProperty.UnsetValue;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    internal class PanelTypeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is ChartType chartType)
            {
                switch (chartType)
                {
                    case ChartType.Points:
                        return PanelType.Canvas;
                    case ChartType.Bands:
                        return PanelType.DirectionalStackPanel;
                    case ChartType.None:
                        break;
                    default:
                        return PanelType.None;
                }
            }
            return DependencyProperty.UnsetValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    internal class LogConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is double d)
                return Math.Log10(d);
            return DependencyProperty.UnsetValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is double d)
                return Math.Pow(10, d);
            return DependencyProperty.UnsetValue;
        }

        //public static double ToLogScale(double linearValue,
        //                        double minValue,
        //                        double maxValue)
        //{
        //    double minLog = Math.Log10(minValue);
        //    double maxLog = Math.Log10(maxValue);

        //    double scale = (maxLog - minLog) * linearValue + minLog;

        //    return Math.Pow(10, scale);
        //}
    }

    internal class BasicValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int intValue)
                return intValue;
            if (value is double doubleValue)
                return doubleValue.ToString("F1");
            return DependencyProperty.UnsetValue;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    internal class ValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int intValue)
                return TimeSpan.FromSeconds(intValue).ToString(@"mm\:ss\.f");
            if (value is double doubleValue)
                return TimeSpan.FromSeconds(doubleValue).ToString(@"mm\:ss\.f");
            return DependencyProperty.UnsetValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

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

    public class DirectionToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Direction direction)
                return direction == Direction.None ? Visibility.Collapsed : Visibility.Visible;
            return DependencyProperty.UnsetValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }


    public class IdToEnumConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (Enum.TryParse<TwemojiFlag>(value.ToString(), out var x))
                return x;
            return TwemojiFlag.UN;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class AdditionConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values[0] is double one && values[1] is double two)
            {
                return one + two;
            }
            return DependencyProperty.UnsetValue;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}


