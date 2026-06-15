using Renderers;

namespace SandBox
{
    public class MainWindowViewModel : Notification
    {
        private PlayerViewModel player;
        private Viewport viewportX;
        private Viewport viewportY;
        private Viewport viewportItemY;
        private SpeedViewModel speed;
        private ProgressViewModel progress;
        private ConfigurationViewModel configuration;

        public Viewport ViewportX
        {
            get => viewportX; set
            {
                viewportX = value;
                OnPropertyChanged();
            }
        }

        public Viewport ViewportY
        {
            get => viewportY; set
            {
                viewportY = value;
                OnPropertyChanged();
            }
        }
        public Viewport ViewportItemY
        {
            get => viewportItemY; set
            {
                viewportItemY = value;
                OnPropertyChanged();
            }
        }

        public ConfigurationViewModel Configuration
        {
            get => configuration; set
            {
                configuration = value;
                OnPropertyChanged();
            }
        }

        public SpeedViewModel Speed
        {
            get => speed; set
            {
                speed = value;
                OnPropertyChanged();
            }
        }

        public PlayerViewModel Player
        {
            get => player; set
            {
                player = value;
                OnPropertyChanged();
            }
        }

        public ProgressViewModel Progress
        {
            get => progress; set
            {
                progress = value;
                OnPropertyChanged();
            }
        }
    }
}