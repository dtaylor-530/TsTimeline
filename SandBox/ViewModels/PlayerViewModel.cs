using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace SandBox
{
    public class PlayListViewModel 
    {
        public string Name { get; set; } 

        public ObservableCollection<TrackViewModel> Tracks { get; set; } 
        public ObservableCollection<TrackViewModel> Stacks { get; set; } 
    }


    public class PlayerViewModel : Notification
    {

        private int _currentIndex = 0;
        private double _progress;
        private bool _isPlaying;
        private PlayListViewModel playList;

        public PlayerViewModel()
        {
        }
        public PlayListViewModel PlayList { get => playList; set { playList = value; OnPropertyChanged(); } }


        public event Action<PlayState> PlayStateChanged;

        public TrackViewModel CurrentTrack => PlayList.Tracks.Count > 0 ? PlayList.Tracks[CurrentIndex] : null;

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
            CurrentIndex = (CurrentIndex + 1) % PlayList.Tracks.Count;
            ResetProgress();
        }

        public void Previous()
        {
            CurrentIndex--;

            if (CurrentIndex < 0)
                CurrentIndex = PlayList.Tracks.Count - 1;

            ResetProgress();
        }


        private void ResetProgress()
        {            
            PlayStateChanged.Invoke(PlayState.Reset);
        }
    }
}
