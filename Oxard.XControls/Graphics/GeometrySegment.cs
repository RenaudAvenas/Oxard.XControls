using Xamarin.Forms;

namespace Oxard.XControls.Graphics
{
    /// <summary>
    /// Represent a segment of a geometry
    /// </summary>
    public abstract class GeometrySegment
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="endPoint">End point of the segment</param>
        public GeometrySegment(Point endPoint)
        {
            this.EndPoint = endPoint;
        }

        /// <summary>
        /// Get the end point of the segment
        /// </summary>
        public Point EndPoint { get; }
    }
}
