using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace SandBox
{
    public class TrackService : Notification
    {
        private List<Action<(int, double)>> callbacks = new List<Action<(int, double)>>();
        private MediaService MediaService;
        private int index;
        private double progress;

        public TrackService()
        {
     
        }

        public void Load(MediaService MediaService)
        {
            this.MediaService = MediaService;
            MediaService.PropertyChanged += (s, e) =>
            {
                if(e.PropertyName == nameof(MediaService.CurrentIndex))
                {
                    Index = MediaService.CurrentIndex;
                }
            };

            TimeService.Instance.Subscribe(progress =>
            {
                int i = (int)Math.Floor(progress);
                Index = i;
                double t = MathHelpers.SmootherStep(progress - i);
                Progress = t;
                foreach (var callback in callbacks)
                {
                    callback((i, t));
                }
            });
        }

        public int Index
        {
            get => index;
            set
            {
                if (index != value)
                {
                    MediaService.CurrentIndex = index;
                    index = value;
                }
                OnPropertyChanged();
            }
        }

        public double Progress
        {
            get => progress;
            set
            {
                progress = value;
                OnPropertyChanged();
            }
        }


        public Action Subscribe(Action<(int, double)> callback)
        {
            callbacks.Add(callback);
            return new Action(() => callbacks.Remove(callback));
        }
    }
}
