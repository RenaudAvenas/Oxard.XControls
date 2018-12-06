using System;

namespace Oxard.XControls.Extensions
{
    public static class DoubleExtensions
    {
        public static bool DoubleIsEquals(this double a, double b, double tolerance = 0.0001) => Math.Abs(a - b) < tolerance;

        public static double TranslateIfInfinity(this double a, double defaultValue = 0)
        {
            return double.IsInfinity(a) ? defaultValue : a;
        }

        public static double TranslateIfNegative(this double a, double defaultValue = 0)
        {
            return a < 0 ? defaultValue : a;
        }
    }
}
