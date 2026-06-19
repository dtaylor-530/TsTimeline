using System;
using System.Collections;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace SandBox
{
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
}


