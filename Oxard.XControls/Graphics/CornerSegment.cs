using Oxard.XControls.Shapes;
using Xamarin.Forms;

namespace Oxard.XControls.Graphics
{
    public class CornerSegment : GeometrySegment
    {
        public CornerSegment(Point endPoint, SweepDirection sweepDirection) : base(endPoint)
        {
            this.SweepDirection = sweepDirection;
        }
        
        public SweepDirection SweepDirection { get; }
    }
}
