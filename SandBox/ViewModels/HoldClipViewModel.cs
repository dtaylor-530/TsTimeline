using TsTimeline;

namespace SandBox
{
    public class HoldClipViewModel : Notification
    {
        private double startValue;
        private double endValue;

        public double StartValue
        {
            get => startValue;
            set
            {
                startValue = value;
                OnPropertyChanged();
            }
        }
        public double EndValue
        {
            get => endValue;
            set
            {
                endValue = value;
                OnPropertyChanged();
            }
        }
    }
}