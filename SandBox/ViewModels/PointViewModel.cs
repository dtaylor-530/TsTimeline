namespace SandBox
{
    internal class PointViewModel : Notification
    {
        private double x;
        private double y;

        public double X
        {
            get => x;
            set
            {
                x = value;
                OnPropertyChanged();
            }
        }

        public double Y
        {
            get => y;
            set
            {
                y = value;
                OnPropertyChanged();
            }
        }
    }
}
