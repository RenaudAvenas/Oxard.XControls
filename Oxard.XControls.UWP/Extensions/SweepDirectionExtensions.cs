using Oxard.XControls.Shapes;
using System;

namespace Oxard.XControls.UWP.Extensions
{
    public static class SweepDirectionExtensions
    {
        public static Windows.UI.Xaml.Media.SweepDirection ToSweepDirection(this SweepDirection sweepDirection)
        {
            switch (sweepDirection)
            {
                case SweepDirection.Counterclockwise:
                    return Windows.UI.Xaml.Media.SweepDirection.Counterclockwise;
                case SweepDirection.Clockwise:
                    return Windows.UI.Xaml.Media.SweepDirection.Clockwise;
                default:
                    throw new ArgumentOutOfRangeException(nameof(sweepDirection), sweepDirection, null);
            }
        }

        public static SweepDirection ToXamarinSweepDirection(Windows.UI.Xaml.Media.SweepDirection sweepDirection)
        {
            switch (sweepDirection)
            {
                case Windows.UI.Xaml.Media.SweepDirection.Counterclockwise:
                    return SweepDirection.Counterclockwise;
                case Windows.UI.Xaml.Media.SweepDirection.Clockwise:
                    return SweepDirection.Clockwise;
                default:
                    throw new ArgumentOutOfRangeException(nameof(sweepDirection), sweepDirection, null);
            }
        }
    }
}
