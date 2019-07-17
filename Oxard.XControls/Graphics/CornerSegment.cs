using Oxard.XControls.Shapes;
using Xamarin.Forms;

namespace Oxard.XControls.Graphics
{
    /// <summary>
    /// Represent a 90° corner segment
    /// </summary>
    public class CornerSegment : GeometrySegment
    {
        /// <summary>
        /// Default contructor
        /// </summary>
        /// <param name="endPoint">End point of corner</param>
        /// <param name="sweepDirection">Sweep direction to draw the corner</param>
        public CornerSegment(Point endPoint, SweepDirection sweepDirection) : base(endPoint)
        {
            this.SweepDirection = sweepDirection;
        }

        /// <summary>
        /// Get or set the sweep direction to draw the corner
        /// </summary>
        public SweepDirection SweepDirection { get; }
    }
}
