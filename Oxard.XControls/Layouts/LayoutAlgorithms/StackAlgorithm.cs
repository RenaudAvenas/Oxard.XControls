using System;
using System.Linq;
using Xamarin.Forms;

namespace Oxard.XControls.Layouts.LayoutAlgorithms
{
    /// <summary>
    /// Algorithm that stack horizontally or vertically all children
    /// </summary>
    public class StackAlgorithm : LayoutAlgorithm
    {
        private StackOrientation orientation = StackOrientation.Vertical;
        private double spacing;

        /// <summary>
        /// Get or set the orientation of stack
        /// </summary>
        public StackOrientation Orientation
        {
            get => this.orientation;
            set
            {
                if (this.orientation == value)
                    return;

                this.orientation = value;
                this.Invalidate();
            }
        }

        /// <summary>
        /// Get or set the space between each children.
        /// </summary>
        public double Spacing
        {
            get => spacing;
            set
            {
                if (this.spacing == value)
                    return;

                this.spacing = value;
                this.Invalidate();
            }
        }

        /// <summary>
        /// Method called when a measurement is asked.
        /// </summary>
        /// <param name="widthConstraint">Width constraint</param>
        /// <param name="heightConstraint">Height constraint</param>
        /// <returns>Requested size</returns>
        protected override SizeRequest OnMeasure(double widthConstraint, double heightConstraint)
        {
            if (this.Orientation == StackOrientation.Horizontal)
                return this.OnMeasureHorizontal(heightConstraint);
            else
                return this.OnMeasureVertical(widthConstraint);
        }

        /// <summary>
        /// Layout the children of the current layout
        /// </summary>
        /// <param name="x">X delay</param>
        /// <param name="y">Y delay</param>
        /// <param name="width">Width constraint to layout</param>
        /// <param name="height">Height constraint to layout</param>
        protected override void OnLayoutChildren(double x, double y, double width, double height)
        {
            if (this.Orientation == StackOrientation.Horizontal)
                this.OnLayoutChildrenHorizontal(x, y, height);
            else
                this.OnLayoutChildrenVertical(x, y, width);
        }

        private SizeRequest OnMeasureVertical(double widthConstraint)
        {
            var totalHeight = 0d;
            var width = 0d;
            var calculateWidth = double.IsPositiveInfinity(widthConstraint);

            foreach (var child in this.ParentLayout.Children.Where(c => c.IsVisible))
            {
                var sizeRequest = child.Measure(widthConstraint, double.PositiveInfinity);
                if (calculateWidth)
                    width = Math.Max(width, sizeRequest.Request.Width);

                totalHeight = sizeRequest.Request.Height + this.Spacing;
            }

            if (totalHeight > 0)
                totalHeight -= this.Spacing;

            return new SizeRequest(new Size(calculateWidth ? width : widthConstraint, totalHeight));
        }

        private SizeRequest OnMeasureHorizontal(double heightConstraint)
        {
            var totalWidth = 0d;
            var height = 0d;
            var calculateHeight = double.IsPositiveInfinity(heightConstraint);

            foreach (var child in this.ParentLayout.Children.Where(c => c.IsVisible))
            {
                var sizeRequest = child.Measure(double.PositiveInfinity, heightConstraint);
                if (calculateHeight)
                    height = Math.Max(height, sizeRequest.Request.Height);

                totalWidth = sizeRequest.Request.Width + this.Spacing;
            }

            if (totalWidth > 0)
                totalWidth -= this.Spacing;

            return new SizeRequest(new Size(totalWidth, calculateHeight ? height : heightConstraint));
        }

        private void OnLayoutChildrenHorizontal(double x, double y, double height)
        {
            var currentX = x;
            foreach (var child in this.ParentLayout.Children.Where(c => c.IsVisible))
            {
                var childMeasure = child.Measure(double.PositiveInfinity, height, MeasureFlags.IncludeMargins);

                var alignY = y;
                var childHeight = childMeasure.Request.Height;
                switch (child.VerticalOptions.Alignment)
                {
                    case LayoutAlignment.Start:
                        break;
                    case LayoutAlignment.Center:
                        alignY = height / 2d - childMeasure.Request.Height / 2d;
                        break;
                    case LayoutAlignment.End:
                        alignY = height - childMeasure.Request.Height;
                        break;
                    default:
                        childHeight = height;
                        break;
                }

                child.Layout(new Rectangle(currentX, alignY, childMeasure.Request.Width, childHeight));
                currentX += childMeasure.Request.Width + this.Spacing;
            }
        }

        private void OnLayoutChildrenVertical(double x, double y, double width)
        {
            var currentY = y;
            foreach (var child in this.ParentLayout.Children.Where(c => c.IsVisible))
            {
                var childMeasure = child.Measure(width, double.PositiveInfinity, MeasureFlags.IncludeMargins);

                var alignX = x;
                var childWidth = childMeasure.Request.Width;
                switch (child.HorizontalOptions.Alignment)
                {
                    case LayoutAlignment.Start:
                        break;
                    case LayoutAlignment.Center:
                        alignX = width / 2d - childMeasure.Request.Width / 2d;
                        break;
                    case LayoutAlignment.End:
                        alignX = width - childMeasure.Request.Width;
                        break;
                    default:
                        childWidth = width;
                        break;
                }

                child.Layout(new Rectangle(alignX, currentY, childWidth, childMeasure.Request.Height));
                currentY += childMeasure.Request.Height + this.Spacing;
            }
        }
    }
}
