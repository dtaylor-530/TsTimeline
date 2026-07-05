using System.Windows.Media.Imaging;

namespace Renderers
{
    public interface IDrawingLayer
    {
        //string Name { get; }
        bool Enabled { get; set; }

        int Order { get; }

        void Draw(
            DrawingContext dc,
            Rect bounds,
            RenderContext context);
    }

    public sealed class RenderContext
    {
        public double Time { get; set; }

        public double Zoom { get; set; }

        public Point Offset { get; set; }

        public Random Random { get; } = new Random();

        public Size Size { get; }

        public Dictionary<string, object> Cache { get; }

        public PerlinNoise Noise { get; } = new(seed: 1);

        public int Seed { get; set; }
    }

    public abstract class CachedBitmapLayer : IDrawingLayer
    {
        private WriteableBitmap? _bitmap;
        private bool _dirty = true;

        public bool Enabled { get; set; } = true;

        public abstract int Order { get; }

        /// <summary>
        /// Marks the cached bitmap as stale so it will be regenerated on the next Draw call.
        /// Call this whenever the RenderContext changes (e.g. noise seed or other parameters).
        /// </summary>
        public void Invalidate() => _dirty = true;

        public void Draw(
            DrawingContext dc,
            Rect bounds,
            RenderContext context)
        {
            int w = Math.Max(1, (int)bounds.Width);
            int h = Math.Max(1, (int)bounds.Height);

            if (_bitmap == null ||
                _bitmap.PixelWidth != w ||
                _bitmap.PixelHeight != h ||
                _dirty)
            {
                _bitmap = new WriteableBitmap(
                    w,
                    h,
                    96,
                    96,
                    PixelFormats.Pbgra32,
                    null);

                Generate(_bitmap, context);
                _dirty = false;
            }

            dc.DrawImage(
                _bitmap,
                new Rect(0, 0, w, h));
        }

        protected abstract void Generate(
            WriteableBitmap bitmap,
            RenderContext context);
    }
}