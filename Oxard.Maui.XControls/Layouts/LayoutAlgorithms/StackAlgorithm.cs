using Microsoft.Maui.Layouts;
using LayoutAlignment = Microsoft.Maui.Primitives.LayoutAlignment;

namespace Oxard.Maui.XControls.Layouts.LayoutAlgorithms;

/// <summary>
/// Algorithm that stack horizontally or vertically all children
/// </summary>
public class StackAlgorithm : LayoutManager
{
    private StackOrientation orientation;
    private double spacing;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="layout"></param>
    public StackAlgorithm(Microsoft.Maui.ILayout layout) : base(layout)
    {
    }

    /// <summary>
    /// Get or set the orientation of the WrapLayout
    /// </summary>
    public StackOrientation Orientation
    {
        get => this.orientation;
        set
        {
            if (this.orientation == value)
                return;

            this.orientation = value;
            this.Layout.InvalidateMeasure();
        }
    }

    /// <summary>
    /// Get or set the space between each child
    /// </summary>
    public double Spacing
    {
        get => this.spacing;
        set
        {
            if (this.spacing == value)
                return;

            this.spacing = value;
            this.Layout.InvalidateMeasure();
        }
    }

    /// <summary>
    /// Method called when a measurement is asked.
    /// </summary>
    /// <param name="widthConstraint">Width constraint</param>
    /// <param name="heightConstraint">Height constraint</param>
    /// <returns>Requested size</returns>
    public override Size Measure(double widthConstraint, double heightConstraint)
    {
        if (this.Orientation == StackOrientation.Horizontal)
            return this.OnMeasureHorizontal(heightConstraint);
        else
            return this.OnMeasureVertical(widthConstraint);
    }

    /// <summary>
    /// Layout the children of the current layout
    /// </summary>
    /// <param name="bounds">Rectangle where layout must be arranged</param>
    public override Size ArrangeChildren(Rectangle bounds)
    {
        if (this.Orientation == StackOrientation.Horizontal)
            this.OnLayoutChildrenHorizontal(bounds.X, bounds.Y, bounds.Height);
        else
            this.OnLayoutChildrenVertical(bounds.X, bounds.Y, bounds.Width);

        return this.Layout.DesiredSize;
    }

    private Size OnMeasureVertical(double widthConstraint)
    {
        var totalHeight = 0d;
        var width = 0d;
        var calculateWidth = double.IsPositiveInfinity(widthConstraint);

        foreach (var child in this.Layout.Where(c => c.Visibility != Visibility.Collapsed))
        {
            var sizeRequest = child.Measure(widthConstraint, double.PositiveInfinity);
            if (calculateWidth)
                width = Math.Max(width, sizeRequest.Width);

            totalHeight += sizeRequest.Height + this.Spacing;
        }

        if (totalHeight > 0)
            totalHeight -= this.Spacing;

        return new Size(calculateWidth ? width : widthConstraint, totalHeight);
    }

    private Size OnMeasureHorizontal(double heightConstraint)
    {
        var totalWidth = 0d;
        var height = 0d;
        var calculateHeight = double.IsPositiveInfinity(heightConstraint);

        foreach (var child in this.Layout.Where(c => c.Visibility != Visibility.Collapsed))
        {
            var sizeRequest = child.Measure(double.PositiveInfinity, heightConstraint);
            if (calculateHeight)
                height = Math.Max(height, sizeRequest.Height);

            totalWidth += sizeRequest.Width + this.Spacing;
        }

        if (totalWidth > 0)
            totalWidth -= this.Spacing;

        return new SizeRequest(new Size(totalWidth, calculateHeight ? height : heightConstraint));
    }

    private void OnLayoutChildrenHorizontal(double x, double y, double height)
    {
        var currentX = x;
        foreach (var child in this.Layout.Where(c => c.Visibility != Visibility.Collapsed))
        {
            var childMeasure = child.DesiredSize;

            var alignY = y;
            var childHeight = childMeasure.Height;
            switch (child.VerticalLayoutAlignment)
            {
                case LayoutAlignment.Start:
                    break;
                case LayoutAlignment.Center:
                    alignY = height / 2d - childMeasure.Height / 2d;
                    break;
                case LayoutAlignment.End:
                    alignY = height - childMeasure.Height;
                    break;
                default:
                    childHeight = height;
                    break;
            }

            child.Arrange(new Rectangle(currentX, alignY, childMeasure.Width, childHeight));
            currentX += childMeasure.Width + this.Spacing;
        }
    }

    private void OnLayoutChildrenVertical(double x, double y, double width)
    {
        var currentY = y;
        foreach (var child in this.Layout.Where(c => c.Visibility != Visibility.Collapsed))
        {
            var childMeasure = child.Measure(width, double.PositiveInfinity);

            var alignX = x;
            var childWidth = childMeasure.Width;
            switch (child.HorizontalLayoutAlignment)
            {
                case LayoutAlignment.Start:
                    break;
                case LayoutAlignment.Center:
                    alignX = width / 2d - childMeasure.Width / 2d;
                    break;
                case LayoutAlignment.End:
                    alignX = width - childMeasure.Width;
                    break;
                default:
                    childWidth = width;
                    break;
            }

            child.Arrange(new Rectangle(alignX, currentY, childWidth, childMeasure.Height));
            currentY += childMeasure.Height + this.Spacing;
        }
    }
}
