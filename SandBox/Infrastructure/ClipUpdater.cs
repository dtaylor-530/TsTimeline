using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using TsTimeline;

namespace SandBox
{
    public class ClipUpdater : IUpdater
    {
        private FrameworkElement point;
        private Path path;

        public void UpdateX(ClipBase clipBase)
        {
            if (clipBase.DataContext is Country country)
            {
                var path = clipBase.GetType().GetMethod("GetTemplateChild", BindingFlags.Instance | BindingFlags.NonPublic).Invoke(clipBase, ["PART_PATH"]) as Path;
                if (path == null)
                    return;

                Canvas.SetLeft(clipBase, (country.Left.Value - clipBase.ViewportX.Offset )* clipBase.ViewportX.Zoom);
                Canvas.SetTop(clipBase, (country.Top.Value - clipBase.ViewportY.Offset) * clipBase.ViewportY.Zoom);
                clipBase.Width = country.Width.Value * clipBase.ViewportX.Zoom;
                clipBase.Height = country.Height.Value * clipBase.ViewportY.Zoom;

                path.Data = Geometry.Parse(country.Data);

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
                        X = country.Translate_X ?? default,
                        Y = country.Translate_Y ?? default,
                    });

                path.RenderTransform = transformGroup;

                return;
            }

            {
                point ??= clipBase.GetType().GetMethod("GetTemplateChild", BindingFlags.Instance | BindingFlags.NonPublic).Invoke(clipBase, ["PART_POINT"]) as FrameworkElement;
                if (point == null)
                    return;

                Canvas.SetLeft(clipBase, clipBase.X * clipBase.ViewportX.Zoom);
            }
        }

        public void UpdateY(ClipBase clipBase)
        {
            point ??= clipBase.GetType().GetMethod("GetTemplateChild", BindingFlags.Instance | BindingFlags.NonPublic).Invoke(clipBase, ["PART_POINT"]) as FrameworkElement;
            if (point == null)
                return;

            // TODO: value needs to be calculated based on height of container 
            var y = clipBase.ViewportY.ViewportLength - clipBase.Y * clipBase.ViewportY.ViewportLength / (clipBase.ViewportY.End - clipBase.ViewportY.Start);
            Canvas.SetTop(clipBase, y);
        }
    }
}
