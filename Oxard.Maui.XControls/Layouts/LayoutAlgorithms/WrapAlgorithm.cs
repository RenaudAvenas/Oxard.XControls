using Microsoft.Maui.Layouts;
using LayoutAlignment = Microsoft.Maui.Primitives.LayoutAlignment;
using Oxard.Maui.XControls.Extensions;

namespace Oxard.Maui.XControls.Layouts.LayoutAlgorithms;

/// <summary>
/// Describe how child must be placed on a layout
/// </summary>
public enum ChildAlignment
{
    /// <summary>
    /// Dock child on the left or top (depending on layout direction)
    /// </summary>
    LeftOrTop,
    /// <summary>
    /// Dock child on the right or bottom (depending on layout direction)
    /// </summary>
    RightOrBottom,
    /// <summary>
    /// Center child on layout
    /// </summary>
    Center,
    /// <summary>
    /// Maximize size of child to fill the layout
    /// </summary>
    Justify
}

/// <summary>
/// Algorithm that stack horizontally or vertically all children and wrap them if necessary
/// </summary>
public class WrapAlgorithm : LayoutManager
{
    private StackOrientation orientation;
    private double spacing;
    private double wrapSpacing;
    private ChildAlignment childAlignment;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="layout"></param>
    public WrapAlgorithm(Microsoft.Maui.ILayout layout) : base(layout)
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
    /// Get or set spacing between each wrap of children
    /// </summary>
    public double WrapSpacing
    {
        get => this.wrapSpacing;
        set
        {
            if (this.wrapSpacing == value)
                return;

            this.wrapSpacing = value;
            this.Layout.InvalidateMeasure();
        }
    }

    /// <summary>
    /// Get or set the childrend alignment by rows or columns in the wrap layout
    /// </summary>
    public ChildAlignment ChildAlignment
    {
        get => this.childAlignment;
        set
        {
            if (this.childAlignment == value)
                return;

            this.childAlignment = value;
            this.Layout.InvalidateArrange();
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
            return this.OnMeasureHorizontal(widthConstraint);
        else
            return this.OnMeasureVertical(heightConstraint);
    }

    /// <summary>
    /// Layout the children of the current layout
    /// </summary>
    /// <param name="bounds">Rectangle where layout must be arranged</param>
    public override Size ArrangeChildren(Rectangle bounds)
    {
        if (this.Orientation == StackOrientation.Horizontal)
            return this.OnLayoutChildrenHorizontal(bounds);
        else
            return this.OnLayoutChildrenVertical(bounds);
    }

    private Size OnMeasureVertical(double heightConstraint)
    {
        var maxHeight = 0d;
        var columnWidth = 0d;
        var actualX = 0d;
        var actualY = 0d;
        var firstElement = true;

        foreach (var child in this.Layout.Where(c => c.Visibility != Visibility.Collapsed))
        {
            var sizeRequest = child.Measure(double.PositiveInfinity, heightConstraint);
            if (firstElement)
            {
                // First element is always displayed on first row even if its width is larger than widthConstraint
                firstElement = false;
            }
            else if (actualY + sizeRequest.Height > heightConstraint)
            {
                // We need to wrap / change row
                maxHeight = Math.Max(maxHeight, actualY - this.Spacing);
                actualY = 0;
                actualX += columnWidth + this.WrapSpacing;
                columnWidth = 0;
            }

            actualY += sizeRequest.Height + this.Spacing;
            columnWidth = Math.Max(columnWidth, sizeRequest.Width);
        }

        var width = actualX + columnWidth;
        maxHeight = Math.Max(maxHeight, actualY - this.Spacing);

        return new Size(width, Math.Min(maxHeight, heightConstraint.TranslateIfInfinity()));
    }

    private Size OnMeasureHorizontal(double widthConstraint)
    {
        var maxWidth = 0d;
        var rowHeight = 0d;
        var actualX = 0d;
        var actualY = 0d;
        var firstElement = true;

        foreach (var child in this.Layout.Where(c => c.Visibility != Visibility.Collapsed))
        {
            var sizeRequest = child.Measure(widthConstraint, double.PositiveInfinity);
            if (firstElement)
            {
                // First element is always displayed on first row even if its width is larger than widthConstraint
                firstElement = false;
            }
            else if (actualX + sizeRequest.Width > widthConstraint)
            {
                // We need to wrap / change row
                maxWidth = Math.Max(maxWidth, actualX - this.Spacing);
                actualX = 0;
                actualY += rowHeight + this.WrapSpacing;
                rowHeight = 0;
            }

            actualX += sizeRequest.Width + this.Spacing;
            rowHeight = Math.Max(rowHeight, sizeRequest.Height);
        }

        var height = actualY + rowHeight;
        maxWidth = Math.Max(maxWidth, actualX - this.Spacing);

        return new Size(Math.Min(maxWidth, widthConstraint.TranslateIfInfinity()), height);
    }

    private Size OnLayoutChildrenHorizontal(Rectangle bounds)
    {
        var currentX = bounds.X;
        var currentY = bounds.Y;
        var rowHeight = 0d;
        var firstElement = true;
        var childrenOnRow = new Dictionary<IView, Size>();
        var finalWidth = bounds.Width;

        foreach (var child in this.Layout.Where(c => c.Visibility != Visibility.Collapsed))
        {
            var childMeasure = child.DesiredSize;

            if (firstElement)
            {
                if (childMeasure.Width > finalWidth)
                    finalWidth = childMeasure.Width;

                firstElement = false;
            }
            else if (currentX + childMeasure.Width > bounds.Width)
            {
                // Wrapping needed, we can display the current row
                this.LayoutRow(childrenOnRow, currentY, rowHeight, bounds.Width);

                currentX = bounds.X;
                currentY += rowHeight + this.WrapSpacing;
                rowHeight = 0;
                childrenOnRow.Clear();
            }

            childrenOnRow[child] = childMeasure;
            currentX += childMeasure.Width + this.Spacing;
            rowHeight = Math.Max(rowHeight, childMeasure.Height);
        }

        if (childrenOnRow.Count > 0)
        {
            this.LayoutRow(childrenOnRow, currentY, rowHeight, bounds.Width);
            currentY += rowHeight;
        }
        else
            currentY -= this.WrapSpacing;

        return new Size(finalWidth, currentY);
    }

    private Size OnLayoutChildrenVertical(Rectangle bounds)
    {
        var currentX = bounds.X;
        var currentY = bounds.Y;
        var columnWidth = 0d;
        var firstElement = true;
        var childrenOnColumn = new Dictionary<IView, Size>();
        var finalHeight = bounds.Height;

        foreach (var child in this.Layout.Where(c => c.Visibility != Visibility.Collapsed))
        {
            var childMeasure = child.DesiredSize;

            if (firstElement)
            {
                if (childMeasure.Height > finalHeight)
                    finalHeight = childMeasure.Height;

                firstElement = false;
            }
            else if (currentY + childMeasure.Height > bounds.Height)
            {
                // Wrapping needed, we can display the current row
                this.LayoutColumn(childrenOnColumn, currentX, columnWidth, bounds.Height);

                currentY = bounds.Y;
                currentX += columnWidth + this.WrapSpacing;
                columnWidth = 0;
                childrenOnColumn.Clear();
            }

            childrenOnColumn[child] = childMeasure;
            currentY += childMeasure.Height + this.Spacing;
            columnWidth = Math.Max(columnWidth, childMeasure.Width);
        }

        if (childrenOnColumn.Count > 0)
        {
            this.LayoutColumn(childrenOnColumn, currentX, columnWidth, bounds.Height);
            currentX += columnWidth;
        }
        else
            currentX -= this.WrapSpacing;

        return new Size(currentX, finalHeight);
    }

    private void LayoutRow(Dictionary<IView, Size> childrenOnRow, double rowY, double rowHeight, double totalWidth)
    {
        var xOnRow = 0d;

        var childWidthSum = childrenOnRow.Sum(c => c.Value.Width);
        childWidthSum += Spacing * (childrenOnRow.Count - 1);

        var justifyWidthBonus = 0d;

        if (totalWidth > childWidthSum)
        {
            switch (this.ChildAlignment)
            {
                case ChildAlignment.RightOrBottom:
                    xOnRow = totalWidth - childWidthSum;
                    break;
                case ChildAlignment.Center:
                    xOnRow = totalWidth / 2d - childWidthSum / 2d;
                    break;
                case ChildAlignment.Justify:
                    justifyWidthBonus = (totalWidth - childWidthSum) / childrenOnRow.Count;
                    break;
            }
        }

        foreach (var rowKeyValue in childrenOnRow)
        {
            var childMeasure = rowKeyValue.Value;
            var child = rowKeyValue.Key;

            var alignY = rowY;
            var childHeight = childMeasure.Height;
            switch (child.VerticalLayoutAlignment)
            {
                case LayoutAlignment.Start:
                    break;
                case LayoutAlignment.Center:
                    alignY += rowHeight / 2d - childMeasure.Height / 2d;
                    break;
                case LayoutAlignment.End:
                    alignY += rowHeight - childMeasure.Height;
                    break;
                default:
                    childHeight = rowHeight;
                    break;
            }

            child.Arrange(new Rectangle(xOnRow, alignY, childMeasure.Width + justifyWidthBonus, childHeight));
            xOnRow += childMeasure.Width + justifyWidthBonus + this.Spacing;
        }
    }

    private void LayoutColumn(Dictionary<IView, Size> childrenOnColumn, double columnX, double columnWidth, double totalHeight)
    {
        var yOnColumn = 0d;

        var childHeightSum = childrenOnColumn.Sum(c => c.Value.Height);
        childHeightSum += Spacing * (childrenOnColumn.Count - 1);

        var justifyHeightBonus = 0d;

        if (totalHeight > childHeightSum)
        {
            switch (this.ChildAlignment)
            {
                case ChildAlignment.RightOrBottom:
                    yOnColumn = totalHeight - childHeightSum;
                    break;
                case ChildAlignment.Center:
                    yOnColumn = totalHeight / 2d - childHeightSum / 2d;
                    break;
                case ChildAlignment.Justify:
                    justifyHeightBonus = (totalHeight - childHeightSum) / childrenOnColumn.Count;
                    break;
            }
        }

        foreach (var columnKeyValue in childrenOnColumn)
        {
            var childMeasure = columnKeyValue.Value;
            var child = columnKeyValue.Key;

            var alignX = columnX;
            var childWidth = childMeasure.Width;
            switch (child.HorizontalLayoutAlignment)
            {
                case LayoutAlignment.Start:
                    break;
                case LayoutAlignment.Center:
                    alignX += columnWidth / 2d - childMeasure.Width / 2d;
                    break;
                case LayoutAlignment.End:
                    alignX += columnWidth - childMeasure.Width;
                    break;
                default:
                    childWidth = columnWidth;
                    break;
            }

            child.Arrange(new Rectangle(alignX, yOnColumn, childWidth, childMeasure.Height + justifyHeightBonus));
            yOnColumn += childMeasure.Height + justifyHeightBonus + this.Spacing;
        }
    }
}
