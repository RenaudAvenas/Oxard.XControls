using Android.Content;
using Android.Graphics;
using Oxard.XControls.Graphics;
using Xamarin.Forms.Platform.Android;

namespace Oxard.XControls.Droid.Interpretors
{
    public class LineSegmentInterpretor : ISegmentInterpretor
    {
        public void AddToPath(GeometrySegment segment, Xamarin.Forms.Point fromPoint, Path path, Context context)
        {
            path.LineTo(context.ToPixels(segment.EndPoint.X), context.ToPixels(segment.EndPoint.Y));
        }
    }
}