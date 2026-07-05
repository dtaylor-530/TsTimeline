namespace Renderers
{
    public class AxisRenderContext
    {
        public required DrawingContext DrawingContext { get; init; }

        public IEnumerable<AxisTick> Ticks { get; set; }

        public required Rect Bounds { get; init; }

        public required AxisLabelCache LabelCache { get; init; }

    }
}