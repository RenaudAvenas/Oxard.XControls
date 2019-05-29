using Oxard.XControls.Extensions;
using Oxard.XControls.Graphics;
using Oxard.XControls.Helpers;
using Oxard.XControls.Shapes;
using Xamarin.Forms;

namespace Oxard.TestApp
{
    public class RandomShape : Shape
    {
        private Geometry actualGeometry;

        public override Geometry Geometry => this.actualGeometry;

        protected override SizeRequest OnMeasure(double widthConstraint, double heightConstraint) => this.GetStandardMeasure(widthConstraint, heightConstraint);

        protected override void OnSizeAllocated(double width, double height)
        {
            base.OnSizeAllocated(width, height);

            var xUnit = width / 5d;
            var yUnit = height / 3d;

            this.actualGeometry = new Geometry(width, height, this.StrokeThickness)
                .StartAt(0 * xUnit, 1 * yUnit)
                .CornerTo(1 * xUnit, 0 * yUnit, SweepDirection.Clockwise)
                .LineTo(3 * xUnit, 0 * yUnit)
                .CornerTo(5 * xUnit, 1 * yUnit, SweepDirection.Clockwise)
                .CornerTo(4 * xUnit, 3 * yUnit, SweepDirection.Counterclockwise)
                .LineTo(2 * xUnit, 3 * yUnit)
                .CornerTo(0 * xUnit, 1 * yUnit, SweepDirection.Clockwise);

            this.InvalidateGeometry();
        }
    }
}
