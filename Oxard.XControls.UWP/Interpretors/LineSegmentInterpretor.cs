using Oxard.XControls.Graphics;
using Oxard.XControls.UWP.Extensions;
using Windows.UI.Xaml.Media;
using Xamarin.Forms;

namespace Oxard.XControls.UWP.Interpretors
{
    public class LineSegmentInterpretor : ISegmentInterpretor
    {
        public PathSegment ToNativeSegment(GeometrySegment segment, Point fromPoint)
        {
            return new Windows.UI.Xaml.Media.LineSegment { Point = segment.EndPoint.ToPoint() };
        }
    }
}
