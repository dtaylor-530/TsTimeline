using System;
using System.Collections.Generic;
using System.Text;

namespace SandBox
{
    public class VectorViewModel : Notification
    {
        private double value = 1;
        private double scale = 1;

        public double Value { get => value; set { this.value = value; OnPropertyChanged(); } }
        public double Scale { get => scale; set { this.scale = value; OnPropertyChanged(); } }
    }
}
