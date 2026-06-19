using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows;
using System.Windows.Data;

namespace SandBox
{
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
}
