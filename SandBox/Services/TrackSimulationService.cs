using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;

namespace SandBox
{
    public class TrackSimulationService
    {
        public void Load(PlayListViewModel master, PlayListViewModel slaves)
        {
            var maximum = 100;
            var rand = new Random();
            System.Timers.Timer _refreshTimer = new System.Timers.Timer(300) { AutoReset = false};
            _refreshTimer.Elapsed += (s, e) => Application.Current.Dispatcher.BeginInvoke(()=> refreshStacks());

            foreach (var i in Enumerable.Range(0, 200))
            {
                var start = rand.Next(maximum);
                var end = start + rand.Next(maximum - start);
                var holdClip = new HoldClipViewModel()
                {
                    X = start,
                    Width = end - start,
                };
                var track = new TrackViewModel()
                {
                    Name = $"Track {i}",
                    Order = i,
                    Clips =
                    [
                        holdClip,
                        new TriggerClipViewModel()
                        {
                            Value = rand.Next(maximum),
                        },
                        new TriggerClipViewModel()
                        {
                            Value = rand.Next(maximum),
                        }
                    ]
                };
                Setup(holdClip);
                slaves.Tracks.Add(track);
            }
            refreshStacks();

            void refreshStacks()
            {
                master.Tracks.Clear();
                foreach (var item in toStacks())
                {
                    master.Tracks.Add(item);
                }
            }
            void Setup(INotifyPropertyChanged holdClip)
            {    
                holdClip.PropertyChanged += (s, e) =>
                {
                    _refreshTimer.Stop();
                    _refreshTimer.Start();
                };
            }

            IEnumerable<TrackViewModel> toStacks()
            {
                return toRanges(
                    slaves.Tracks.OfType<TrackViewModel>(),
                    a => a.Clips.OfType<HoldClipViewModel>()
                    .Select(a => ((int)a.X, (int)(a.X + a.Width))))
                    .Select(kvp => new TrackViewModel()
                    {
                        Order = kvp.Key,
                        Clips = new(
                            groupContiguousNumbers([.. kvp.Value])
                            .Select(group => new HoldClipViewModel()
                            {
                                X = group.First(),
                                Width = group.Last() - group.First(),
                            })
                            .Cast<Notification>())
                    })
                    .OrderBy(a => a.Order);
            }

            static List<List<int>> groupContiguousNumbers(params int[] numbers)
            {
                var result = new List<List<int>>();
                var currentGroup = new List<int>();

                foreach (var number in numbers.Order())
                {
                    if (currentGroup.Count == 0 || number == currentGroup.Last() + 1)
                    {
                        currentGroup.Add(number);
                    }
                    else
                    {
                        result.Add(currentGroup);
                        currentGroup = new List<int> { number };
                    }
                }

                if (currentGroup.Count > 0)
                {
                    result.Add(currentGroup);
                }
                return result;
            }

            Dictionary<int, List<int>> toRanges<T>(IEnumerable<T> items, Func<T, IEnumerable<(int, int)>> minMax)
            {
                return items
                    .Aggregate(new Dictionary<int, int>(),
                    (acc, track) =>
                    {
                        foreach (var (min, max) in minMax(track))
                        {

                            for (var i = min; i <= max; i++)
                            {
                                if (!acc.TryGetValue(i, out int value))
                                {
                                    acc[i] = value = 0;
                                }
                                acc[i] += 1;
                            }

                        }
                        return acc;
                    })
                    .Aggregate(new Dictionary<int, List<int>>(),
                    (min_max, kvp) =>
                    {
                        for (int i = 0; i < kvp.Value + 1; i++)
                        {
                            if (!min_max.TryGetValue(i, out var current))
                            {
                                min_max[i] = [kvp.Key];
                            }
                            else
                            {
                                min_max[i].Add(kvp.Key);
                            }
                        }
                        return min_max;
                    });
            }
        }
    }
}
