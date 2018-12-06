using Android.Content;
using Android.Graphics;
using Oxard.XControls.Graphics;
using Oxard.XControls.Extensions;
using Xamarin.Forms.Platform.Android;
using System;
using Oxard.XControls.Shapes;

namespace Oxard.XControls.Droid.Interpretors
{
    public class CornerSegmentInterpretor : ISegmentInterpretor
    {
        public void AddToPath(GeometrySegment segment, Xamarin.Forms.Point fromPoint, Path path, Context context)
        {
            var cornerSegment = (CornerSegment)segment;

            if (cornerSegment.EndPoint.X.DoubleIsEquals(fromPoint.X) || cornerSegment.EndPoint.Y.DoubleIsEquals(fromPoint.Y))
                path.LineTo(context.ToPixels(segment.EndPoint.X), context.ToPixels(segment.EndPoint.Y));
            else
            {
                var properties = this.GetCornerProperties(cornerSegment, fromPoint, context);
                path.ArcTo(properties.RectF, properties.StartAngle, properties.SweepAngle);
            }
        }

        private (RectF RectF, float StartAngle, float SweepAngle) GetCornerProperties(CornerSegment segment, Xamarin.Forms.Point fromPoint, Context context)
        {
            var fromX = context.ToPixels(fromPoint.X);
            var fromY = context.ToPixels(fromPoint.Y);
            var toX = context.ToPixels(segment.EndPoint.X);
            var toY = context.ToPixels(segment.EndPoint.Y);

            float left = fromX, top = fromY;
            float startAngle = 0f, sweepAngle = 0f;

            float rx = Math.Abs(toX - fromX);
            float ry = Math.Abs(toY - fromY);

            if (fromX < toX && fromY < toY)
            {
                if (segment.SweepDirection == SweepDirection.Clockwise)
                {
                    left -= rx;
                    startAngle = -90;
                    sweepAngle = 90;
                }
                else
                {
                    top -= ry;
                    startAngle = -180;
                    sweepAngle = -90;
                }
            }
            else if (fromX < toX && fromY > toY)
            {
                if (segment.SweepDirection == SweepDirection.Clockwise)
                {
                    top -= ry;
                    startAngle = -180;
                    sweepAngle = 90;
                }
                else
                {
                    left -= rx;
                    top -= ry * 2f;
                    startAngle = 90;
                    sweepAngle = -90;
                }
            }
            else if (fromX > toX && fromY > toY)
            {
                if (segment.SweepDirection == SweepDirection.Clockwise)
                {
                    left -= rx;
                    top -= ry * 2f;
                    startAngle = 90;
                    sweepAngle = 90;
                }
                else
                {
                    left -= rx;
                    top -= ry * 2f;
                    startAngle = 0;
                    sweepAngle = -90;
                }
            }
            else if (fromX > toX && fromY < toY)
            {
                if (segment.SweepDirection == SweepDirection.Clockwise)
                {
                    left -= rx * 2f;
                    top -= ry;
                    startAngle = 0;
                    sweepAngle = 90;
                }
                else
                {
                    left -= rx;
                    startAngle = -90;
                    sweepAngle = -90;
                }
            }

            return (new RectF(left, top, left + (rx * 2f), top + (ry * 2f)), startAngle, sweepAngle);
        }
    }
}