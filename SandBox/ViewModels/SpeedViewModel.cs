using System;
using System.Collections.Generic;
using System.Text;

namespace SandBox
{
    public class SpeedViewModel : Notification
    {
        private double value = 1;

        public double Value { get => value; set { this.value = value; OnPropertyChanged(); } }
    }
}
