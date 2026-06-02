using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace SandBox
{

    public class MainWindowVm : Notification
    {
        public ObservableCollection<TrackVm> Tracks { get; } = new ObservableCollection<TrackVm>();

        public MainWindowVm()
        {
            var maxinum = 1000;
            var rand = new Random();
            foreach (var track in Enumerable.Range(0, 100))
            {
                var start = rand.Next(maxinum);
                var end = start + rand.Next(maxinum - start);
                Tracks.Add(new TrackVm()
                {
                    Clips =
                    {
                        new HoldClipVm()
                        {
                            StartValue = start,
                            EndValue = end,
                        },
                        new TriggerClipVm()
                        {
                            Value = rand.Next(maxinum),
                        },
                        new TriggerClipVm()
                        {
                            Value = rand.Next(maxinum),
                        }
                    }
                });
            }
        }
    }
}