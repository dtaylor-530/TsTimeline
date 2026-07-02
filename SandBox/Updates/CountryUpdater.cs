using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace SandBox
{
    class CountryUpdater : IUpdater
    {
        public bool CanUpdate(object clipBase)
        {
            return clipBase is Country;
        }

        public void Update(FrameworkElement element, object context)
        {
            if (element is not ClipBase clipBase || context is not Context { UpdateType: { } updateType, Viewport: var viewport })
                throw new Exception("DS 34");

            if (clipBase.DataContext is not Country country)
                return;
            if (viewport.Group != Groups.One || country.Group != Groups.One)
                return;

            if (updateType == UpdateType.Initilisation)
            {
                if (viewport is { Axis: Axis.X } viewportX)
                {
                    country.UILeft = country.Left.Value * viewportX.Zoom;
                    country.UIWidth = country.Width.Value * viewportX.Zoom;
                }
                if (viewport is { Axis: Axis.Y } viewportY)
                {
                    country.UITop = country.Top.Value * viewportY.Zoom;
                    country.UIHeight = country.Height.Value * viewportY.Zoom;
                }

                var path = clipBase.TemplateChild<Path>("PART_PATH");
                if (path == null)
                    return;
                try
                {
                    path.Data = Geometry.Parse(country.Data);
                }
                catch(Exception ex)
                {

                }
                if (path.RenderTransform == null)
                {
                    var transformGroup = new TransformGroup();

                    transformGroup.Children.Add(new ScaleTransform());
                    if (country.Skew.HasValue)
                        transformGroup.Children.Add(new SkewTransform
                        {
                            AngleX = country.Skew.Value
                        });
                    if (country.Rotate.HasValue)
                        transformGroup.Children.Add(new RotateTransform
                        {
                            Angle = country.Rotate.Value
                        });
                    if (country.Translate_X.HasValue || country.Translate_Y.HasValue)
                        transformGroup.Children.Add(new TranslateTransform
                        {
                            X = 1, //country.Translate_X ?? default,
                            Y = 0,//country.Translate_Y ?? default,
                        });

                    path.RenderTransform = transformGroup;
                }
                return;
            }
            else if (updateType == UpdateType.Viewport)
            {
                if (viewport is { Axis: Axis.X } viewportX)
                {
                    country.UILeft = country.Left.Value * viewportX.Zoom;
                    country.UIWidth = country.Width.Value * viewportX.Zoom;
                }
                if (viewport is { Axis: Axis.Y } viewportY)
                {
                    country.UITop = country.Top.Value * viewportY.Zoom;
                    country.UIHeight = country.Height.Value * viewportY.Zoom;
                }
            }
        }
    }
}
