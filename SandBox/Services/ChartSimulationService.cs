using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using TsTimeline;

namespace SandBox
{
    public class ChartSimulationService
    {
        public void Load(PlayListViewModel master, PlayListViewModel slaves)
        {
            var maximum = 100;
            var rand = new Random();

            foreach (var i in Enumerable.Range(0, 200))
            {
                var start = rand.Next(maximum);
                var end = rand.Next(maximum);
                var track = new TrackViewModel()
                {
                    Name = $"Track {i}",
                    Order = i,
                    Clips =
                    [
                        new PointViewModel()
                        {
                            X = start,
                            Y = end,
                        },
                    ]
                };

                slaves.Tracks.Add(track);
            }

            foreach (var stack in slaves.Tracks.OfType<TrackViewModel>().SelectMany(t => t.Clips.OfType<PointViewModel>()))
                master.Tracks.Add(stack);
        }
    }
}
