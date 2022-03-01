namespace Oxard.Maui.XControls.Extensions;

/// <summary>
/// Helpers for double manipulation in controls
/// </summary>
public static class DoubleExtensions
{
    /// <summary>
    /// Return true if <paramref name="a"/> is near <paramref name="b"/> with <paramref name="tolerance"/> (default is 0.0001)
    /// </summary>
    /// <param name="a">First operand</param>
    /// <param name="b">Second operand</param>
    /// <param name="tolerance">Equality tolerance</param>
    /// <returns>True if <paramref name="a"/> - <paramref name="b"/> is inferior to <paramref name="tolerance"/></returns>
    public static bool DoubleIsEquals(this double a, double b, double tolerance = 0.0001) => Math.Abs(a - b) < tolerance;

    /// <summary>
    /// Return <paramref name="defaultValue"/> if <paramref name="a"/> is positive infinity
    /// </summary>
    /// <param name="a">The operand</param>
    /// <param name="defaultValue">Value to return if <paramref name="a"/> is positive infinity</param>
    /// <returns>If <paramref name="a"/> is positive infinity returns <paramref name="defaultValue"/> otherwise <paramref name="a"/></returns>
    public static double TranslateIfInfinity(this double a, double defaultValue = 0)
    {
        return double.IsInfinity(a) ? defaultValue : a;
    }

    /// <summary>
    /// Return <paramref name="defaultValue"/> if <paramref name="a"/> is negative
    /// </summary>
    /// <param name="a">The operand</param>
    /// <param name="defaultValue">Value to return if <paramref name="a"/> is negative</param>
    /// <returns>If <paramref name="a"/> is negative returns <paramref name="defaultValue"/> otherwise <paramref name="a"/></returns>
    public static double TranslateIfNegative(this double a, double defaultValue = 0)
    {
        return a < 0 ? defaultValue : a;
    }
}
