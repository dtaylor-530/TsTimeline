using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using TsTimeline;

namespace SandBox
{
    public class ItemPointClipUpdater : IUpdater
    {
        private FrameworkElement point;

        public void Update(ClipBase clipBase)
        {

            point ??= clipBase.GetType().GetMethod("GetTemplateChild", BindingFlags.Instance | BindingFlags.NonPublic).Invoke(clipBase, ["PART_POINT"]) as FrameworkElement;
            if (point == null)
                return;

            Canvas.SetLeft(clipBase, clipBase.X * clipBase.Viewport.ScaleX * clipBase.Viewport.ZoomX);
            // TODO: value needs to be calculated based on height of container 
            Canvas.SetTop(clipBase, 1);

        }
    }
}
