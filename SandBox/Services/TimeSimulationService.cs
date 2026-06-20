using System;
using System.Diagnostics;
using System.Windows.Threading;

namespace SandBox
{
    public interface IProgressService
    {
        double Progress(ref double progress, long delta, double rate);
    }

    public class ProgressService : Notification, IProgressService
    {
        public double Progress(ref double progress, long delta, double rate)
        {
            progress += (rate * delta / 1000.0);
            return progress;
        }
    }

    public class StaggeredProgressService : Notification, IProgressService
    {
        public double Progress(ref double progress, long delta, double rate)
        {
            progress += rate * delta / 1000.0;
            return Math.Round(progress) * 15;
        }
    }

    public class TimeSimulationService : Notification
    {
        private double rate;
        private double progress;
        private DispatcherTimer _timer;
        private const int MilliSecondInterval = 10;

        public TimeSimulationService()
        {
        }

        public void Load(PlayerViewModel playerViewModel, ProgressViewModel progressViewModel, IProgressService progressService)
        {
            _timer = new DispatcherTimer
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
                    progressViewModel.Progress = progress = 0;

                }
                else
                    throw new NotImplementedException();
            };

            _timer.Tick += onTick;

            void onTick(object? sender, EventArgs e)
            {
                long current = stopwatch.ElapsedMilliseconds;
                long delta = current - lastElapsed;
                lastElapsed = current;
                progressViewModel.Progress = progressService.Progress(ref progress, delta, rate);
            }
        }

        public void Unload()
        {
            _timer?.Stop();
        }

        internal void Load(SpeedViewModel speed)
        {
            rate = speed.Value;
            speed.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == nameof(SpeedViewModel.Value))
                {
                    rate = speed.Value;
                }
            };
        }
    }
}
