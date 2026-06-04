using System;
using System.Collections.Generic;
using System.Text;

namespace SandBox
{
    public class ProgressViewModel : Notification
    {
        private double _progress;

        public double Progress
        {
            get => _progress;
            set
            {
                if (_progress != value)
                {
                    _progress = value;
                    OnPropertyChanged();
                }
            }
        }
    }
}
