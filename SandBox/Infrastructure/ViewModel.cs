namespace SandBox
{
    public partial class ViewModel : Notification
    {
        private double x, y, value = 1, width;
        private Direction direction = Direction.Down;
        private PanelType panelType;
        private double height;
        private Enum @enum;
        private Visibility visibility = Visibility.Visible;
        private double opacity = 1;
        private Rect position;
        private Brush background;
        private double worldX;
        private double worldY;
        private double worldWidth;
        private double worldHeight;

        public Direction Direction
        {
            get => direction;
            set
            {
                direction = value;
                OnPropertyChanged();
            }
        }
        public double Opacity
        {
            get => opacity;
            set
            {
                opacity = value;
                OnPropertyChanged();
            }
        }

        public Brush Background
        {
            get => background;
            set
            {
                background = value;
                OnPropertyChanged();
            }
        }

        public PanelType PanelType
        {
            get => panelType;
            set
            {
                panelType = value;
                OnPropertyChanged();
            }
        }

        public Rect Position
        {
            get => position;
            set
            {
                position = value;
                OnPropertyChanged();
            }
        }

        public int Order
        {
            get; set;
        }

        public GridLength GridLength
        {
            get; set;
        }
        public Thickness Margin
        {
            get; set;
        }


        public Axis Axis { get; set; }


        public double X
        {
            get => x;
            set
            {
                if (x != value)
                {
                    x = value;
                    OnPropertyChanged();
                }
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

        public double Width
        {
            get => width;
            set
            {
                if (value == width)
                    return;
                width = value;
                OnPropertyChanged();
            }
        }

        public double Height
        {
            get => height;
            set
            {
                if (value == height)
                    return;
                height = value;
                OnPropertyChanged();
            }
        }


        public double WorldX
        {
            get => worldX;
            set
            {
                if (worldX != value)
                {
                    worldX = value;
                    OnPropertyChanged();
                }
            }
        }

        public double WorldY
        {
            get => worldY;
            set
            {
                worldY = value;
                OnPropertyChanged();
            }
        }

        public double WorldWidth
        {
            get => worldWidth;
            set
            {
                if (value == worldWidth)
                    return;
                worldWidth = value;
                OnPropertyChanged();
            }
        }

        public double WorldHeight
        {
            get => worldHeight;
            set
            {
                if (value == worldHeight)
                    return;
                worldHeight = value;
                OnPropertyChanged();
            }
        }

        public double Value
        {
            get => value;
            set
            {
                if (this.value != value)
                {
                    this.value = value;
                    OnPropertyChanged();
                }
            }
        }


        public Enum Enum
        {
            get => @enum;
            set
            {
                if (this.@enum != value)
                {
                    this.@enum = value;
                    OnPropertyChanged();
                }
            }
        }

        public Visibility Visibility
        {
            get => visibility;
            set
            {
                if (this.visibility != value)
                {
                    this.visibility = value;
                    OnPropertyChanged();
                }
            }
        }

        public HorizontalAlignment HorizontalAlignment { get; set; }
        public VerticalAlignment VerticalAlignment { get; set; }

        public override string ToString()
        {
            return Key + " " + Name;
        }
    }
}
