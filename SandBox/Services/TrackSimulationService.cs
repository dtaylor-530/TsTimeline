using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace SandBox
{
    public class TrackSimulationService
    {
        public void Load(PlayerViewModel playerViewModel)
        {
            var maximum = 100;
            var rand = new Random();
            playerViewModel.PlayList = new PlayListViewModel
            {
                Name = "My PlayList",
                Tracks = []
            };
            foreach (var track in Enumerable.Range(0, 200))
            {
                var start = rand.Next(maximum);
                var end = start + rand.Next(maximum - start);
                playerViewModel.PlayList.Tracks.Add(new TrackViewModel()
                {
                    Name = $"Track {track}",
                    Clips =
                    [
                        new HoldClipViewModel()
                        {
                            StartValue = start,
                            EndValue = end,
                        },
                        new TriggerClipViewModel()
                        {
                            Value = rand.Next(maximum),
                        },
                        new TriggerClipViewModel()
                        {
                            Value = rand.Next(maximum),
                        }
                    ]
                });
            }
        }
    }
}
