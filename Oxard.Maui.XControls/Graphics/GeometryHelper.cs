using Microsoft.Maui.Controls.Shapes;
using Oxard.Maui.XControls.Shapes;
using CornerRadius = Oxard.Maui.XControls.Shapes.CornerRadius;
using Geometry = Microsoft.Maui.Controls.Shapes.Geometry;

namespace Oxard.Maui.XControls.Graphics;

/// <summary>
/// Helpers for geometries
/// </summary>
public static class GeometryHelper
{
    /// <summary>
    /// Returns a rectangle geometry
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

    /// <summary>
    /// Returns an oriented line geometry
    /// </summary>
    /// <param name="thickness">The thickness of the line</param>
    /// <param name="orientation">The orientation of the line</param>
    /// <param name="lineDimension">The size of the line</param>
    /// <param name="containerSize">The size where the line is drawn. If the orientation is vertical, containerSize must be the available width to draw the line otherwise the available height.</param>
    /// <returns>Oriented line geometry</returns>
    public static Geometry GetOrientedLine(double thickness, LineOrientation orientation, double lineDimension, double containerSize)
    {
        var geometry = new PathGeometry();

        var pathFigure = new PathFigure();
        pathFigure.IsClosed = false;
        geometry.Figures.Add(pathFigure);

        var halfStroke = thickness / 2d;
        var middleContainer = containerSize / 2d;

        if (orientation == LineOrientation.Vertical)
        {
            pathFigure.StartPoint = new Point(middleContainer - halfStroke, 0);
            pathFigure.Segments.Add(new LineSegment { Point = new Point(middleContainer - halfStroke, lineDimension) });
        }
        else
        {
            pathFigure.StartPoint = new Point(0, middleContainer - halfStroke);
            pathFigure.Segments.Add(new LineSegment { Point = new Point(lineDimension, middleContainer - halfStroke) });
        }

        return geometry;
    }
}
