using System;
using System.Diagnostics;
using System.Windows.Threading;
using TsTimeline;

namespace SandBox
{
    public class TimeSimulationService : Notification
    {
        private double progress;
        private const int MilliSecondInterval = 10;
        public TimeSimulationService()
        {


        }

        public void Load(PlayerViewModel playerViewModel, ProgressViewModel progressViewModel, Viewport viewport)
        {
            var _timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromMilliseconds(MilliSecondInterval)
            };
            var stopwatch = new Stopwatch();
            long lastElapsed = 0;

            playerViewModel.PlayStateChanged += (s) =>
            {
                if (s == PlayState.Play)
                {
                    _timer.Start();
                    stopwatch.Start();
                }
                else if (s == PlayState.Pause)
                {
                    _timer.Stop();
                    stopwatch.Stop();
                }
                else if (s == PlayState.Reset)
                {
                    progressViewModel.Progress = 0;

                }
                else
                    throw new NotImplementedException();
            };

            _timer.Tick += OnTick;

            void OnTick(object? sender, EventArgs e)
            {
                long current = stopwatch.ElapsedMilliseconds;
                long delta = current - lastElapsed;
                lastElapsed = current;
                progressViewModel.Progress += (progress * delta / 1000.0); 

                //if (playerViewModel.Progress >= 100)
                //{
                //    playerViewModel.Next();
                //}
            }
        }

        internal void Load(SpeedViewModel speed)
        {
            progress = speed.Value;
            speed.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == nameof(SpeedViewModel.Value))
                {
                    progress = speed.Value;
                }
            };
        }
    }
}
