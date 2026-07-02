using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Data;

namespace Renderers
{
    public sealed class ConverterFormatter(IValueConverter _converter) : IFormatter
    {
        public string Format(object value)
        {
            return
                _converter.Convert(
                    value,
                    typeof(string),
                    null,
                    null)?.ToString()
                ?? value.ToString();
        }
    }
}
