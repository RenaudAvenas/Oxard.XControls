using Xamarin.Forms;

namespace Oxard.XControls.UWP.Extensions
{
    public static class PointExtensions
    {
        public static Windows.Foundation.Point ToPoint(this Point point)
        {
            return new Windows.Foundation.Point(point.X, point.Y);
        }

        public static Point ToXamarinPoint(this Windows.Foundation.Point point)
        {
            return new Point(point.X, point.Y);
        }
    }
}
