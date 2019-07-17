using System.Collections.Generic;
using Xamarin.Forms;

namespace Oxard.XControls.Graphics
{
    /// <summary>
    /// Reader for geometry
    /// </summary>
    public class GeometryReader
    {
        private readonly List<GeometrySegment> segments;
        private int actualIndex = -1;

        internal GeometryReader(Point startPoint, IEnumerable<GeometrySegment> segments)
        {
            this.segments = new List<GeometrySegment>(segments);
            this.FromPoint = startPoint;
        }

        /// <summary>
        /// Get the current start point
        /// </summary>
        public Point FromPoint { get; private set; }

        /// <summary>
        /// Get the next segment of the geometry
        /// </summary>
        /// <returns>Next segment</returns>
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
