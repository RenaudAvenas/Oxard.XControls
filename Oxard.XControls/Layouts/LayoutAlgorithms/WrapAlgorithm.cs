using Oxard.XControls.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;

namespace Oxard.XControls.Layouts.LayoutAlgorithms
{
    /// <summary>
    /// Algorithm that stack horizontally or vertically all children and wrap them if necessary
    /// </summary>
    /// <seealso cref="Oxard.XControls.Layouts.LayoutAlgorithms.LayoutAlgorithm" />
    public class WrapAlgorithm : LayoutAlgorithm
    {
        /// <summary>
        /// Identifies the Orientation property.
        /// </summary>
        public static readonly BindableProperty OrientationProperty = BindableProperty.Create(nameof(Orientation), typeof(StackOrientation), typeof(WrapAlgorithm), StackOrientation.Horizontal, propertyChanged: OnMeasureLayoutRequested);
        /// <summary>
        /// Identifies the Spacing property.
        /// </summary>
        public static readonly BindableProperty SpacingProperty = BindableProperty.Create(nameof(Spacing), typeof(double), typeof(WrapAlgorithm), default(double), propertyChanged: OnMeasureLayoutRequested);
        /// <summary>
        /// Identifies the WrapSpacing property.
        /// </summary>
        public static readonly BindableProperty WrapSpacingProperty = BindableProperty.Create(nameof(WrapSpacing), typeof(double), typeof(WrapAlgorithm), default(double), propertyChanged: OnMeasureLayoutRequested);

        /// <summary>
        /// Get or set the orientation of the WrapLayout
        /// </summary>
        public StackOrientation Orientation
        {
            get => (StackOrientation)this.GetValue(OrientationProperty);
            set => this.SetValue(OrientationProperty, value);
        }

        /// <summary>
        /// Get or set the space between each child
        /// </summary>
        public double Spacing
        {
            get => (double)this.GetValue(SpacingProperty);
            set => this.SetValue(SpacingProperty, value);
        }

        /// <summary>
        /// Get or set spacing between each wrap of children
        /// </summary>
        public double WrapSpacing
        {
            get => (double)this.GetValue(WrapSpacingProperty);
            set => this.SetValue(WrapSpacingProperty, value);
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
                return this.OnMeasureHorizontal(widthConstraint);
            else
                return this.OnMeasureVertical(heightConstraint);
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
                this.OnLayoutChildrenHorizontal(x, y, width);
            else
                this.OnLayoutChildrenVertical(x, y, height);
        }

        private SizeRequest OnMeasureVertical(double heightConstraint)
        {
            var maxHeight = 0d;
            var columnWidth = 0d;
            var actualX = 0d;
            var actualY = 0d;
            var firstElement = true;

            foreach (var child in this.ParentLayout.Children.Where(c => c.IsVisible))
            {
                var sizeRequest = child.Measure(double.PositiveInfinity, heightConstraint, MeasureFlags.IncludeMargins);
                if (firstElement)
                {
                    // First element is always displayed on first row even if its width is larger than widthConstraint
                    firstElement = false;
                }
                else if (actualY + sizeRequest.Request.Height > heightConstraint)
                {
                    // We need to wrap / change row
                    maxHeight = Math.Max(maxHeight, actualY - this.Spacing);
                    actualY = 0;
                    actualX += columnWidth + this.WrapSpacing;
                    columnWidth = 0;
                }

                actualY += sizeRequest.Request.Height + this.Spacing;
                columnWidth = Math.Max(columnWidth, sizeRequest.Request.Width);
            }

            var width = actualX + columnWidth;
            maxHeight = Math.Max(maxHeight, actualY - this.Spacing);

            return new SizeRequest(new Size(width, Math.Min(maxHeight, heightConstraint.TranslateIfInfinity())));
        }

        private SizeRequest OnMeasureHorizontal(double widthConstraint)
        {
            var maxWidth = 0d;
            var rowHeight = 0d;
            var actualX = 0d;
            var actualY = 0d;
            var firstElement = true;

            foreach (var child in this.ParentLayout.Children.Where(c => c.IsVisible))
            {
                var sizeRequest = child.Measure(widthConstraint, double.PositiveInfinity, MeasureFlags.IncludeMargins);
                if (firstElement)
                {
                    // First element is always displayed on first row even if its width is larger than widthConstraint
                    firstElement = false;
                }
                else if (actualX + sizeRequest.Request.Width > widthConstraint)
                {
                    // We need to wrap / change row
                    maxWidth = Math.Max(maxWidth, actualX - this.Spacing);
                    actualX = 0;
                    actualY += rowHeight + this.WrapSpacing;
                    rowHeight = 0;
                }

                actualX += sizeRequest.Request.Width + this.Spacing;
                rowHeight = Math.Max(rowHeight, sizeRequest.Request.Height);
            }

            var height = actualY + rowHeight;
            maxWidth = Math.Max(maxWidth, actualX - this.Spacing);

            return new SizeRequest(new Size(Math.Min(maxWidth, widthConstraint.TranslateIfInfinity()), height));
        }

        private void OnLayoutChildrenHorizontal(double x, double y, double width)
        {
            var currentX = x;
            var currentY = y;
            var rowHeight = 0d;
            var firstElement = true;
            var childrenOnRow = new Dictionary<View, SizeRequest>();

            foreach (var child in this.ParentLayout.Children.Where(c => c.IsVisible))
            {
                var childMeasure = child.Measure(width, double.PositiveInfinity, MeasureFlags.IncludeMargins);

                if (firstElement)
                    firstElement = false;
                else if (currentX + childMeasure.Request.Width > width)
                {
                    // Wrapping needed, we can display the current row
                    this.LayoutRow(childrenOnRow, currentY, rowHeight);

                    currentX = x;
                    currentY += rowHeight + this.WrapSpacing;
                    rowHeight = 0;
                    childrenOnRow.Clear();
                }

                childrenOnRow[child] = childMeasure;
                currentX += childMeasure.Request.Width + this.Spacing;
                rowHeight = Math.Max(rowHeight, childMeasure.Request.Height);
            }

            if (childrenOnRow.Count > 0)
                this.LayoutRow(childrenOnRow, currentY, rowHeight);
        }

        private void OnLayoutChildrenVertical(double x, double y, double height)
        {
            var currentX = x;
            var currentY = y;
            var columnWidth = 0d;
            var firstElement = true;
            var childrenOnColumn = new Dictionary<View, SizeRequest>();

            foreach (var child in this.ParentLayout.Children.Where(c => c.IsVisible))
            {
                var childMeasure = child.Measure(double.PositiveInfinity, height, MeasureFlags.IncludeMargins);

                if (firstElement)
                    firstElement = false;
                else if (currentY + childMeasure.Request.Height > height)
                {
                    // Wrapping needed, we can display the current row
                    this.LayoutColumn(childrenOnColumn, currentX, columnWidth);

                    currentY = y;
                    currentX += columnWidth + this.WrapSpacing;
                    columnWidth = 0;
                    childrenOnColumn.Clear();
                }

                childrenOnColumn[child] = childMeasure;
                currentY += childMeasure.Request.Height + this.Spacing;
                columnWidth = Math.Max(columnWidth, childMeasure.Request.Width);
            }

            if (childrenOnColumn.Count > 0)
                this.LayoutColumn(childrenOnColumn, currentX, columnWidth);
        }

        private void LayoutRow(Dictionary<View, SizeRequest> childrenOnRow, double rowY, double rowHeight)
        {
            var xOnRow = 0d;
            foreach (var rowKeyValue in childrenOnRow)
            {
                var childMeasure = rowKeyValue.Value;
                var child = rowKeyValue.Key;

                var alignY = rowY;
                var childHeight = childMeasure.Request.Height;
                switch (child.VerticalOptions.Alignment)
                {
                    case LayoutAlignment.Start:
                        break;
                    case LayoutAlignment.Center:
                        alignY = rowHeight / 2d - childMeasure.Request.Height / 2d;
                        break;
                    case LayoutAlignment.End:
                        alignY = rowHeight - childMeasure.Request.Height;
                        break;
                    default:
                        childHeight = rowHeight;
                        break;
                }

                child.Layout(new Rectangle(xOnRow, alignY, childMeasure.Request.Width, childHeight));
                xOnRow += childMeasure.Request.Width + this.Spacing;

            }
        }

        private void LayoutColumn(Dictionary<View, SizeRequest> childrenOnColumn, double columnX, double columnWidth)
        {
            var yOnColumn = 0d;
            foreach (var columnKeyValue in childrenOnColumn)
            {
                var childMeasure = columnKeyValue.Value;
                var child = columnKeyValue.Key;

                var alignX = columnX;
                var childWidth = childMeasure.Request.Width;
                switch (child.HorizontalOptions.Alignment)
                {
                    case LayoutAlignment.Start:
                        break;
                    case LayoutAlignment.Center:
                        alignX = columnWidth / 2d - childMeasure.Request.Width / 2d;
                        break;
                    case LayoutAlignment.End:
                        alignX = columnWidth - childMeasure.Request.Width;
                        break;
                    default:
                        childWidth = columnWidth;
                        break;
                }

                child.Layout(new Rectangle(alignX, yOnColumn, childWidth, childMeasure.Request.Height));
                yOnColumn += childMeasure.Request.Height + this.Spacing;

            }
        }
    }
}
