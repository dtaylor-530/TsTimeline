using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using Renderers;
using TsTimeline;

namespace SandBox
{
    public class ConfigurationViewModel : Notification
    {
        private ChartType chartType;
        private Direction timelineDirection;
        private Direction combinedTimelineDirection;
        private IAxisLayer xAxisRenderer;
        private IAxisFactory xAxisFactory;
        private IAxisLayer yAxisRenderer;
        private IAxisFactory yAxisFactory;

        public ChartType ChartType 
        { 
            get => chartType; 
            set 
            { 
                chartType = value;
                OnPropertyChanged();
            } 
        }
        public Direction TimelineDirection 
        { 
            get => timelineDirection; 
            set 
            {
                timelineDirection = value;
                OnPropertyChanged();
            } 
        }
        public Direction CombinedTimelineDirection
        { 
            get => combinedTimelineDirection; 
            set 
            {
                combinedTimelineDirection = value;
                OnPropertyChanged();
            } 
        }

        public IAxisLayer XAxisRenderer
        {
            get => xAxisRenderer;
            set
            {
                xAxisRenderer = value;
                OnPropertyChanged();
            }
        }

        public IAxisFactory XAxisFactory
        {
            get => xAxisFactory;
            set
            {
                xAxisFactory = value;
                OnPropertyChanged();
            }
        }

        public IAxisLayer YAxisRenderer
        {
            get => yAxisRenderer;
            set
            {
                yAxisRenderer = value;
                OnPropertyChanged();
            }
        }

        public IAxisFactory YAxisFactory
        {
            get => yAxisFactory;
            set
            {
                yAxisFactory = value;
                OnPropertyChanged();
            }
        }
    }
}
