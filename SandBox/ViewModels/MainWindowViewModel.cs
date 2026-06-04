using System;
using System.Collections.ObjectModel;
using System.Linq;
using TsTimeline;

namespace SandBox
{
    public class MainWindowViewModel : Notification
    {
        private PlayerViewModel player;
        private Viewport viewport;
        private SpeedViewModel speed;
        private ChartViewModel chart;
        private ProgressViewModel progress;

        public Viewport Viewport
        {
            get => viewport; set
            {
                viewport = value;
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

        public ChartViewModel Chart
        {
            get => chart; set
            {
                chart = value;
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