using Oxard.XControls.Shapes;
using System.Collections.Generic;
using Xamarin.Forms;

namespace Oxard.XControls.Graphics
{
    public class Geometry
    {
        private readonly List<GeometrySegment> segments = new List<GeometrySegment>();
        private Point startPoint;

        public Geometry StartAt(Point position)
        {
            this.startPoint = position;
            return this;
        }

        public Geometry StartAt(double x, double y)
        {
            return this.StartAt(new Point(x, y));
        }

        public Geometry LineTo(Point toPoint)
        {
            this.segments.Add(new LineSegment(toPoint));
            return this;
        }

        public Geometry LineTo(double toX, double toY)
        {
            return this.LineTo(new Point(toX, toY));
        }

        public Geometry CornerTo(Point toPoint, SweepDirection sweepDirection)
        {
            this.segments.Add(new CornerSegment(toPoint, sweepDirection));
            return this;
        }

        public Geometry CornerTo(double toX, double toY, SweepDirection sweepDirection)
        {
            this.CornerTo(new Point(toX, toY), sweepDirection);
            return this;
        }

        public GeometryReader GetReader()
        {
            return new GeometryReader(this.startPoint, this.segments);
        }

        public void ClosePath()
        {
            this.LineTo(this.startPoint);
        }
    }
}
