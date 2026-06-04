using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Data;

namespace TsTimeline.Axis
{
    public sealed class ConverterFormatter
            : ILabelFormatter
    {
        private readonly IValueConverter _converter;

        public ConverterFormatter(
            IValueConverter converter)
        {
            _converter = converter;
        }

        public string Format(double value)
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
