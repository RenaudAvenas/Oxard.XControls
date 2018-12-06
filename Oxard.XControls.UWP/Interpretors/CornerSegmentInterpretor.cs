using Oxard.XControls.Extensions;
using Oxard.XControls.Graphics;
using Oxard.XControls.UWP.Extensions;
using System;
using Windows.UI.Xaml.Media;
using Xamarin.Forms;

namespace Oxard.XControls.UWP.Interpretors
{
    public class CornerSegmentInterpretor : ISegmentInterpretor
    {
        public PathSegment ToNativeSegment(GeometrySegment segment, Point fromPoint)
        {
            var cornerSegment = (CornerSegment)segment;
            var rx = Math.Abs(segment.EndPoint.X - fromPoint.X) / 2d;
            var ry = Math.Abs(segment.EndPoint.Y - fromPoint.Y) / 2d;

            if (rx.DoubleIsEquals(0) || ry.DoubleIsEquals(0))
                return new Windows.UI.Xaml.Media.LineSegment { Point = segment.EndPoint.ToPoint() };
            else
                return new ArcSegment { Point = segment.EndPoint.ToPoint(), Size = new Windows.Foundation.Size(rx, ry), SweepDirection = cornerSegment.SweepDirection.ToSweepDirection() };
        }
    }
}
