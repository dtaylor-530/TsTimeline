namespace Renderers
{
    public class AxisTick(    
        object value,
        double screenPosition,
        TickLevel level) : Notification
    { 
        public object Value { get; } = value;

        public double ScreenPosition { get; } = screenPosition;

        public TickLevel Level { get; } = level;

        public bool HasLabel => Level == TickLevel.Major;
    }
}
