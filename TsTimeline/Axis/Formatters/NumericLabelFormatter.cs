using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace TsTimeline
{
    public sealed class NumericLabelFormatter : ILabelFormatter
    {
        public string Format(double value)
        {
            return value.ToString("0.###", CultureInfo.InvariantCulture);
        }
    }
}
