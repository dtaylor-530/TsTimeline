using System.Windows.Input;

namespace Demos
{
    public class TrackFactory : BaseFactory
    {
        private ViewModel master;
        private ViewModel slaves;

        public TrackFactory()
        {
            this.PropertyChanged += (s, e) => handler(master, slaves);
        }

        public void Load(ViewModel master, ViewModel slaves)
        {
            this.master = master;
            this.slaves = slaves;
            this.PropertyChanged += (s, e) => handler(master, slaves);

            var maximum = (int)App.viewportX.End;
            var rand = new Random();
            System.Timers.Timer _refreshTimer = new System.Timers.Timer(300) { AutoReset = false };
            _refreshTimer.Elapsed += (s, e) => Application.Current.Dispatcher.BeginInvoke(() => refreshStacks());

            foreach (var i in Enumerable.Range(0, Count))
            {
                var start = rand.Next(maximum);
                var end = start + rand.Next(maximum - start);
                var holdClip = new ViewModel()
                {
                    Key = Keys.HoldClip,
                    X = start,
                    Width = end - start + 5,
                    Children = [
                        new ViewModel()
                        {
                            Key = Keys.HoldclipThumb,
                            Name = Names.Left,
                            Opacity = 0.55,
                            Column = 0,
                            CursorType = CursorType.SizeWE,
                            Width = 5,
                        },
                        new ViewModel()
                        {
                            Key = Keys.HoldclipThumb,
                            Name = Names.Center,
                            Opacity = 0.25,
                            Width = end - start,
                            Column = 1,
                            CursorType = CursorType.Hand,
                        },
                        new ViewModel()
                        {
                            Key = Keys.HoldclipThumb,
                            Name = Names.Right,
                            Opacity = 0.55,
                            Column = 2,
                            CursorType = CursorType.SizeWE,
                            Width = 5,
                        },

                        ]
                };
                holdClip.PropertyChanged += (s, e) =>
                {
                    _refreshTimer.Stop();
                    _refreshTimer.Start();
                };
                var track = new ViewModel()
                {
                    Key = Keys.TrackClip,
                    Name = "A",
                    Order = i,
                    Children =
                    [
                        holdClip,
                        new ViewModel()
                        {
                            Key =  Keys.TriggerClip,
                            X = rand.Next(maximum),
                        },
                        new ViewModel()
                        {
                            Key =  Keys.TriggerClip,
                            X = rand.Next(maximum),
                        }
                    ]
                };
                slaves.Add(track);
            }
            refreshStacks();

            void refreshStacks()
            {
                master.Clear();
                foreach (var item in toStacks())
                {
                    master.Add(item);
                }
                //master.Remove(master.Children.Last());
            }


            IEnumerable<ViewModel> toStacks()
            {
                return toRanges(
                    slaves.Children.OfType<Notification>().Where(a => a.Key == Keys.TrackClip),
                    a => a.Children.OfType<Notification>().Where(a => a.Key == Keys.HoldClip)
                    .OfType<ViewModel>()
                    .Select(a => ((int)a.X, (int)(a.X + a.Width))))
                    .Select(kvp => new ViewModel()
                    {
                        Key = Keys.TrackClip,
                        Order = kvp.Key,
                        Children = [.. groupContiguousNumbers([.. kvp.Value])
                            .Select(group => new ViewModel()
                            {
                                Key = Keys.BandClip,
                                X = group.First(),
                                Width = group.Last() - group.First(),
                            })
                            .Cast<Notification>()]
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

        bool flag = false;
        void handler(ViewModel master, ViewModel slaves)
        {
            flag = true;
            Application.Current.Dispatcher.BeginInvoke(() =>
            {
                if (flag == false)
                    return;
                flag = false;
                if (master != null && slaves != null)
                {
                    master.Clear();
                    slaves.Clear();
                    Load(master, slaves);
                }
            }, System.Windows.Threading.DispatcherPriority.ApplicationIdle);
        }

        public void Unload()
        {
            this.PropertyChanged -= (s, e) => handler(master, slaves);

        }

    }
}
