using System.Diagnostics;
using System.Windows.Threading;

namespace SandBox
{
    public class TimeService : Notification
    {
        private double speed;

        private DispatcherTimer _timer;
        private const int MilliSecondInterval = 1;
        private Stopwatch stopwatch = new();
        private long lastElapsed = 0;
        private List<Action<double>> progressCallbacks = new();
        private double progress;
        private double rate;

        private TimeService()
        {
            this.Key = Keys.Time;
            _timer ??= new DispatcherTimer
            {
                Interval = TimeSpan.FromMilliseconds(MilliSecondInterval)
            };
            _timer.Tick += onTick;
            void onTick(object? sender, EventArgs e)
            {
                var (current, delta) = getElapsed();
                Progress += (speed * delta * rate);
                
    

                (long current, long delta) getElapsed()
                {
                    long current = stopwatch.ElapsedMilliseconds;
                    long delta = current - lastElapsed;
                    lastElapsed = current;
                    return (current, delta);
                }
            }
        }


        public double Progress
        {
            get => progress;
            set
            {
                if (progress == value)
                    return;
                progress = value;
                foreach (var callback in progressCallbacks)
                {
                    callback(progress);
                }
                OnPropertyChanged();
            }
        }

        public void Load(MediaService MediaService)
        {
            rate = MediaService.CurrentRate;
            MediaService.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == nameof(MediaService.CurrentIndex))
                {
                    if (this.Progress < MediaService.CurrentIndex)
                    {

                        Progress = MediaService.CurrentIndex;
                    }
                }
                if (e.PropertyName == nameof(MediaService.CurrentRate))
                {
                    this.rate = MediaService.CurrentRate;
                }

            };
        
            MediaService.PlayStateChanged += (s) =>
            {
                if (s == PlayState.Play)
                {
                    _timer?.Start();
                    stopwatch.Start();
                }
                else if (s == PlayState.Pause)
                {
                    _timer.Stop();
                    stopwatch.Stop();
                }
                else if (s == PlayState.Reset)
                {
                    _timer.Stop();
                    stopwatch.Stop();
                    reset();
                }
                else
                    throw new NotImplementedException();
            };
        }


        public virtual void Load(ViewModel viewModel)
        {
            if (viewModel.Key == Keys.Speed)
            {
                load(viewModel);
                return;
            }

            void load(ViewModel speed)
            {
                this.speed = speed.Value;
                speed.PropertyChanged += (s, e) =>
                {
                    if (e.PropertyName == nameof(ViewModel.Value))
                    {
                        this.speed = speed.Value;
                    }
                };
            }
        }

        void reset()
        {
            lastElapsed = 0;
            Progress = 0;
            foreach (var callback in progressCallbacks)
            {
                callback(progress);
            }
        }

        public static TimeService Instance { get; } = new TimeService();

        public Action Subscribe(Action<double> progressCallback)
        {
            progressCallbacks.Add(progressCallback);
            return () => progressCallbacks.Remove(progressCallback);
        }

        public void Unload()
        {
            progressCallbacks.Clear();
            reset();
            _timer?.Stop();
        }

    }
}
