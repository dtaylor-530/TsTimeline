namespace Demos
{
    public partial class Updater : IUpdater
    {
        TimelineTickGenerator X1TickGenerator = new TimelineTickGenerator();
        ReverseTimelineTickGenerator Y1TickGenerator = new ReverseTimelineTickGenerator();

        public bool CanUpdate(object context)
        {
            if (context is not Context { Element: { } element } _context)
                throw new Exception("DS 34");
            return element.DataContext is ViewModel;
        }

        public void Update(object context)
        {
            if (context is not Context { Element: { } element, UpdateType: { } updateType } _context)
                throw new Exception("DS 34");

            if (element.DataContext is not ViewModel viewmodel)
            {
                throw new Exception("W g fd45 fhg");
            }
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
                if (clipBase.DataContext is ViewModel { Key: Keys.Progress } && _context.UpdateType == UpdateType.Initilisation)
                {
                    viewmodel.PropertyChanged += (s, e) =>
                    {
                        if (e.PropertyName == nameof(ViewModel.X) && viewmodel.X < viewport.Length && viewmodel.X >= 0)
                        {
                            foreach (var child in viewmodel.FindChildren<ViewModel>(Keys.Line))
                                child.WorldX = viewmodel.X;
                            foreach (var child in viewmodel.FindChildren<ViewModel>(Keys.Area))
                                child.WorldWidth = viewmodel.X;
                            foreach (var child in viewmodel.FindChildren<ViewModel>(Keys.TimelineThumb))
                                child.X = viewmodel.X;
                        }
                        if (e.PropertyName == nameof(ViewModel.Y) && viewmodel.Y < viewport.Length && viewmodel.Y >= 0)
                        {
                            foreach (var child in viewmodel.FindChildren<ViewModel>(Keys.Line))
                                child.WorldY = viewmodel.Y;
                            foreach (var child in viewmodel.FindChildren<ViewModel>(Keys.Area))
                                child.WorldHeight = viewmodel.Y;
                            foreach (var child in viewmodel.FindChildren<ViewModel>(Keys.TimelineThumb))
                                child.Y = viewmodel.Y;
                        }
                    };

                    return;
                }
                else if (clipBase.DataContext is ViewModel { Parent.Key: Keys.Progress })
                {
                    if (viewport.Axis == Axis.X && clipBase.DataContext is ViewModel { Parent: ViewModel { Axis: Axis.X } })
                        initialise_TimeLine(clipBase, viewmodel, viewport);
                    if (viewport.Axis == Axis.Y && clipBase.DataContext is ViewModel { Parent: ViewModel { Axis: Axis.Y } })
                        initialise_TimeLine(clipBase, viewmodel, viewport);
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
                else if (clipBase.DataContext is ViewModel { Key: Keys.ChartLine } cviewmodel && viewmodel.Group == viewport.Group)
                {
                    if (viewport.Group == Groups.One)
                    {
                        if (viewport.Axis == Axis.Y)
                        {
                            cviewmodel.WorldHeight = cviewmodel.Height * viewport.Zoom;
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
                            cviewmodel.WorldHeight = App.viewportY2.MinimumSpacing * ((cviewmodel.Height / (App.viewportY.End - App.viewportY.Start))) * viewport.Zoom;
                            cviewmodel.WorldY = App.viewportY2.MinimumSpacing * (1 - (cviewmodel.Y / (App.viewportY.End - App.viewportY.Start))) * viewport.Zoom - ChartFactory.PointWidth / 2;
                        }
                        else if (viewport.Axis == Axis.X)
                        {
                            cviewmodel.WorldWidth = cviewmodel.Width * viewport.Zoom;
                            cviewmodel.WorldX = cviewmodel.X * viewport.Zoom + ChartFactory.PointWidth / 2;
                        }
                    }

                }
                else if (clipBase.DataContext is ViewModel { Key: Keys.TrackClip } tCviewmodel && viewport.Axis == Axis.Y && viewmodel.Group == viewport.Group)
                {
                    tCviewmodel.Height = viewport.MinimumSpacing * viewport.Zoom;
                }
                else if (clipBase.DataContext is ViewModel { Key: Keys.TriggerClip } && viewport.Axis == Axis.X && viewmodel.Group == viewport.Group)
                {
                    if (updateType == UpdateType.Initilisation)
                    {
                        var thumb = clipBase.TemplateChild<Thumb>("PART_THUMB");
                        viewmodel.WorldX = viewmodel.X * viewport.Zoom;
    
                        if (thumb != null)
                        {
                            new ThumbDragToMousePointConverter(thumb, () => { }).BindDragDelta(_Thumb_OnDragDelta);

                            void _Thumb_OnDragDelta(Vector vector)
                            {
                                if (viewport == null)
                                    return;
                                viewmodel.X += vector.X / viewport.Zoom;
                                viewmodel.WorldX = Math.Clamp(viewmodel.X * viewport.Zoom, viewport.Start * viewport.Zoom, viewport.End * viewport.Zoom);
                            }
                        }
                    }
                    else if (updateType == UpdateType.Viewport)
                    {
                        viewmodel.X = Math.Clamp(viewmodel.X * viewport.Zoom, viewport.Start, viewport.End);
                    }

                    return;
                }
                else if (clipBase.DataContext is ViewModel { Key: Keys.BandClip } bviewmodel && viewmodel.Group == viewport.Group)
                {
                    if (viewport.Axis == Axis.Y)
                        bviewmodel.Height = viewport.MinimumSpacing * viewport.Zoom;
                    else
                    {
                        viewmodel.WorldWidth = viewmodel.Width * viewport.Zoom;
                        viewmodel.WorldX = viewmodel.X * viewport.Zoom;
                        return;
                    }
                }
                else if (clipBase.DataContext is ViewModel { Key: Keys.HoldClip } hc_viewmodel)
                {

                    if (updateType == UpdateType.Initilisation)
                    {
                        if (viewport.Axis == Axis.Y)
                        {
                            hc_viewmodel.Height = viewport.MinimumSpacing;
                        }
                        if (viewport.Axis == Axis.X)
                        {
                            hc_viewmodel.WorldX = hc_viewmodel.X * viewport.Zoom;
                            hc_viewmodel.WorldWidth = hc_viewmodel.Width * viewport.Zoom;
                            //}
                            //else
                            //{
                            //if (viewport is { Axis: Axis.X })
                            //    update_HoldClip(clipBase, viewport);
                            hc_viewmodel.PropertyChanged += (s, e) =>
                            {
                                if (e.PropertyName == nameof(ViewModel.X))
                                {
                                    hc_viewmodel.WorldX = hc_viewmodel.X * viewport.Zoom;
                                }
                                else if (e.PropertyName == nameof(ViewModel.Width))
                                {
                                    hc_viewmodel.WorldWidth = hc_viewmodel.Width * viewport.Zoom;
                                }
                            };
                        }
                    }

                    return;
                }
                else if (clipBase.DataContext is ViewModel { Key: Keys.HoldclipThumb })
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
                else if (clipBase.DataContext is ViewModel { Key: Keys.Axis, Name: { } name, Axis: { } axis } axisModel)
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
    }
}
