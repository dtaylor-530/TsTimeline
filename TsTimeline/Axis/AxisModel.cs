using System;
using System.Collections.Generic;
using System.Text;

namespace TsTimeline
{
    public sealed class AxisModel
    {
        public List<AxisTick> Ticks { get; } = new();

        public double VisibleStart { get; init; }

        public double VisibleEnd { get; init; }

        public double PixelsPerUnit { get; init; }
    }
}
