namespace SandBox
{

    public class ChartFactory : BaseFactory
    {
        private ViewModel master;
        private ViewModel slaves;

        public const double PointHeight = 4;
        public const double PointWidth = 4;

        public ChartFactory()
        {
        }

        public void Load(ViewModel master, ViewModel slaves)
        {
            this.master = master;
            this.slaves = slaves;
            this.PropertyChanged += (s, e) => handler(master, slaves);

            var rand = new Random();
            var xColors = OklchPalette.Generate(100);

            var colors = xColors.GetEnumerator();

            foreach (var i in Enumerable.Range(0, Count))
            {
                var track = new ViewModel()
                {
                    Name = $"Track {i}",
                    Key = Keys.TrackClip,
                    Order = i,
                    //Height = yLayout.CellLength,
                    Children = andLines(children(5, colors.MoveNext() ? ToMediaColor(colors.Current) : Colors.Black), ToMediaColor(colors.Current.WithOpacity(0.5))),
                };

                slaves.Add(track);
            }

            IEnumerable<ViewModel> children(int count, Color color)
            {
                for (int i = 0; i < count; i++)
                {
                    var x = rand.Next((int)App.viewportX.Start, (int)App.viewportX.End);
                    var y = rand.Next((int)App.viewportY.Start, (int)App.viewportY.End);

                    yield return
                        new ViewModel()
                        {
                            Key = Keys.Point,
                            Background = new SolidColorBrush(color),
                            X = x,
                            Y = y,
                            Height = PointHeight,
                            Width = PointWidth,
                        };
                }
            }

            IEnumerable<ViewModel> andLines(IEnumerable<ViewModel> points, Color color)
            {
                var pointEnumerator = points.OrderBy(a => a.X).GetEnumerator();
                pointEnumerator.MoveNext();
                var last = pointEnumerator.Current;
                yield return last;
                while (pointEnumerator.MoveNext())
                {
                    var point = pointEnumerator.Current;

                    yield return new ViewModel()
                    {
                        Key = Keys.ChartLine,
                        X = last.X ,
                        Y = last.Y ,
                        Height = - point.Y + last.Y,
                        Width = point.X - last.X,
                        Background = new SolidColorBrush(color),
                    };

                    yield return point;

                    last = point;
                }
            }


            foreach (var stack in slaves
                .Children.OfType<Notification>().Where(a => a.Key == Keys.TrackClip)
                .SelectMany(t => t.Children.OfType<ViewModel>())
                .Select(viewModel =>
                viewModel.Key == Keys.Point ?
                new ViewModel
                {
                    Key = Keys.Point,
                    X = viewModel.X,
                    Y = viewModel.Y,
                    Height = 4,
                    Width = 4,
                    Background = viewModel.Background
                } :
                new ViewModel
                {
                    Key = Keys.ChartLine,
                    X = viewModel.X,
                    Y = viewModel.Y,
                    Height = viewModel.Height,
                    Width = viewModel.Width,
                    Background = viewModel.Background
                }
                ))
            {
                master.Add(stack);
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

        public static Color ToMediaColor(System.Drawing.Color color)
        {
            return Color.FromArgb(color.A, color.R, color.G, color.B);
        }
    }
}
