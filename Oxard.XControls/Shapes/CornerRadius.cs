namespace Oxard.XControls.Shapes
{
    /// <summary>
    /// Define a corner radius with X and Y radius
    /// </summary>
    public class CornerRadius
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        public CornerRadius()
        {
        }

        /// <summary>
        /// Define the corner radius with X and Y radius
        /// </summary>
        /// <param name="radiusX">X radius</param>
        /// <param name="radiusY">Y radius</param>
        public CornerRadius(double radiusX, double radiusY)
        {
            this.RadiusX = radiusX;
            this.RadiusY = radiusY;
        }

        /// <summary>
        /// Get a X=0, Y=0 corner radius
        /// </summary>
        public static CornerRadius Zero { get; } = new CornerRadius();

        /// <summary>
        /// Get or set the X radius
        /// </summary>
        public double RadiusX { get; set; }

        /// <summary>
        /// Get or set the Y radius
        /// </summary>
        public double RadiusY { get; set; }
    }
}
