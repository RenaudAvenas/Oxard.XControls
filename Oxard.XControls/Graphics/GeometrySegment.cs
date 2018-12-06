using Xamarin.Forms;

namespace Oxard.XControls.Graphics
{
    public abstract class GeometrySegment
    {
        public GeometrySegment(Point endPoint)
        {
            this.EndPoint = endPoint;
        }

        public Point EndPoint { get; }
    }
}
