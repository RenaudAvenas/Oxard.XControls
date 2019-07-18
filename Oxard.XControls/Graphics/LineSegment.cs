using Xamarin.Forms;

namespace Oxard.XControls.Graphics
{
    /// <summary>
    /// Create a line segment in a geometry
    /// </summary>
    public class LineSegment : GeometrySegment
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="endPoint">End point of the line</param>
        public LineSegment(Point endPoint) : base(endPoint)
        {
        }
    }
}
