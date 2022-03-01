using Microsoft.Maui.Layouts;
using Oxard.Maui.XControls.Components;
using Oxard.Maui.XControls.Layouts.LayoutAlgorithms;

namespace Oxard.Maui.XControls.Layouts;

/// <summary>
/// Stack layout that display children vertically and use virtualization.
/// This layout need to be used as ItemsPanel in an <see cref="Components.VirtualizingItemsControl"/> and must have a <see cref="ScrollView"/> in parent hierarchy.
/// </summary>
public class VirtualizingStackLayout : Layout
{
    private double rowHeight = double.NaN;
    private const int itemOverflowNumber = 5;
    private int lastStartRange;
    private int lastEndRange;

    /// <summary>
    /// Get the <see cref="Components.VirtualizingItemsControl"/> parent
    /// </summary>
    protected VirtualizingItemsControl VirtualizingItemsControl { get; private set; }

    /// <summary>
    /// Get the <see cref="ScrollView"/> parent
    /// </summary>
    protected ScrollView ScrollOwner => this.VirtualizingItemsControl?.ScrollOwner;

    /// <summary>
    /// Get the viewport bounds displayed by the <see cref="ScrollOwner"/>
    /// </summary>
    protected Rectangle Viewport { get; private set; }

    /// <summary>
    /// Create the layout manager used by the current layout
    /// </summary>
    /// <returns>The layout manager</returns>
    protected override ILayoutManager CreateLayoutManager()
    {
        return new VirtualizingStackAlgorithm(this);
    }

    internal Size NeedMeasure(double widthConstraint, double heightConstraint)
    {
        this.InitializeScrollOwner();
        var firstItem = VirtualizingItemsControl?.GenerateAt(0);

        if (firstItem == null)
            return base.OnMeasure(widthConstraint, heightConstraint);

        var request = firstItem.Measure(widthConstraint, double.PositiveInfinity, MeasureFlags.IncludeMargins);
        var oldRowHeight = rowHeight;
        this.rowHeight = request.Request.Height;
        var height = this.VirtualizingItemsControl.ItemsSource.Count * request.Request.Height;

        if (oldRowHeight != rowHeight)
            GenerateItemsForViewport();

        return new Size(request.Request.Width, height);
    }

    /// <summary>
    /// Layout the children of the current layout
    /// </summary>
    /// <param name="bounds">X delay</param>
    internal Size NeedLayout(Rectangle bounds)
    {
        foreach (var item in VirtualizingItemsControl.GetDisplayedChildren())
            item.View.Layout(new Rectangle(bounds.X, bounds.Y + rowHeight * item.Index, bounds.Width, rowHeight));

        return this.DesiredSize;
    }

    /// <summary>
    /// Calls when <see cref="VirtualizingItemsControl"/> <see cref="Components.VirtualizingItemsControl.ItemsSource"/> property changed
    /// </summary>
    protected virtual void OnItemsSourceChanged()
    {
        lastEndRange = 0;
        lastStartRange = 0;
        this.GenerateItemsForViewport();
    }

    /// <summary>
    /// Calls when viewport of <see cref="ScrollOwner"/> changed
    /// </summary>
    protected virtual void OnViewportChanged()
    {
        this.GenerateItemsForViewport();
    }

    /// <summary>
    /// Generate a range of item with virtualization
    /// </summary>
    /// <param name="minIndex">The start range index</param>
    /// <param name="maxIndex">The end range index</param>
    protected void GenerateChildrenForRange(int minIndex, int maxIndex)
    {
        this.VirtualizingItemsControl.GenerateRange(minIndex, maxIndex);
    }

    /// <summary>
    /// Return a point where the child at <paramref name="index"/> should appear
    /// </summary>
    /// <param name="index">Index of child in collection</param>
    /// <param name="scrollToPosition">Position of child after scrolling</param>
    /// <returns>The top left point to scroll to</returns>
    protected virtual Point GetChildPositionForScroll(int index, ScrollToPosition scrollToPosition)
    {
        var y = rowHeight * index;
        var adaptedScrollToPosition = scrollToPosition;

        if(adaptedScrollToPosition == ScrollToPosition.MakeVisible)
        {
            if (y < Viewport.Y)
                adaptedScrollToPosition = ScrollToPosition.Start;
            else if (y + rowHeight > Viewport.Y + Viewport.Height)
                adaptedScrollToPosition = ScrollToPosition.End;
            else
                y = Viewport.Y;
        }

        switch (adaptedScrollToPosition)
        {
            case ScrollToPosition.Center:
                y -= (Viewport.Height / 2d) - (rowHeight / 2d);
                break;
            case ScrollToPosition.End:
                y -= Viewport.Height - rowHeight;
                break;
        }

        return new Point(0, y);
    }

    internal void SetVirtualizingItemsControl(VirtualizingItemsControl virtualizingItemsControl)
    {
        if (this.ScrollOwner != null)
        {
            this.ScrollOwner.SizeChanged -= this.ScrollOwner_SizeChanged;
            this.ScrollOwner.Scrolled -= this.ScrollOwner_Scrolled;
        }

        this.VirtualizingItemsControl = virtualizingItemsControl;
        this.InitializeScrollOwner();
    }

    internal void InitializeItemsSource()
    {
        InvalidateMeasure();
        Device.BeginInvokeOnMainThread(OnItemsSourceChanged);
    }

    internal Point InternalGetChildPositionForScroll(int index, ScrollToPosition scrollToPosition)
    {
        return this.GetChildPositionForScroll(index, scrollToPosition);
    }

    private void InitializeScrollOwner()
    {
        if (this.ScrollOwner == null)
            return;

        this.ScrollOwner.SizeChanged += ScrollOwner_SizeChanged;
        this.ScrollOwner.Scrolled += ScrollOwner_Scrolled;

        CalculateViewport();
    }

    private void ScrollOwner_Scrolled(object sender, ScrolledEventArgs e)
    {
        CalculateViewport();
    }

    private void ScrollOwner_SizeChanged(object sender, System.EventArgs e)
    {
        CalculateViewport();
    }

    private void CalculateViewport()
    {
        var newViewport = new Rectangle(ScrollOwner.ScrollX, ScrollOwner.ScrollY, ScrollOwner.Width, ScrollOwner.Height);

        if (double.IsNaN(this.rowHeight))
        {
            this.Viewport = newViewport;
            return;
        }

        if (Viewport != newViewport)
        {
            this.Viewport = newViewport;
            OnViewportChanged();
        }
    }

    private void GenerateItemsForViewport()
    {
        if (this.Viewport.IsEmpty || double.IsNaN(this.rowHeight))
            return;

        if (this.VirtualizingItemsControl.ItemsSource == null || this.VirtualizingItemsControl.ItemsSource.Count == 0)
        {
            this.VirtualizingItemsControl.RecycleAll();
            return;
        }

        int startVisibleIndex = (int)Math.Ceiling(this.Viewport.Y / this.rowHeight);
        int endVisibleIndex = (int)Math.Floor(this.Viewport.Height / this.rowHeight) + startVisibleIndex + 1;

        var startLayoutsIndex = Math.Max(0, startVisibleIndex - itemOverflowNumber);
        var endLayoutsIndex = Math.Min(this.VirtualizingItemsControl.ItemsSource.Count - 1, endVisibleIndex + itemOverflowNumber);

        if (this.lastEndRange == endLayoutsIndex && this.lastStartRange == startLayoutsIndex)
            return;

        if (endLayoutsIndex >= lastEndRange + itemOverflowNumber)
        {
            // Recycling elements at start range
            for (int i = lastStartRange; i < startLayoutsIndex; i++)
                this.VirtualizingItemsControl.RecycleAt(i);
        }

        if (startLayoutsIndex <= lastStartRange - itemOverflowNumber)
        {
            // Recycling elements at end range
            for (int i = endLayoutsIndex; i < lastEndRange; i++)
                this.VirtualizingItemsControl.RecycleAt(i);
        }

        lastStartRange = startLayoutsIndex;
        lastEndRange = endLayoutsIndex;

        GenerateChildrenForRange(startLayoutsIndex, endLayoutsIndex);
    }
}
