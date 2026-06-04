using System;
using System.Collections.Generic;
using System.Text;

namespace TsTimeline
{
    public interface ILabelFormatter
    {
        string Format(double value);
    }
}
