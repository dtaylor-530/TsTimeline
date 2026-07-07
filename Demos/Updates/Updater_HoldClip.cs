namespace Demos
{
    public partial class Updater
    {
        void update_HoldClip(ClipBase clipBase, Viewport viewport)
        {
        }

        bool initialise_HoldClip(ClipBase clipBase, Viewport viewport)
        {
            if (clipBase.DataContext is not ViewModel { Parent: ViewModel parent } viewmodel)
            {
                throw new Exception("W g fd45 fhg");
            }

            var thumb = clipBase.TemplateChild<Thumb>("PART_THUMB");

            if (viewmodel.Name == Names.Left)
            {
                var binder = new ThumbDragToMousePointConverter(thumb, () => { });
                binder.BindDragDelta(left_OnDragDelta);


            }
            else if (viewmodel.Name == Names.Center)
            {
                var binder = new ThumbDragToMousePointConverter(thumb, () => { });
                binder.BindDragDelta(center_OnDragDelta);


            }
            else if (viewmodel.Name == Names.Right)
            {
                var binder = new ThumbDragToMousePointConverter(thumb, () => { });
                binder.BindDragDelta(right_OnDragDelta);


            }
            else
                throw new Exception(" S45 tghf ");

            updateX();

            void right_OnDragDelta(Vector vector)
            {
                if (viewmodel.IsReadOnly)
                    return;

                var change = vector.X / viewport.Zoom;
                parent.Width += change;

                updateX();
            }

            void center_OnDragDelta(Vector vector)
            {
                var change = vector.X / viewport.Zoom;
                parent.X += change;
                if (parent.X < 0)
                {
                    parent.X = 0;
                }
                updateX();
            }

            void left_OnDragDelta(Vector vector)
            {
                if (viewmodel.IsReadOnly)
                    return;

                var change = vector.X / viewport.Zoom;
                parent.X += change;
               
                if (parent.X < 0)
                {
                    parent.X = 0;
                }
                else
                {
                    parent.Width += -change;
                }

                updateX();
            }

            return true;

            void updateX()
            {
                if (viewmodel.X < 0)
                {
                    viewmodel.X = 0;
                }

                if (viewmodel.Width > 0)
                {
                }
                else
                {
                }

                update_HoldClip(clipBase, viewport);

            }
        }


    }
}
