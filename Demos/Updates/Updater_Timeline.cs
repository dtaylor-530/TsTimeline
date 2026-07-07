using System.Windows.Shapes;

namespace Demos
{
    public partial class Updater
    {
        void initialise_TimeLine(ClipBase clipBase, ViewModel viewmodel, Viewport viewport)
        {
            if (viewmodel is not ViewModel { Parent: ViewModel _parent } )
            {
                throw new Exception("W g fd45 fhg");
            }

            var parent = clipBase.FindParent<ClipBase>();


            //if (binder == null)
            //{
            if (clipBase.TemplateChild<Thumb>("PART_THUMB") is { } _thumb)
            {
                var binder = new ThumbDragToMousePointConverter(
                    _thumb,
                    () => { });
                binder.BindDragDelta(_Thumb_OnDragDelta);
                if (viewmodel.VerticalAlignment == VerticalAlignment.Bottom)
                    Canvas.SetBottom(clipBase, 0);
            }
            //}

            if (viewmodel.Direction == Direction.Up || viewmodel.Direction == Direction.Down)
            {             
                if (viewmodel.Key == Keys.Area)
                    viewmodel?.WorldHeight = parent.ActualHeight;
                if (viewmodel.Key == Keys.Line)
                {
                    viewmodel?.WorldWidth = 1;
                    viewmodel?.WorldHeight = parent.ActualHeight;
                }
            }
            else if (viewmodel.Direction == Direction.Right || viewmodel.Direction == Direction.Left)
            {
                if (viewmodel.Key == Keys.Area)
                    viewmodel?.WorldWidth = parent.ActualWidth - 16;
                if (viewmodel.Key == Keys.Line)
                {
                    viewmodel?.WorldWidth = parent.ActualWidth;
                    viewmodel?.WorldHeight = 1;
                }
            }
  
            void _Thumb_OnDragDelta(Vector vector)
            {

                var _clipBase = clipBase;
                if (viewport == null)
                    return;

                if (viewmodel.Direction == Direction.Up || viewmodel.Direction == Direction.Down)
                {
                    double worldDelta = vector.X;

                    _parent.X += worldDelta;
                    if (_parent.X < 0)
                        _parent.X = 0;
                    //X = Math.Clamp(X + worldDelta, viewport.Start, viewport.End);


                }
                else if (viewmodel.Direction == Direction.Right || viewmodel.Direction == Direction.Left)
                {
                    double worldDelta = vector.Y;

                    _parent.Y += worldDelta;
                    if (_parent.Y < 0)
                        _parent.Y = 0;
                    if (parent is { } p && _parent.Y > p.ActualHeight)
                        _parent.Y = parent.ActualHeight;
                    //X = Math.Clamp(X + worldDelta, viewport.Start, viewport.End);


                }
            }
        }
    }
}
