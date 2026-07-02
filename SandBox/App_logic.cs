using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;

namespace SandBox
{
    public partial class App
    {
        TrackFactory trackSimulationService = new();
        MapFactory mapFactory = new();

        MapTraverseService mapSimulationService = new();
        ChartFactory chartSimulationService = new();
        GridHighlightService gridSimulationService = new();
        TrackService trackService = new();

        void reloadData()
        {
            playListViewModel.Clear();
            playList2ViewModel.Clear();
            TimeService.Instance.Unload();
            
            chartSimulationService.Unload();
            trackSimulationService.Unload();
            y2Combination.Remove(yBackgroundLayer);
            styleSelector.ChartType = (ChartType)chartTypeViewModel.Enum;

            if (chartTypeViewModel.Enum is ChartType.Points)
            {
                TimeService.Instance.Subscribe(p => progressX.X = p);
                TimeService.Instance.Subscribe(p => progressX2.X = p);
                chartSimulationService.Load(playListViewModel, playList2ViewModel);
                viewmodel.FindChild<ViewModel>(Keys.Configuration).Remove(trackSimulationService);
                viewmodel.FindChild<ViewModel>(Keys.Configuration).Add(chartSimulationService);
                viewportY.Zoom = 2;
                viewportX.Zoom = 10;
                viewportX2.Zoom = 10;
                progressY.Direction = Direction.None;
                progressY2.Direction = Direction.None;
                progressX.Direction = Direction.Right;
                progressX2.Direction = Direction.Right;
                playListViewModel.PanelType = PanelType.Canvas;
                x1Combination.Add(xBottomLabelLayer);
                y1Combination.Add(yLabelLayer);
                yLabelLayer.Formatter = numericLabelFormatter;
                y2LabelLayer.Formatter = nameLabelFormatter;
                xBottomLabelLayer.Formatter = numericLabelFormatter;
                viewportY2.MinimumSpacing = 26;

            }
            else if (chartTypeViewModel.Enum is ChartType.Bands)
            {
                TimeService.Instance.Subscribe(p => progressX.X = p);
                TimeService.Instance.Subscribe(p => progressX2.X = p);
                trackSimulationService.Load(playListViewModel, playList2ViewModel);
                viewmodel.FindChild<ViewModel>(Keys.Configuration).Remove(chartSimulationService);
                viewmodel.FindChild<ViewModel>(Keys.Configuration).Add(trackSimulationService);
                viewportY.Zoom = 1;
                viewportX.Zoom = 10;
                progressY.Direction = Direction.None;
                progressY2.Direction = Direction.None;
                progressX.Direction = Direction.Right;
                progressX2.Direction = Direction.Right;
                playListViewModel.PanelType = PanelType.DirectionalStackPanel;
                y1Combination.Add(yLabelLayer);
                y2Combination.Add(yBackgroundLayer);
                y2Combination.Remove(yGridLayer);
                y2Combination.Remove(yTickLayer);
                y2Combination.Remove(yLabelLayer);
                xBottomLabelLayer.Formatter = dateTimeLabelFormatter;
                y2LabelLayer.Formatter = nameLabelFormatter;
                viewportY.MinimumSpacing = 6;
                viewportY2.MinimumSpacing = 16;
            }
            else if (chartTypeViewModel.Enum is ChartType.Map)
            {
                playList2ViewModel.PanelType = PanelType.AutoGrid;
                playListViewModel.PanelType = PanelType.Map;

                viewmodel.FindChild<ViewModel>(Keys.Configuration).Remove(chartSimulationService);
                viewmodel.FindChild<ViewModel>(Keys.Configuration).Remove(trackSimulationService);

                mapSimulationService.Load(playListViewModel);
                mapSimulationService.Load(trackService);
                gridSimulationService.Load(playList2ViewModel);
                mapFactory.Load(playListViewModel, playList2ViewModel);
                viewportY.Zoom = 0.5;
                viewportX.Zoom = 0.5;
                progressX.Direction = Direction.Right;
                progressY.Direction = Direction.Up;
                progressY2.Direction = Direction.Down;
                progressX2.Direction = Direction.Right;
                x1Combination.Clear();
                y1Combination.Clear();
                x2Combination.Clear();
                y2Combination.Clear();
                x1Combination.Add(xMapBackgroundLayer);
                trackService.Load(MediaService);

                foreach (var child in progressX.FindChildren<ViewModel>(Keys.Area))
                {
                    child.Direction = Direction.None;
                    child.Visibility = Visibility.Collapsed;
                }
                foreach (var child in progressX.FindChildren<ViewModel>(Keys.Thumb))
                {
                    child.Direction = Direction.None;
                    child.Visibility = Visibility.Collapsed;
                }
                foreach (var child in progressY.FindChildren<ViewModel>(Keys.Area))
                {
                    child.Direction = Direction.None;
                    child.Visibility = Visibility.Collapsed;

                }
                foreach (var child in progressY.FindChildren<ViewModel>(Keys.Thumb))
                {
                    child.Direction = Direction.None;
                    child.Visibility = Visibility.Collapsed;
                }
                var lineChildX = progressX.FindChild<ViewModel>(Keys.Line);
                mapSimulationService.Load(lineChildX);

                var lineChildY = progressY.FindChild<ViewModel>(Keys.Line);
                mapSimulationService.Load(lineChildY);

                foreach (var child in progressX2.FindChildren<ViewModel>(Keys.Area))
                {
                    child.Opacity = 0.8;
                    gridSimulationService.Load(child);
                    child.Background = Brushes.Gray;
                }

                foreach (var child in progressY2.FindChildren<ViewModel>(Keys.Area))
                {
                    child.Opacity = 0.8;
                    gridSimulationService.Load(child);
                    child.Background = Brushes.Gray;
                }

                progressX2.FindChild<ViewModel>(Keys.Line).Direction = Direction.None;
                progressY2.FindChild<ViewModel>(Keys.Line).Direction = Direction.None;
                foreach (var child in progressX.FindChildren<ViewModel>(Keys.Area))
                    child.Direction = Direction.None;
            }
            else
            {
                throw new System.Exception(" ");
            }

            reRender();
        }

        void reRender()
        {
            foreach (var item in list)
            {
                if (item.DataContext is ViewModel { Key: Keys.Renderer })
                {
                    Dispatcher.BeginInvoke(
                        DispatcherPriority.ApplicationIdle,
                        new Action(() =>
                        {
                            item.InvalidateVisual();
                        }));
                }
            }
            foreach (var item in list2)
            {
                if (item.DataContext is ViewModel { Key: Keys.Renderer })
                {
                    Dispatcher.BeginInvoke(
                        DispatcherPriority.ApplicationIdle,
                        new Action(() =>
                        {
                            item.InvalidateVisual();
                        }));
                }
            }
        }

        void initialiseViewPorts()
        {
            viewportX = new Viewport() { Name = "X1", Key = Keys.Viewport, Axis = Axis.X };
            viewportX.PropertyChanged += (s, e) => propertyChanged(treeView, viewportX, e.PropertyName, list);
            viewportY = new Viewport() { Name = "Y1", Key = Keys.Viewport, Axis = Axis.Y };
            viewportY.PropertyChanged += (s, e) => propertyChanged(treeView, viewportY, e.PropertyName, list);
            viewportX2 = new Viewport() { Name = "X2", Key = Keys.Viewport, Axis = Axis.X };
            viewportX2.PropertyChanged += (s, e) => propertyChanged(treeView, viewportX2, e.PropertyName, list2);
            viewportY2 = new Viewport() { Name = "Y2", Key = Keys.Viewport, Axis = Axis.Y };
            viewportY2.PropertyChanged += (s, e) => propertyChanged(treeView, viewportY2, e.PropertyName, list2);

            void propertyChanged(TreeView treeView, Viewport viewport, string name, List<TreeViewItem> _list)
            {
                if (_list.Count != 0)
                    treeView.Dispatcher.BeginInvoke(new Action(() =>
                    {
                        switch (name)
                        {
                            case nameof(Viewport.Zoom):
                            case nameof(Viewport.Length):
                            case nameof(Viewport.MinimumSpacing):
                            case nameof(Viewport.Offset):
                            case nameof(Viewport.Start):
                            case nameof(Viewport.End):
                                refresh(viewport);
                                break;
                        }
                    }));

                void refresh(Viewport viewport)
                {
                    List<TreeViewItem> removals = new();
                    foreach (var item in _list)
                    {
                        if (item is ClipBase { DataContext: { } context } clipBase && getUpdater(context) is { } updater)
                        {
                            //var layout = layouts.SingleOrDefault(l => l.Name == viewport.Name);
                            updater.Update(clipBase, new UpdateContext(viewport));
                        }
                        else
                        {
                            removals.Add(item);
                        }
                    }
                    foreach (var item in removals)
                        _list.Remove(item);

                }
            }
        }

        private void TreeViewItem_Rendering(object sender, RenderEventArgs e)
        {
            if (sender is FrameworkElement { DataContext: ViewModel { } viewmodel } element)
                if (viewmodel.Parent.FindChild<ViewModel>(Keys.Playlist) is { } playlist)
                    if (getUpdater(viewmodel) is { } updater)
                    {

                        if (viewmodel.Group == Groups.One)
                            foreach (var viewport in GroupOneViewports)
                            {
                                updater.Update(element, new RenderContext(viewport, e.DrawingContext, playlist));
                            }
                        if (viewmodel.Group == Groups.Two)
                            foreach (var viewport in GroupTwoViewports)
                            {
                                updater.Update(element, new RenderContext(viewport, e.DrawingContext, playlist));
                            }
                    }
        }

        private void TreeViewItem_Loaded(object sender, RoutedEventArgs e)
        {
            if (sender is not ClipBase clipBase)
                throw new Exception("R  dfddf");

            clipBase.ApplyTemplate();

            if (clipBase.DataContext is Viewport _viewport)
            {
                var parent = VisualTreeExtensions.FindParent<TreeViewItem>(sender as TreeViewItem);
                parent = VisualTreeExtensions.FindParent<TreeViewItem>(parent as TreeViewItem);

                if (_viewport.Axis == Axis.X)
                {
                    _viewport.Length = parent.ActualWidth;
                }
                if (_viewport.Axis == Axis.Y)
                {
                    _viewport.Length = parent.ActualHeight;
                }

                parent.SizeChanged += (s, e) =>
                {
                    if (_viewport.Axis == Axis.X)
                    {
                        _viewport.Length = parent.ActualWidth;
                    }
                    if (_viewport.Axis == Axis.Y)
                    {
                        _viewport.Length = parent.ActualHeight;
                    }
                    ;
                };
            }

            if (clipBase.DataContext is ViewModel { Key: Keys.Renderer })
            {
                Dispatcher.BeginInvoke(
                    DispatcherPriority.ApplicationIdle,
                    new Action(() =>
                    {
                        clipBase.InvalidateVisual();
                    }));
            }

            Dispatcher.BeginInvoke(
            DispatcherPriority.ApplicationIdle,
            new Action(() =>
            {
                if (getUpdater(clipBase.DataContext) is { } updater)
                    if (clipBase.DataContext is Notification { } notification)
                    {
                        if (notification.Group == Groups.One)
                        {
                            list.Add(sender as TreeViewItem);

                            foreach (var viewport in GroupOneViewports)
                            {
                                //    var layout = layouts.SingleOrDefault(l => l.Name == viewport.Name);
                                updater.Update(clipBase, new InitialisationContext(viewport));
                            }
                        }
                        else if (notification.Group == Groups.Two)
                        {
                            list2.Add(sender as TreeViewItem);

                            foreach (var viewport in GroupTwoViewports)
                            {
                                //var layout = layouts.SingleOrDefault(l => l.Name == viewport.Name);
                                updater.Update(clipBase, new InitialisationContext(viewport));
                            }
                        }
                    }
                    else
                    {

                    }
                // child containers usually available here
            }));
        }

        IEnumerable<Viewport> GroupOneViewports => [viewportX, viewportY];
        IEnumerable<Viewport> GroupTwoViewports => [viewportX2, viewportY2];

        List<IUpdater> updaters = [new CountryUpdater()];

        IUpdater getUpdater(object dataContext)
        {
            return dataContext is IUpdater updater ? updater : updaters.SingleOrDefault(u => u.CanUpdate(dataContext));

        }
    }
}
