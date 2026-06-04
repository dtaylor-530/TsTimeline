using System;
using System.Collections.Generic;
using System.Text;
using TsTimeline;

namespace SandBox
{
    public class ChartViewModel : Notification
    {
        private ChartSeries series;
        private Viewport viewport;

        public ChartSeries Series
        {
            get => series;
            set
            {
                series = value;
                OnPropertyChanged();
            }
        }

        public Viewport Viewport
        {
            get => viewport;
            set
            {
                viewport = value;
                OnPropertyChanged();
            }
        }
    }
}
