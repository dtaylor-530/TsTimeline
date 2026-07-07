namespace Common
{
    public static class MathHelpers
    {
        /// <summary>
        /// quintic easing function
        /// zero velocity and zero acceleration at both ends
        /// </summary>
        public static double SmootherStep(double t)
        {
            return t * t * t * (t * (t * 6 - 15) + 10);
        }

        public static double Lerp(double a, double b, double t)
        {
            return a + (b - a) * t;
        }
    }
}
