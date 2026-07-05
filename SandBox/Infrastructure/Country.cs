namespace SandBox
{

    public class Country : Notification, ISize
    {
        private Rect position;
        private double uIWidth;
        private double uIHeight;
        private double uILeft;
        private double uITop;

        public string? ISO2 { get; set; } = string.Empty;

        public double? Width { get; set; }
        public double? Height { get; set; }
        public double? Left { get; set; }
        public double? Top { get; set; }

        public double? Skew { get; set; }
        public double? Rotate { get; set; }
        public double? Translate_X { get; set; }
        public double? Translate_Y { get; set; }

        public string? Data { get; set; } = string.Empty;

        [CsvHelper.Configuration.Attributes.Ignore]
        public Rect Position
        {
            get => position;
            set
            {
                position = value;
                OnPropertyChanged();
            }
        }

        [CsvHelper.Configuration.Attributes.Ignore]

        public double UIWidth
        {
            get => uIWidth;
            set
            {
                uIWidth = value;
                OnPropertyChanged();
            }
        }
        [CsvHelper.Configuration.Attributes.Ignore]

        public double UIHeight
        {
            get => uIHeight;
            set
            {
                uIHeight = value;
                OnPropertyChanged();
            }
        }
        [CsvHelper.Configuration.Attributes.Ignore]

        public double UILeft
        {
            get => uILeft;
            set
            {
                uILeft = value;
                OnPropertyChanged();
            }
        }
        [CsvHelper.Configuration.Attributes.Ignore]

        public double UITop
        {
            get => uITop;
            set
            {
                uITop = value;
                OnPropertyChanged();
            }
        }
    }
}
