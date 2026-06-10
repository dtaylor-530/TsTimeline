namespace TsTimeline
{
    public readonly struct AxisTick(
        double value,
        double screenPosition,
        TickLevel level)
    {
        public double Value { get; } = value;
        public double ScreenPosition { get; } = screenPosition;

        public TickLevel Level { get; } = level;

        public bool HasLabel => Level == TickLevel.Major;
    }
}
