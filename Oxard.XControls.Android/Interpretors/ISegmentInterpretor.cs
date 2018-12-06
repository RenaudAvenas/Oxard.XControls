using Android.Content;
using Android.Graphics;
using Oxard.XControls.Graphics;
using Oxard.XControls.Interpretors;
using Point = Xamarin.Forms.Point;

namespace Oxard.XControls.Droid.Interpretors
{
    public interface ISegmentInterpretor : IInterpretor
    {
        void AddToPath(GeometrySegment segment, Point fromPoint, Path path, Context context);
    }
}