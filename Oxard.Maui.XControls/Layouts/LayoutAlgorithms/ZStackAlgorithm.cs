using Microsoft.Maui.Layouts;

namespace Oxard.Maui.XControls.Layouts.LayoutAlgorithms;

/// <summary>
/// Algorithm that stack all children on top of each other
/// </summary>
public class ZStackAlgorithm : LayoutManager
{
    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="layout"></param>
    public ZStackAlgorithm(Microsoft.Maui.ILayout layout) : base(layout)
    {
    }

    /// <summary>
    /// Method called when a measurement is asked.
    /// </summary>
    /// <param name="widthConstraint">Width constraint</param>
    /// <param name="heightConstraint">Height constraint</param>
    /// <returns>Requested size</returns>
    public override Size Measure(double widthConstraint, double heightConstraint)
    {
        var maxSize = Size.Zero;
        foreach (var item in this.Layout.Where(c => c.Visibility != Visibility.Collapsed))
        {
            var measuredSize = item.Measure(widthConstraint, heightConstraint);
            maxSize.Width = Math.Max(measuredSize.Width, maxSize.Width);
            maxSize.Height = Math.Max(measuredSize.Height, maxSize.Height);
        }

        return new SizeRequest(maxSize);
    }

    /// <summary>
    /// Layout the children of the current layout
    /// </summary>
    /// <param name="bounds">Rectangle where layout must be arranged</param>
    public override Size ArrangeChildren(Rectangle bounds)
    {
        foreach (var child in this.Layout.Where(c => c.Visibility != Visibility.Collapsed))
        {
            child.Arrange(bounds);
        }

        return bounds.Size;
    }
}
