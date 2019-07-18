using Oxard.XControls.Graphics;

namespace Oxard.XControls.Shapes
{
    /// <summary>
    /// Create a shape that represent an ellipse
    /// </summary>
    public class Ellipse : Shape
    {
        private Geometry actualGeometry;

        /// <summary>
        /// Get the geometry of the shape
        /// </summary>
        public override Geometry Geometry => this.actualGeometry;

        /// <summary>
        /// Called when instance size changed (Width and Height).
        /// </summary>
        /// <param name="width">New width</param>
        /// <param name="height">New height</param>
        protected override void OnSizeAllocated(double width, double height)
        {
            this.CalculateGeometry();
        }

        private void CalculateGeometry()
        {
            this.actualGeometry = GeometryHelper.GetEllipse(this.Width, this.Height, this.StrokeThickness);
            this.InvalidateGeometry();
        }
    }
}
