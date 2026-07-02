using System;
using System.Collections.Generic;
using System.Formats.Tar;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Shapes;

namespace SandBox
{
    public partial class ViewModel
    {
        private Dictionary<ClipBase, FrameworkElement> areas = new();

        private ClipBase line;
        private ClipBase _thumb;
        private Thumb thumb;

        private ThumbDragToMousePointConverter binder;
        private ClipBase _area;

                void initialise_TimeLine(ClipBase clipBase, ViewModel viewmodel, Viewport viewport)
        {
            thumb ??= clipBase.TemplateChild<Thumb>("PART_THUMB");
            _thumb ??= clipBase.TemplateChild<Thumb>("PART_THUMB") is { } ? clipBase : null;
            _area ??= clipBase.TemplateChild<FrameworkElement>("PART_AREA") is { } ? clipBase : null; 
            line ??= clipBase.TemplateChild<Rectangle>("PART_LINE") is { } ? clipBase : null;

            this.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == nameof(X) && X < viewport.Length && X >= 0)
                {
                    (line.DataContext as ViewModel).X = X;
                    (_thumb.DataContext as ViewModel).X = X;
                    (_area.DataContext as ViewModel)?.Width = X;
                }
                if (e.PropertyName == nameof(Y) && Y < viewport.Length && Y >= 0)
                {
                    (line.DataContext as ViewModel).Y = Y;
                    (_thumb.DataContext as ViewModel).Y = Y;
                    (_area.DataContext as ViewModel)?.Height = Y;
                }
            };

            if (thumb == null)
                return;

            var parent = clipBase.FindParent<ClipBase>();

            if (binder == null)
            {
                binder = new ThumbDragToMousePointConverter(
                    thumb,
                    () => { });
                binder.BindDragDelta(_Thumb_OnDragDelta);

            }

            if (this.Direction == Direction.Right || this.Direction == Direction.Left)
            {
                line?.Height = parent.ActualHeight;
                _area?.Height = parent.ActualHeight;
                line?.Width = 1;
            }
            else if (this.Direction == Direction.Up || this.Direction == Direction.Down)
            {
                line?.Width = parent.ActualWidth;
                _area?.Width = parent.ActualWidth - 16;
                line?.Height = 1;
            }

            if (_thumb.VerticalAlignment == VerticalAlignment.Bottom)
                Canvas.SetBottom(_thumb, 0);

            void _Thumb_OnDragDelta(Vector vector)
            {

                var _clipBase = clipBase;
                if (viewport == null)
                    return;

                if (this.Direction == Direction.Right || this.Direction == Direction.Left)
                {
                    double worldDelta = vector.X;

                    X += worldDelta;
                    if (X < 0)
                        X = 0;
                    //X = Math.Clamp(X + worldDelta, viewport.Start, viewport.End);


                }
                else if (this.Direction == Direction.Up || this.Direction == Direction.Down)
                {
                    double worldDelta = vector.Y;

                    Y += worldDelta;
                    if (Y < 0)
                        Y = 0;
                    if (parent is { } p && Y > p.ActualHeight)
                        Y = parent.ActualHeight;
                    //X = Math.Clamp(X + worldDelta, viewport.Start, viewport.End);


                }
            }
        }
    }
}
