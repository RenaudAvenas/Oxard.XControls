using Oxard.XControls.Extensions;
using Oxard.XControls.Shapes;
using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace Oxard.XControls.Graphics
{
    /// <summary>
    /// Class that defines a geometry
    /// </summary>
    public class Geometry
    {
        private readonly List<GeometrySegment> segments = new List<GeometrySegment>();
        private readonly ProjectionDefinition projectionDefinition;
        private Point startPoint;

        /// <summary>
        /// Default constructor
        /// </summary>
        public Geometry()
        {
        }

        /// <summary>
        /// Constructor that defines size and stroke thickness of the geometry
        /// </summary>
        /// <param name="size">Geometry bounds size</param>
        /// <param name="strokeThickness">Wanted stroke thickness for this geometry</param>
        public Geometry(Size size, double strokeThickness)
        {
            this.projectionDefinition = new ProjectionDefinition(size.Width, size.Height, strokeThickness);
            this.startPoint = this.projectionDefinition.ProjectPoint(0d, 0d);
        }

        /// <summary>
        /// Constructor that defines size and stroke thickness of the geometry
        /// </summary>
        /// <param name="width">Geometry bound width</param>
        /// <param name="height">Geometry bound height</param>
        /// <param name="strokeThickness">Wanted stroke thickness for this geometry</param>
        public Geometry(double width, double height, double strokeThickness)
        {
            this.projectionDefinition = new ProjectionDefinition(width, height, strokeThickness);
            this.startPoint = this.projectionDefinition.ProjectPoint(0d, 0d);
        }

        /// <summary>
        /// Get or set a value that indicate if the geometry path is closed or not
        /// </summary>
        public bool IsClosed { get; set; }

        /// <summary>
        /// Define start position of the geometry
        /// </summary>
        /// <param name="position">Start position</param>
        /// <returns>The current geometry</returns>
        public Geometry StartAt(Point position)
        {
            this.startPoint = this.ProjectPoint(position);
            return this;
        }

        /// <summary>
        /// Define start position of the geometry
        /// </summary>
        /// <param name="x">X start position</param>
        /// <param name="y">Y start position</param>
        /// <returns>The current geometry</returns>
        public Geometry StartAt(double x, double y)
        {
            this.startPoint = this.ProjectPoint(x, y);
            return this;
        }

        /// <summary>
        /// Define a line from current position to <paramref name="toPoint"/> position
        /// </summary>
        /// <param name="toPoint">End line position</param>
        /// <returns>The current geometry</returns>
        public Geometry LineTo(Point toPoint)
        {
            this.segments.Add(new LineSegment(this.ProjectPoint(toPoint)));
            return this;
        }

        /// <summary>
        /// Define a line from current position to end position
        /// </summary>
        /// <param name="toX">X end line position</param>
        /// <param name="toY">Y end line position</param>
        /// <returns>The current geometry</returns>
        public Geometry LineTo(double toX, double toY)
        {
            this.segments.Add(new LineSegment(this.ProjectPoint(toX, toY)));
            return this;
        }

        /// <summary>
        /// Define a corner from current position to <paramref name="toPoint"/> position
        /// </summary>
        /// <param name="toPoint">End line position</param>
        /// <param name="sweepDirection">Sweep direction of the corner</param>
        /// <returns>The current geometry</returns>
        public Geometry CornerTo(Point toPoint, SweepDirection sweepDirection)
        {
            this.segments.Add(new CornerSegment(this.ProjectPoint(toPoint), sweepDirection));
            return this;
        }

        /// <summary>
        /// Define a corner from current position to end position
        /// </summary>
        /// <param name="toX">X end corner position</param>
        /// <param name="toY">Y end corner position</param>
        /// <param name="sweepDirection">Sweep direction of the corner</param>
        /// <returns>The current geometry</returns>
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

        /// <summary>
        /// Adapt point calculation to <paramref name="size"/> and <paramref name="strokeThickness"/> of the geometry
        /// </summary>
        /// <param name="size">Size of the geometry</param>
        /// <param name="strokeThickness">Stroke thickness of the geometry</param>
        /// <param name="toTransform">Point to project</param>
        /// <returns>ProjectedPoint</returns>
        public static Point ProjectPoint(Size size, double strokeThickness, Point toTransform) => ProjectPoint(size.Width, size.Height, strokeThickness, toTransform);

        /// <summary>
        /// Adapt point calculation to size and <paramref name="strokeThickness"/> of the geometry
        /// </summary>
        /// <param name="width">Width of the geometry</param>
        /// <param name="height">Height of the geometry</param>
        /// <param name="strokeThickness">Stroke thickness of the geometry</param>
        /// <param name="toTransform">Point to project</param>
        /// <returns>ProjectedPoint</returns>
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

        /// <summary>
        /// Get the <see cref="GeometryReader"/> for current geometry
        /// </summary>
        /// <returns></returns>
        public GeometryReader GetReader()
        {
            return new GeometryReader(this.startPoint, this.segments);
        }

        /// <summary>
        /// Return to the start point with a line and set the <see cref="IsClosed"/> property to true
        /// </summary>
        /// <returns>The current geometry</returns>
        public Geometry ClosePath()
        {
            this.segments.Add(new LineSegment(this.startPoint));
            this.IsClosed = true;
            return this;
        }

        private class ProjectionDefinition
        {
            private readonly double halfStrokeThickness;
            private readonly double ceilingHalfStrokeThickness;
            private readonly double widthRatio;
            private readonly double heightRatio;
            private readonly double width;
            private readonly double height;

            public ProjectionDefinition(double width, double height, double strokeThickness)
            {
                this.width = width;
                this.height = height;

                if (strokeThickness.DoubleIsEquals(0))
                {
                    this.halfStrokeThickness = 0;
                    this.widthRatio = 1;
                    this.heightRatio = 1;
                }
                else
                {
                    this.halfStrokeThickness = strokeThickness / 2d;
                    this.ceilingHalfStrokeThickness = Math.Ceiling(this.halfStrokeThickness);

                    var realWidth = width - strokeThickness;
                    var realHeight = height - strokeThickness;

                    this.widthRatio = realWidth / width;
                    this.heightRatio = realHeight / height;
                }
            }

            public Point ProjectPoint(double x, double y)
            {
                var projectedX = Math.Round(x * this.widthRatio + this.halfStrokeThickness);
                var projectedY = Math.Round(y * this.heightRatio + this.halfStrokeThickness);

                if (projectedX < this.ceilingHalfStrokeThickness)
                    projectedX = this.ceilingHalfStrokeThickness;
                else if (projectedX > this.width - this.ceilingHalfStrokeThickness)
                    projectedX = this.width - this.ceilingHalfStrokeThickness;

                if (projectedY < this.ceilingHalfStrokeThickness)
                    projectedY = this.ceilingHalfStrokeThickness;
                else if(projectedY > this.height - this.ceilingHalfStrokeThickness)
                    projectedY = this.height - this.ceilingHalfStrokeThickness;


                return new Point(projectedX, projectedY);
            }

            public Point ProjectPoint(Point toTransform) => this.ProjectPoint(toTransform.X, toTransform.Y);
        }
    }
}
