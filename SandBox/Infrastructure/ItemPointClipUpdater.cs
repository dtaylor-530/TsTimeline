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

            Canvas.SetLeft(clipBase, clipBase.X * clipBase.ViewportX.Scale * clipBase.ViewportX.Zoom);
            // TODO: value needs to be calculated based on height of container 
            var y = clipBase.ViewportY.ViewportLength - clipBase.Y * clipBase.ViewportY.ViewportLength / (clipBase.ViewportY.WorldEnd - clipBase.ViewportY.WorldStart);
            Canvas.SetTop(clipBase, y); 

        }
    }
}
