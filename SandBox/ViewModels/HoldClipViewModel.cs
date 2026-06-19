using TsTimeline;

namespace SandBox
{
    public class HoldClipViewModel : Notification
    {
        private double startValue;
        private double endValue;

        public double X
        {
            get => startValue;
            set
            {
                if (value == startValue)
                    return;
                startValue = value;
                OnPropertyChanged();
            }
        }
        public double Width
        {
            get => endValue;
            set
            {
                if (value == endValue)
                    return;
                endValue = value;
                OnPropertyChanged();
            }
        }
    }
}