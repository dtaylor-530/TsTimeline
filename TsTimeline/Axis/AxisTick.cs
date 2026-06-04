using System;
using System.Collections.Generic;
using System.Text;

namespace TsTimeline
{
    public readonly struct AxisTick
    {
        public double Value { get; }
        public double ScreenPosition { get; }

        public TickLevel Level { get; }

        public bool HasLabel =>
            Level == TickLevel.Major;

        public AxisTick(
            double value,
            double screenPosition,
            TickLevel level)
        {
            Value = value;
            ScreenPosition = screenPosition;
            Level = level;
        }
    }
}
