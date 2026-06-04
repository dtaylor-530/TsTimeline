using System.Collections.Generic;
using System.Windows.Media;

namespace TsTimeline
{
    public sealed class ChartSeries
    {
        public IList<ChartPoint> Values { get; } = new List<ChartPoint>();
        public Brush Stroke { get; set; } = Brushes.DodgerBlue;
        public double StrokeThickness { get; set; } = 1.5;
    }
}