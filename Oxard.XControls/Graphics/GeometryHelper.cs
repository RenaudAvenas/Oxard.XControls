using Xamarin.Forms;
using Xamarin.Forms.Shapes;
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
            var geometry = new PathGeometry();

            var pathFigure = new PathFigure();
            pathFigure.IsClosed = true;
            geometry.Figures.Add(pathFigure);

            var halfStroke = strokeThickness / 2d;

            if (topLeft != null && !topLeft.IsEmpty)
            {
                pathFigure.StartPoint = new Point(halfStroke, topLeft.RadiusY + halfStroke);
                pathFigure.Segments.Add(new ArcSegment { Point = new Point(topLeft.RadiusX + halfStroke, halfStroke), RotationAngle = 90, SweepDirection = SweepDirection.Clockwise, Size = new Size(topLeft.RadiusX, topLeft.RadiusY), IsLargeArc = false });
            }
            else
                pathFigure.StartPoint = new Point(halfStroke, halfStroke);

            if (topRight != null && !topRight.IsEmpty)
            {
                pathFigure.Segments.Add(new LineSegment { Point = new Point(width - topRight.RadiusX - halfStroke, halfStroke) });
                pathFigure.Segments.Add(new ArcSegment { Point = new Point(width - halfStroke, topRight.RadiusY + halfStroke), RotationAngle = 90, SweepDirection = SweepDirection.Clockwise, Size = new Size(topRight.RadiusX, topRight.RadiusY) });
            }
            else
                pathFigure.Segments.Add(new LineSegment { Point = new Point(width - halfStroke, halfStroke) });

            if (bottomRight != null && !bottomRight.IsEmpty)
            {
                pathFigure.Segments.Add(new LineSegment { Point = new Point(width - halfStroke, height - bottomRight.RadiusY - halfStroke) });
                pathFigure.Segments.Add(new ArcSegment { Point = new Point(width - bottomRight.RadiusX - halfStroke, height - halfStroke), RotationAngle = 90, SweepDirection = SweepDirection.Clockwise, Size = new Size(bottomRight.RadiusX, bottomRight.RadiusY) });
            }
            else
                pathFigure.Segments.Add(new LineSegment { Point = new Point(width - halfStroke, height - halfStroke) });

            if (bottomLeft != null && !bottomLeft.IsEmpty)
            {
                pathFigure.Segments.Add(new LineSegment { Point = new Point(bottomLeft.RadiusX + halfStroke, height - halfStroke) });
                pathFigure.Segments.Add(new ArcSegment { Point = new Point(halfStroke, height - bottomLeft.RadiusY - halfStroke), RotationAngle = 90, SweepDirection = SweepDirection.Clockwise, Size = new Size(bottomLeft.RadiusX, bottomLeft.RadiusY) });
            }
            else
                pathFigure.Segments.Add(new LineSegment { Point = new Point(halfStroke, height - halfStroke) });

            return geometry;
        }
    }
}
