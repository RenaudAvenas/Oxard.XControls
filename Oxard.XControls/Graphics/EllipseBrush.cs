namespace Oxard.XControls.Graphics
{
    /// <summary>
    /// Draw an ellipse as a brush
    /// </summary>
    public class EllipseBrush : DrawingBrush
    {
        private Geometry actualGeometry;

        /// <summary>
        /// Get the geometry of the drawable
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
