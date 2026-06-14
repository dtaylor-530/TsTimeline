namespace Renderers
{
    public interface IAxisFactory
    {
        AxisModel Build(Viewport viewport);
    }
}