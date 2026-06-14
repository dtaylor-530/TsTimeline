using System;
using System.Collections.Generic;
using System.Text;

namespace Renderers
{
    public sealed class TimeLabelFormatter : ILabelFormatter
    {
        public string Format(double value)
        {
            var ts = TimeSpan.FromSeconds(value);

            if (ts.TotalHours >= 1)
                return ts.ToString(@"hh\:mm\:ss");

            if (ts.TotalMinutes >= 1)
                return ts.ToString(@"mm\:ss");

            return ts.ToString(@"ss\.fff");
        }
    }
}
