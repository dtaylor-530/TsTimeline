using System;
using System.Diagnostics.Metrics;

namespace SandBox
{

    public class PlayerViewModel : Notification
    {

        private int _currentIndex = 0;
        private double _progress;
        private bool _isPlaying;
        private PlayListViewModel master, slaves;

        public PlayerViewModel()
        {
        }
        public PlayListViewModel Master { get => master; set { master = value; OnPropertyChanged(); } }

        public PlayListViewModel Slaves { get => slaves; set { slaves = value; OnPropertyChanged(); } }


        public event Action<PlayState> PlayStateChanged;

        public TrackViewModel CurrentTrack => Master.Tracks.Count > 0 ? (TrackViewModel)Master.Tracks[CurrentIndex] : null;

        public int CurrentIndex
        {
            get => _currentIndex;
            set
            {
                if (_currentIndex != value)
                {
                    _currentIndex = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(CurrentTrack));
                }
            }
        }


        public bool IsPlaying
        {
            get => _isPlaying;
            private set
            {
                if (_isPlaying != value)
                {
                    _isPlaying = value;
                    OnPropertyChanged();
                }
            }
        }
        public void PlayPause()
        {
            if (IsPlaying)
            {
                //_timer.Stop();
                PlayStateChanged.Invoke(PlayState.Pause);
                IsPlaying = false;
            }
            else
            {
                //_timer.Start();
                PlayStateChanged.Invoke(PlayState.Play);
                IsPlaying = true;
            }
        }

        public void Next()
        {
            CurrentIndex = (CurrentIndex + 1) % Master.Tracks.Count;
            ResetProgress();
        }

        public void Previous()
        {
            CurrentIndex--;

            if (CurrentIndex < 0)
                CurrentIndex = Master.Tracks.Count - 1;

            ResetProgress();
        }


        private void ResetProgress()
        {            
            PlayStateChanged.Invoke(PlayState.Reset);
        }
    }
}
