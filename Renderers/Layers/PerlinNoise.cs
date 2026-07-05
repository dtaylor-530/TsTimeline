namespace Renderers
{
    public interface INoise
    {
        double Noise(double x, double y);

        double Fractal(
            double x,
            double y,
            int octaves,
            double persistence = 0.5,
            double lacunarity = 2.0);
    }

    public sealed class PerlinNoise : INoise
    {
        private readonly int[] p = new int[512];

        public PerlinNoise(int seed = 1)
        {
            var permutation = new int[256];

            for (int i = 0; i < 256; i++)
                permutation[i] = i;

            var random = new Random(seed);

            for (int i = 255; i > 0; i--)
            {
                int j = random.Next(i + 1);

                (permutation[i], permutation[j]) =
                    (permutation[j], permutation[i]);
            }

            for (int i = 0; i < 512; i++)
                p[i] = permutation[i & 255];
        }

        public double Noise(double x, double y)
        {
            int X = (int)Math.Floor(x) & 255;
            int Y = (int)Math.Floor(y) & 255;

            x -= Math.Floor(x);
            y -= Math.Floor(y);

            double u = Fade(x);
            double v = Fade(y);

            int A = p[X] + Y;
            int B = p[X + 1] + Y;

            return Lerp(
                v,

                Lerp(
                    u,
                    Grad(p[A], x, y),
                    Grad(p[B], x - 1, y)),

                Lerp(
                    u,
                    Grad(p[A + 1], x, y - 1),
                    Grad(p[B + 1], x - 1, y - 1)));
        }

        public double Fractal(
            double x,
            double y,
            int octaves,
            double persistence = 0.5,
            double lacunarity = 2.0)
        {
            double amplitude = 1;
            double frequency = 1;

            double total = 0;
            double max = 0;

            for (int i = 0; i < octaves; i++)
            {
                total += Noise(
                    x * frequency,
                    y * frequency) * amplitude;

                max += amplitude;

                amplitude *= persistence;
                frequency *= lacunarity;
            }

            return total / max;
        }

        private static double Fade(double t)
        {
            return t * t * t *
                   (t * (t * 6 - 15) + 10);
        }

        private static double Lerp(
            double t,
            double a,
            double b)
        {
            return a + t * (b - a);
        }

        private static double Grad(
            int hash,
            double x,
            double y)
        {
            switch (hash & 3)
            {
                case 0:
                    return x + y;

                case 1:
                    return -x + y;

                case 2:
                    return x - y;

                default:
                    return -x - y;
            }
        }
    }
}
