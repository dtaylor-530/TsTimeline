using System;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace SandBox
{
    public partial class ViewModel : IUpdater
    {
        private FrameworkElement center;

        TimelineTickGenerator X1TickGenerator = new TimelineTickGenerator();
        ReverseTimelineTickGenerator Y1TickGenerator = new ReverseTimelineTickGenerator();

        public bool CanUpdate(FrameworkElement element)
        {
            return element.DataContext is ViewModel;
        }

        public void Update(FrameworkElement element, object context)
        {
            if (context is not Context { UpdateType: { } updateType } _context)
                throw new Exception("DS 34");


            if (element is ClipBase { DataContext: Notification dataContext } renderer &&
                context is RenderContext axisRenderContext)
            {
                render_Measure(renderer, dataContext, axisRenderContext.Viewport, axisRenderContext.DrawingContext, axisRenderContext.Playlist);
                return;
            }
            if (element is ClipBase { DataContext: Notification { Key: Keys.Renderer } } _renderer &&
                context is UpdateContext { UpdateType: UpdateType.Viewport })
            {
                _renderer.InvalidateVisual();
                return;
            }
            if (element is ClipBase clipBase && _context is Context { Viewport: Viewport viewport })
            {
                if (clipBase.DataContext is ViewModel { Parent.Key: Keys.Progress })
                {
                    //if (this.Direction == Direction.Up || this.Direction == Direction.Down && viewport.Axis == Axis.X)
                    if (viewport.Axis == Axis.X && clipBase.DataContext is ViewModel { Parent: ViewModel { Axis: Axis.X } })
                        (this.Parent as ViewModel).initialise_TimeLine(clipBase, this, viewport);
                    if (viewport.Axis == Axis.Y && clipBase.DataContext is ViewModel { Parent: ViewModel { Axis: Axis.Y } })
                        (this.Parent as ViewModel).initialise_TimeLine(clipBase, this, viewport);
                    //if (this.Direction == Direction.Left || this.Direction == Direction.Right && viewport.Axis == Axis.Y)
                    //if (viewport.Axis == Axis.Y)
                    //    (this.Parent as ViewModel).initialise_TimeLine(clipBase, this, viewport);
                    return;
                }

                else if (clipBase.DataContext is ViewModel { Key: Keys.Point } viewModel)
                {
                    update_Point(clipBase, viewport, updateType);
                    return;
                }
                else if (clipBase.DataContext is ViewModel { Key: Keys.Playlist } _viewmodel)
                {
                    if (updateType == UpdateType.Initilisation)
                        initialise_Playlist(clipBase, viewport);
                    return;
                }
                else if (clipBase.DataContext is ViewModel { Key: Keys.BandClip } bviewmodel && viewport.Axis == Axis.Y && this.Group == viewport.Group)
                {
                    bviewmodel.Height = viewport.MinimumSpacing * viewport.Zoom;
                }
                else if (clipBase.DataContext is ViewModel { Key: Keys.ChartLine } cviewmodel && this.Group == viewport.Group)
                {
                    if (viewport.Group == Groups.One)
                    {
                        if (viewport.Axis == Axis.Y)
                        {
                            cviewmodel.WorldHeight =  cviewmodel.Height * viewport.Zoom;
                            cviewmodel.WorldY = viewport.Length - cviewmodel.Y * viewport.Zoom - ChartFactory.PointWidth / 2;
                        }
                        else if (viewport.Axis == Axis.X)
                        {
                            cviewmodel.WorldWidth = cviewmodel.Width * viewport.Zoom;
                            cviewmodel.WorldX = cviewmodel.X * viewport.Zoom + ChartFactory.PointWidth / 2;
                        }
                    }
                    else if (viewport.Group == Groups.Two)
                    {
                        if (viewport.Axis == Axis.Y)
                        {
                            cviewmodel.WorldHeight = App.viewportY2.MinimumSpacing * ((cviewmodel.Height/ (App.viewportY.End - App.viewportY.Start))) * viewport.Zoom;
                            cviewmodel.WorldY = App.viewportY2.MinimumSpacing * (1-( cviewmodel.Y / (App.viewportY.End - App.viewportY.Start))) * viewport.Zoom - ChartFactory.PointWidth / 2;
                        }
                        else if (viewport.Axis == Axis.X)
                        {
                            cviewmodel.WorldWidth = cviewmodel.Width * viewport.Zoom;
                            cviewmodel.WorldX = cviewmodel.X * viewport.Zoom + ChartFactory.PointWidth / 2;
                        }
                    }

                }
                else if (clipBase.DataContext is ViewModel { Key: Keys.TrackClip } tCviewmodel && viewport.Axis == Axis.Y && this.Group == viewport.Group)
                {
                    tCviewmodel.Height = viewport.MinimumSpacing * viewport.Zoom;
                }
                else if (clipBase.DataContext is ViewModel { Key: Keys.TriggerClip } viewmodel && viewport.Axis == Axis.X && this.Group == viewport.Group)
                {
                    if (updateType == UpdateType.Initilisation)
                    {
                        thumb ??= clipBase.TemplateChild<Thumb>("PART_THUMB");
                        startValue = X;
                        X = startValue * viewport.Zoom;
                        if (thumb == null)
                            return;

                        if (thumb != null)
                        {
                            new ThumbDragToMousePointConverter(thumb, () => { }).BindDragDelta(_Thumb_OnDragDelta);

                            void _Thumb_OnDragDelta(Vector vector)
                            {
                                if (viewport == null)
                                    return;
                                startValue += vector.X;
                                X = Math.Clamp(startValue * viewport.Zoom, viewport.Start, viewport.End);
                            }
                        }
                    }
                    else if (updateType == UpdateType.Viewport)
                    {
                        X = Math.Clamp(startValue * viewport.Zoom, viewport.Start, viewport.End);
                    }

                    return;
                }
                if (clipBase.DataContext is ViewModel { Key: Keys.BandClip } && viewport.Axis == Axis.X && this.Group == viewport.Group)
                {
                    if (updateType == UpdateType.Initilisation)
                    {
                        startValue = this.X;
                        endValue = this.X + this.Width;
                    }
                    center ??= clipBase.TemplateChild<FrameworkElement>("PART_CENTER");
                    if (center == null)
                        return;

                    Width = (endValue - startValue) * viewport.Zoom;
                    X = startValue * viewport.Zoom;

                    clipBase.Width = Width;
                    center.Width = Width;
                    return;
                }
                if (clipBase.DataContext is ViewModel { Key: Keys.HoldClip })
                {
                    if (updateType == UpdateType.Initilisation)
                    {
                        if (viewport is { Axis: Axis.X })
                        {
                            initialise_HoldClip(clipBase, viewport);
                        }
                        if (viewport is { Axis: Axis.Y })
                        {

                        }
                    }
                    else
                    {
                        if (viewport is { Axis: Axis.X })
                            update_HoldClip(clipBase, viewport);
                    }

                    return;
                }
                if (clipBase.DataContext is ViewModel { Key: Keys.Axis, Name: { } name, Axis: { } axis } axisModel)
                {
                    if (viewport is { Axis: Axis.X } viewportX)
                    {
                        if (name == "X")
                        {
                            //XAxisFactory.Build(axisModel, viewportX);
                            //var model = rendererViewModel.AxisFactory.Build(viewportX);
                            //clipBase.Model = model;
                            axisModel.Clear();
                            axisModel.Add([.. X1TickGenerator.Generate(viewport)]);
                            clipBase.InvalidateVisual();
                        }
                    }
                    else if (viewport is { Axis: Axis.Y } viewportY)
                    {
                        if (name == "Y")
                        {
                            //var model = rendererViewModel.AxisFactory.Build(viewportY);
                            //clipBase.Model = model;
                            //YAxisFactory.Build(axisModel, viewportY);
                            axisModel.Clear();
                            axisModel.Add([.. Y1TickGenerator.Generate(viewport)]);
                            clipBase.InvalidateVisual();
                        }
                    }
                    return;
                }
            }
        }

        //public static Panel GetItemsHost(TreeViewItem item)
        //{
        //    ItemsPresenter presenter = FindVisualChild<ItemsPresenter>(item);

        //    if (presenter == null)
        //    {
        //        item.ApplyTemplate();
        //        presenter = FindVisualChild<ItemsPresenter>(item);
        //    }

        //    return presenter != null
        //        ? VisualTreeHelper.GetChild(presenter, 0) as Panel
        //        : null;
        //}

        public static T FindVisualChild<T>(DependencyObject parent)
            where T : DependencyObject
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(parent, i);

                if (child is T result)
                    return result;

                T descendant = FindVisualChild<T>(child);
                if (descendant != null)
                    return descendant;
            }

            return null;
        }

        public bool CanUpdate(object clipBase)
        {
            return this == clipBase;
        }
    }
}
