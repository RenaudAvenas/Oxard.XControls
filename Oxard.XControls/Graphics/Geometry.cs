using Oxard.XControls.Shapes;
using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace Oxard.XControls.Graphics
{
    public class Geometry
    {
        private readonly List<GeometrySegment> segments = new List<GeometrySegment>();
        private readonly ProjectionDefinition projectionDefinition;
        private Point startPoint;

        public Geometry()
        {
        }

        public Geometry(Size size, double strokeThickness)
        {
            this.projectionDefinition = new ProjectionDefinition(size.Width, size.Height, strokeThickness);
            this.startPoint = this.projectionDefinition.ProjectPoint(0d, 0d);
        }

        public Geometry(double width, double height, double strokeThickness)
        {
            this.projectionDefinition = new ProjectionDefinition(width, height, strokeThickness);
            this.startPoint = this.projectionDefinition.ProjectPoint(0d, 0d);
        }

        public Geometry StartAt(Point position)
        {
            this.startPoint = this.ProjectPoint(position);
            return this;
        }

        public Geometry StartAt(double x, double y)
        {
            this.startPoint = this.ProjectPoint(x, y);
            return this;
        }

        public Geometry LineTo(Point toPoint)
        {
            this.segments.Add(new LineSegment(this.ProjectPoint(toPoint)));
            return this;
        }

        public Geometry LineTo(double toX, double toY)
        {
            this.segments.Add(new LineSegment(this.ProjectPoint(toX, toY)));
            return this;
        }

        public Geometry CornerTo(Point toPoint, SweepDirection sweepDirection)
        {
            this.segments.Add(new CornerSegment(this.ProjectPoint(toPoint), sweepDirection));
            return this;
        }

        public Geometry CornerTo(double toX, double toY, SweepDirection sweepDirection)
        {
            this.segments.Add(new CornerSegment(this.ProjectPoint(toX, toY), sweepDirection));
            return this;
        }

        /// <summary>
        /// Add a segment to the geometry.
        /// </summary>
        /// <remarks>If your stroke are cropped be careful to use ProjectPoint method to take care of stroke thickness</remarks>
        /// <param name="geometrySegment"></param>
        /// <returns>Current geometry</returns>
        public Geometry AddSegment(GeometrySegment geometrySegment)
        {
            this.segments.Add(geometrySegment);
            return this;
        }

        public static Point ProjectPoint(Size size, double strokeThickness, Point toTransform) => ProjectPoint(size.Width, size.Height, strokeThickness, toTransform);

        public static Point ProjectPoint(double width, double height, double strokeThickness, Point toTransform)
        {
            return new ProjectionDefinition(width, height, strokeThickness).ProjectPoint(toTransform);
        }

        /// <summary>
        /// Adapt point calculation to size and stroke thickness of the geometry
        /// </summary>
        /// <remarks>Use good constructor by passing size and stroke thickness to have an effect otherwise the source point will be returned</remarks>
        /// <param name="point">Point to project</param>
        /// <returns>Projected point</returns>
        public Point ProjectPoint(Point point)
        {
            if (this.projectionDefinition == null)
                return point;

            return this.projectionDefinition.ProjectPoint(point);
        }

        /// <summary>
        /// Adapt point calculation to size and stroke thickness of the geometry
        /// </summary>
        /// <remarks>Use good constructor by passing size and stroke thickness to have an effect otherwise the source point will be returned</remarks>
        /// <param name="x">X coordinate of point to transform</param>
        /// <param name="y">Y coordinate of point to transform</param>
        /// <returns>Projected point</returns>
        public Point ProjectPoint(double x, double y)
        {
            if (this.projectionDefinition == null)
                return new Point(x, y);

            return this.projectionDefinition.ProjectPoint(x, y);
        }

        public GeometryReader GetReader()
        {
            return new GeometryReader(this.startPoint, this.segments);
        }

        public void ClosePath()
        {
            this.segments.Add(new LineSegment(this.startPoint));
        }

        private class ProjectionDefinition
        {
            private readonly double halfStrokeThickness;
            private readonly double widthRatio;
            private readonly double heightRatio;

            public ProjectionDefinition(double width, double height, double strokeThickness)
            {
                this.halfStrokeThickness = strokeThickness / 2d;

                var realWidth = width - strokeThickness;
                var realHeight = height - strokeThickness;

                // Adding 1 to width and height manage imprecision of width and height ratio due to rounded measures
                this.widthRatio = realWidth / (width + 1);
                this.heightRatio = realHeight / (height + 1);
            }

            public Point ProjectPoint(double x, double y)
            {
                // Adding 0.1 to x and y to manage imprecision of width and height ratio due to rounded measures
                return new Point(Math.Round(x * this.widthRatio + 0.1 + this.halfStrokeThickness), Math.Round(y * this.heightRatio + 0.1 + this.halfStrokeThickness));
            }

            public Point ProjectPoint(Point toTransform) => this.ProjectPoint(toTransform.X, toTransform.Y);
        }
    }
}
