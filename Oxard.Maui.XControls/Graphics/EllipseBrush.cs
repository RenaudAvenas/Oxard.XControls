using Microsoft.Maui.Controls.Shapes;
using Geometry = Microsoft.Maui.Controls.Shapes.Geometry;

namespace Oxard.Maui.XControls.Graphics;

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
    /// To be added.
    /// </summary>
    /// <value>
    /// To be added.
    /// </value>
    /// <remarks>
    /// To be added.
    /// </remarks>
    public override bool IsEmpty => false;

    /// <summary>
    /// Called when instance size changed (Width and Height).
    /// </summary>
    /// <param name="width">New width</param>
    /// <param name="height">New height</param>
    protected override void OnSizeAllocated(double width, double height)
    {
        this.CalculateGeometry();
    }

    /// <summary>
    /// Called when <see cref="DrawingBrush.StrokeThicknessProperty" /> changed for this instance of <see cref="DrawingBrush" />.
    /// </summary>
    protected override void OnStrokeThicknessChanged()
    {
        base.OnStrokeThicknessChanged();
        this.CalculateGeometry();

    }

    /// <summary>
    /// Creates a new <see cref="DrawingBrush"/> that is a copy of the current instance.
    /// Just clone custom properties of inherited classes. The clone method of DrawingBrush already copies its own properties.
    /// </summary>
    /// <returns>A new <see cref="DrawingBrush"/> that is a copy of this instance.</returns>
    protected override DrawingBrush CloneDrawingBrush() => new EllipseBrush();

    private void CalculateGeometry()
    {
        this.actualGeometry = new EllipseGeometry { Center = new Point(this.Width / 2d, this.Height / 2d), RadiusX = this.Width / 2d, RadiusY = this.Height / 2d };
        this.InvalidateGeometry();
    }
}
