using Oxard.XControls.Shapes;
using Xamarin.Forms;
using CornerRadius = Oxard.XControls.Shapes.CornerRadius;

namespace Oxard.XControls.Graphics
{
    /// <summary>
    /// Helpers for geometries
    /// </summary>
    public static class GeometryHelper
    {
        /// <summary>
        /// Return a rectangle geometry
        /// </summary>
        /// <param name="width">Width of the rectangle</param>
        /// <param name="height">Height of the rectangle</param>
        /// <param name="strokeThickness">Stroke thickness of the rectangle</param>
        /// <param name="topLeft">Top left corner definition of the rectangle</param>
        /// <param name="topRight">Top right corner definition of the rectangle</param>
        /// <param name="bottomRight">Bottom right corner definition of the rectangle</param>
        /// <param name="bottomLeft">Bottom left corner definition of the rectangle</param>
        /// <returns>Rectangle geometry</returns>
        public static Geometry GetRectangle(double width, double height, double strokeThickness, CornerRadius topLeft, CornerRadius topRight, CornerRadius bottomRight, CornerRadius bottomLeft)
        {
            var geometry = new Geometry(width, height, strokeThickness);

            if (topLeft != null)
            {
                geometry
                    .StartAt(new Point(0, topLeft.RadiusY))
                    .CornerTo(new Point(topLeft.RadiusX, 0), SweepDirection.Clockwise);
            }
            else
                geometry.StartAt(0, 0);

            if (topRight != null)
            {
                geometry
                    .LineTo(new Point(width - topRight.RadiusX, 0))
                    .CornerTo(new Point(width, topRight.RadiusY), SweepDirection.Clockwise);
            }
            else
                geometry.LineTo(width, 0);

            if (bottomRight != null)
            {
                geometry
                    .LineTo(new Point(width, height - bottomRight.RadiusY))
                    .CornerTo(new Point(width - bottomRight.RadiusX, height), SweepDirection.Clockwise);
            }
            else
                geometry.LineTo(width, height);

            if (bottomLeft != null)
            {
                geometry
                    .LineTo(new Point(bottomLeft.RadiusX, height))
                    .CornerTo(new Point(0, height - bottomLeft.RadiusY), SweepDirection.Clockwise);
            }
            else
                geometry.LineTo(0, height);

            return geometry.ClosePath();
        }

        /// <summary>
        /// Return an ellipse geometry
        /// </summary>
        /// <param name="width">Width of the ellipse</param>
        /// <param name="height">Height of the ellipse</param>
        /// <param name="strokeThickness">Stroke thickness of the ellipse</param>
        /// <returns>Ellipse geometry</returns>
        public static Geometry GetEllipse(double width, double height, double strokeThickness)
        {
            var geometry = new Geometry(width, height, strokeThickness);

            var radiusY = height / 2d;
            var radiusX = width / 2d;

            return geometry
                 .StartAt(0, radiusY)
                 .CornerTo(radiusX, 0, SweepDirection.Clockwise)
                 .CornerTo(width, radiusY, SweepDirection.Clockwise)
                 .CornerTo(radiusX, height, SweepDirection.Clockwise)
                 .CornerTo(0, radiusY, SweepDirection.Clockwise)
                 .ClosePath();
        }
    }
}
