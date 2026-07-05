namespace SandBox
{
    public class MediaService : ViewModel
    {
        private int _currentIndex = 0;
        private bool _isPlaying;
        private double rate = 1 / 5000.0;

        public MediaService()
        {
        }

        public event Action<PlayState> PlayStateChanged;

        public int CurrentIndex
        {
            get => _currentIndex;
            set
            {
                if (_currentIndex != value)
                {
                    _currentIndex = value;
                    OnPropertyChanged();
                }
            }
        }

        public double CurrentRate
        {
            get => rate;
            set
            {
                if (rate != value)
                {
                    rate = value;
                    OnPropertyChanged();
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
            CurrentIndex++;
            //ResetProgress();
        }

        public void Previous()
        {
            CurrentIndex--;

            //if (CurrentIndex < 0)
            //    CurrentIndex = Playlist.Tracks.Count - 1;

            //ResetProgress();
        }


        private void ResetProgress()
        {
            PlayStateChanged.Invoke(PlayState.Reset);
        }
    }
}
