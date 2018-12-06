namespace Oxard.XControls.Shapes
{
    public class CornerRadius
    {
        public CornerRadius()
        {
        }

        public CornerRadius(double radiusX, double radiusY)
        {
            this.RadiusX = radiusX;
            this.RadiusY = radiusY;
        }

        public double RadiusX { get; set; }

        public double RadiusY { get; set; }
    }
}
