using System.Windows;
using System.Windows.Media;

namespace TsTimeline
{
    public sealed class AxisRenderContext
    {
        public required DrawingContext DrawingContext { get; init; }

        public required AxisModel Model { get; init; }

        public required Rect Bounds { get; init; }

        public required ILabelFormatter LabelFormatter { get; init; }

        public required AxisLabelCache LabelCache { get; init; }

        public double TickMargin { get; init; }

        public double TrackHeight { get; init; }

        public int TrackCount { get; init; }
    }
}