using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace SandBox
{
    public partial class ViewModel
    {
        private Thumb _right;
        private Thumb _left;
        private Thumb _center;
        private Grid _grid;
        public double startValue;
        public double endValue;

        //private Thumb thumb;
        //private Rectangle area;
        //private Rectangle line;
        public bool IsReadOnly { get; set; }
        public Size Size { get; private set; }

        void update_HoldClip(ClipBase clipBase, Viewport viewport)
        {
            if (X != startValue * viewport.Zoom)
                this.X = startValue * viewport.Zoom;

            if (this.Width != (endValue - startValue) * viewport.Zoom)
                this.Width = (endValue - startValue) * viewport.Zoom; 
        }

        bool initialise_HoldClip(ClipBase clipBase, Viewport viewport)
        {
            if (_left != null && _right != null && _center != null)
                return true;

            _left ??= clipBase.TemplateChild<Thumb>("PART_LEFT");
            _right ??= clipBase.TemplateChild<Thumb>("PART_RIGHT");
            _center ??= clipBase.TemplateChild<Thumb>("PART_CENTER");
            _grid ??= clipBase.TemplateChild<Grid>("PART_GRID");

            var result = _left != null && _right != null && _center != null;

            if (result)
            {
                var leftBinder = new ThumbDragToMousePointConverter(_left, () => { });
                leftBinder.BindDragDelta(left_OnDragDelta);

                var rightBinder = new ThumbDragToMousePointConverter(_right, () => { });
                rightBinder.BindDragDelta(right_OnDragDelta);

                var centerBinder = new ThumbDragToMousePointConverter(_center, () => { });
                centerBinder.BindDragDelta(center_OnDragDelta);
            }

            startValue = X;
            endValue = X + Width;
            updateX();

            void right_OnDragDelta(Vector vector)
            {
                if (IsReadOnly)
                    return;

                var change = vector.X;

                endValue += change / viewport.Zoom;
                updateX();
            }

            void center_OnDragDelta(Vector vector)
            {
                var change = vector.X;

                startValue += change / viewport.Zoom;
                endValue += change / viewport.Zoom;
                updateX();
            }

            void left_OnDragDelta(Vector vector)
            {
                if (IsReadOnly)
                    return;

                var change = vector.X;
                startValue += change / viewport.Zoom;
                updateX();
            }

            return result;

            void updateX()
            {
                if (startValue < 0)
                {
                    startValue = 0;
                    endValue = this.Width / viewport.Zoom;
                }
                //Canvas.SetLeft(clipBase, startValue * viewport.Zoom);

                var width = (endValue - startValue);

                if (width > 0)
                {
                    clipBase.Width = _center.Width = this.Width = width * viewport.Zoom;
                }
                else
                {
                }

                update_HoldClip(clipBase, viewport);
   
            }
        }


    }
}
