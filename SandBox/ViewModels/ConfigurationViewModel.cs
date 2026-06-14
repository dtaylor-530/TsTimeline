using System;
using System.Collections.Generic;
using System.Text;
using TsTimeline;

namespace SandBox
{
    public class ConfigurationViewModel : Notification
    {
        private ChartType chartType;

        public ChartType ChartType 
        { 
            get => chartType; 
            set 
            { 
                chartType = value;
                OnPropertyChanged();
            } 
        }
    }
}
