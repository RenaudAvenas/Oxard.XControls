using System.Collections.Generic;
using Xamarin.Forms;

namespace Oxard.XControls.Graphics
{
    public class GeometryReader
    {
        private readonly List<GeometrySegment> segments;
        private int actualIndex = -1;

        internal GeometryReader(Point startPoint, IEnumerable<GeometrySegment> segments)
        {
            this.segments = new List<GeometrySegment>(segments);
            this.FromPoint = startPoint;
        }

        public Point FromPoint { get; private set; }

        public GeometrySegment GetNext()
        {
            if (this.actualIndex >= 0)
                this.FromPoint = this.segments[this.actualIndex].EndPoint;

            this.actualIndex++;
            if (this.actualIndex >= this.segments.Count)
                return null;

            return this.segments[this.actualIndex];
        }
    }
}
