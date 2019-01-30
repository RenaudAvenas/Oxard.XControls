using Oxard.XControls.Graphics;

namespace Oxard.XControls.Shapes
{
    public class Ellipse : Shape
    {
        private Geometry actualGeometry;

        public override Geometry Geometry => this.actualGeometry;

        protected override void OnSizeAllocated(double width, double height)
        {
            this.CalculateGeometry();
        }

        private void CalculateGeometry()
        {
            var geometry = new Geometry(this.Width, this.Height, this.StrokeThickness);

            var radiusY = this.Height / 2d;
            var radiusX = this.Width / 2d;

            geometry
                 .StartAt(0, radiusY)
                 .CornerTo(radiusX, 0, SweepDirection.Clockwise)
                 .CornerTo(this.Width, radiusY, SweepDirection.Clockwise)
                 .CornerTo(radiusX, this.Height, SweepDirection.Clockwise)
                 .CornerTo(0, radiusY, SweepDirection.Clockwise);

            this.actualGeometry = geometry;
            this.InvalidateGeometry();
        }
    }
}
