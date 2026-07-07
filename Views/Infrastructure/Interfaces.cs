using System;
using System.Collections.Generic;
using System.Text;

namespace Views
{
    public interface IWidth
    {
        double? Width { get; set; }
    }
    public interface IHeight
    {
        double? Height { get; set; }
    }
    public interface ILeft
    {
        double? Left { get; set; }
    }
    public interface ITop
    {
        double? Top { get; set; }
    }

    public interface ISize : IWidth, IHeight, ILeft, ITop
    {
        
    }
}
