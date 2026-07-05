namespace SandBox
{
    public partial class App
    {
        static XBottomGridLayer xBottomGridLayer = new() { TickMargin = 0 };
        static XBottomTickLayer xBottomTickLayer = new();
        static XTopLabelLayer xTopLabelLayer = new();
        static XBottomLabelLayer xBottomLabelLayer = new();
        static YGridLayer yGridLayer = new() { TickMargin = 0 };
        static YTickLayer yTickLayer = new();
        static YLabelLayer yLabelLayer = new();
        static YLabelLayer y2LabelLayer = new();
        static YBackgroundLayer yBackgroundLayer = new(new SolidColorBrush(Colors.GhostWhite) { Opacity = 0.2 }, new SolidColorBrush(Colors.Gainsboro) { Opacity = 0.2 });
        static OceanBackgroundLayer xMapBackgroundLayer = new();
        List<TreeViewItem> list = new(), list2 = new();

        public static Viewport viewportX { get; set; }
        public static Viewport viewportY { get; set; }
        public static Viewport viewportX2 { get; set; }
        public static Viewport viewportY2 { get; set; }

        private ViewModel playListViewModel;
        private ViewModel playList2ViewModel;
        private ViewModel chartTypeViewModel;
        private ViewModel viewmodel;
        private MediaService MediaService;
        private ViewModel speed;
        private ViewModel progressX;
        private ViewModel progressY;
        private ViewModel progressX2;
        private ViewModel progressY2;
        private ViewModel x1Combination;
        private ViewModel y1Combination;
        private ViewModel x2Combination;
        private ViewModel y2Combination;
        private NameLabelFormatter nameLabelFormatter = new();
        private NumericLabelFormatter numericLabelFormatter = new();
        private TimeLabelFormatter dateTimeLabelFormatter = new();

        void initialise()
        {
            x1Combination = new ViewModel { Key = Keys.Renderer, Axis = Axis.X, Name = "X1", Children = [xBottomGridLayer, xBottomTickLayer, xBottomLabelLayer] };
            y1Combination = new ViewModel { Key = Keys.Renderer, Axis = Axis.Y, Name = "Y1", Children = [yGridLayer, yTickLayer, yLabelLayer] };
            x2Combination = new ViewModel { Key = Keys.Renderer, Axis = Axis.X, Name = "X2", Children = [] };
            y2Combination = new ViewModel { Key = Keys.Renderer, Axis = Axis.Y, Name = "Y2", Children = [yBackgroundLayer, yGridLayer, yTickLayer, y2LabelLayer] };

            speed = new ViewModel() { Key = Keys.Speed };

            progressX = new ViewModel()
            {
                Key = Keys.Progress,
                Name = "X",
                Axis = Axis.X,
                Direction = Direction.Right,
                Children = [
                    new ViewModel()
                    {
                        Name = "One",

                        Key= Keys.Area,
                        Background = Brushes.DarkCyan,
                        Opacity = 0.5,
                    },
                    new ViewModel()
                    {
                        Name = "Two",
                        Key= Keys.Area,
                        Background = Brushes.DarkCyan,
                        Opacity = 0.5,
                    },
                    new ViewModel()
                    {
                        Key= Keys.Line,
                        Width = 1,
                        Background = Brushes.DarkCyan,

                    },
                    new ViewModel()
                    {
                        Key= Keys.Thumb,
                        Direction = Direction.Up,
                        HorizontalAlignment = HorizontalAlignment.Left,
                        VerticalAlignment = VerticalAlignment.Bottom,
                        Width = 10,
                        Height= 15,
                        Margin = new Thickness(-5, 0, 0, 0)
                    },
                    ]
            };
            progressY = new ViewModel()
            {
                Key = Keys.Progress,
                Name = "Y",
                Axis = Axis.Y,
                Direction = Direction.Up,
                Children = [
                    new ViewModel()
                    {
                        Name = "One",
                            Axis = Axis.Y,
                        Key= Keys.Area,
                        Background = Brushes.Violet,
                        Opacity = 0.5,
                    },
                    new ViewModel()
                    {
                        Name = "Two",
                            Axis = Axis.Y,
                        Key= Keys.Area,
                        Background = Brushes.Violet,
                        Opacity = 0.5,
                    },
                    new ViewModel()
                    {
                        Key= Keys.Line,
                            Axis = Axis.Y,
                        Height = 1,
                        Background = Brushes.Violet,
                    },
                    new ViewModel()
                    {
                        Key= Keys.Thumb,
                            Axis = Axis.Y,
                        Direction = Direction.Right,
                        HorizontalAlignment = HorizontalAlignment.Left,
                        VerticalAlignment = VerticalAlignment.Top,
                        Width = 10,
                        Height = 15,
                        Margin = new Thickness(0, -5, 0, 0)
                    },
                    ]
            };
            progressX2 = new ViewModel()
            {
                Key = Keys.Progress,
                Name = "X2",
                Axis = Axis.X,
                Direction = Direction.None,
                Children = [
                    new ViewModel()
                    {
                        Name = "One",
                        Order = 1,
                        Key= Keys.Area,
                        Background = Brushes.Plum,
                        Opacity = 0.5,

                    },
                    new ViewModel()
                    {
                        Name = "Two",
                        Order = 2,
                        Key= Keys.Area,
                        Background = Brushes.Plum,
                        Opacity = 0.5,

                    },
                    new ViewModel()
                    {
                        Key= Keys.Line,
                        Background = Brushes.Plum,
                        Width = 1
                    },
                    new ViewModel()
                    {
                        Key= Keys.Thumb,
                        Direction = Direction.Down,
                        HorizontalAlignment = HorizontalAlignment.Left,
                        VerticalAlignment = VerticalAlignment.Top,
                        Width = 10,
                        Height = 15,
                        Margin = new Thickness(-5, 0, 0, 0)
                    },
                    ]
            };
            progressY2 = new ViewModel()
            {
                Key = Keys.Progress,
                Direction = Direction.Down,
                Name = "Y2",
                Axis = Axis.Y,
                Children = [
                    new ViewModel()
                    {
                        Name = "One",
                        Axis = Axis.Y,
                        Order = 1,
                        Key= Keys.Area,
                        Background = Brushes.CadetBlue,
                        Opacity = 0.5,
                    },
                    new ViewModel()
                    {
                        Name = "Two",
                        Order = 2,
                            Axis = Axis.Y,
                        Key= Keys.Area,
                        Visibility = Visibility.Collapsed,
                        Background = Brushes.CadetBlue,
                        Opacity = 0.5,
                    },
                    new ViewModel()
                    {
                        Key= Keys.Line,
                        Background = Brushes.CadetBlue,
                            Axis = Axis.Y,
                        Height = 1
                    },
                    new ViewModel()
                    {
                        Key= Keys.Thumb,
                        Width = 10,
                            Axis = Axis.Y,
                        Direction = Direction.Right,
                        HorizontalAlignment = HorizontalAlignment.Left,
                         VerticalAlignment = VerticalAlignment.Top,
                               Margin = new Thickness(0, -5, 0, 0)
                    },
                ]
            };

            MediaService = new MediaService() { Key = Keys.Player };

            playListViewModel = new ViewModel
            {
                Name = "PlayList1",
                Order = 1,
                Key = Keys.Playlist,
                PanelType = PanelType.Canvas,
                Direction = Direction.Up,
                Children = [],
            };
            playList2ViewModel = new ViewModel
            {
                Name = "PlayList2",
                Order = 2,
                PanelType = PanelType.DirectionalStackPanel,
                Key = Keys.Playlist,
                Direction = Direction.Down,
                Children = []
            };
            chartTypeViewModel = new ViewModel { Key = Keys.ChartType };

            viewmodel = new ViewModel
            {
                Key = Keys.Master,
                Children = [
                new ViewModel
                    {
                        Key = Keys.Configuration,
                        GridLength = GridLength.Auto,
                        Direction = Direction.Right,
                        Children =
                        [
                            MediaService,
                            speed,
                            chartTypeViewModel,
                            TimeService.Instance,
                        ]
                    },
                    new ViewModel
                    {
                        Key = Keys.Charts,
                        GridLength = new GridLength(1, GridUnitType.Star),
                        Children = [
                            new ViewModel
                            {
                                Name = "Chart 1",
                                Group = Groups.One,
                                Key = Keys.Chart,
                                GridLength = new GridLength(1, GridUnitType.Star),
                                Children =
                                [
                                    new ViewModel
                                    {
                                        Key = Keys.ChartObjects,
                                        Children = [

                                            x1Combination,
                                            y1Combination,
                                            playListViewModel,
                                            progressX,
                                            progressY,
                                        ]
                                    },
                                    new ViewModel
                                    {
                                        Key = Keys.Viewports,
                                        HorizontalAlignment = HorizontalAlignment.Right,
                                        Margin = new Thickness(0,0,10,0),
                                        Children = [
                                            viewportX,
                                            viewportY,
                                        ]
                                    },
                                ]
                            },
                            new ViewModel
                            {
                                Name = "Chart 2",
                                Group = Groups.Two,
                                Key = Keys.Chart,
                                GridLength = new GridLength(1, GridUnitType.Star),
                                Children =
                                [

                                    new ViewModel
                                    {
                                        Key = Keys.ChartObjects,
                                        Children = [
                                            x2Combination,
                                            y2Combination,
                                            playList2ViewModel,
                                            progressX2,
                                            progressY2]
                                    },
                                    new ViewModel
                                    {
                                        Key = Keys.Viewports,
                                        HorizontalAlignment = HorizontalAlignment.Right,
                                        Margin = new Thickness(0,0,10,0),
                                        Children =
                                        [
                                            viewportX2,
                                            viewportY2,
                                        ]
                                    },
                                ]
                            }
                            ]
                    }]
            };
        }
    }


}
