namespace SandBox
{
    public class Factory : Notification
    {
        private int count = 10;

        public int Count
        {
            get => count;
            set
            {
                count = value;
                OnPropertyChanged();
            }
        }
    }
}
