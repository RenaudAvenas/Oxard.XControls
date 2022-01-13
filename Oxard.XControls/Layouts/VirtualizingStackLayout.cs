using Oxard.XControls.Components;
using System;
using Xamarin.Forms;

namespace Oxard.XControls.Layouts
{
    /// <summary>
    /// Stack layout that display children vertically and use virtualization.
    /// This layout need to be used as ItemsPanel in an <see cref="Components.VirtualizingItemsControl"/> and must have a <see cref="ScrollView"/> in parent hierarchy.
    /// </summary>
    public class VirtualizingStackLayout : Layout<View>
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
        protected Rect Viewport { get; private set; }

        /// <summary>
        /// Method called when a measurement is asked.
        /// </summary>
        /// <param name="widthConstraint">Width constraint</param>
        /// <param name="heightConstraint">Height constraint</param>
        /// <returns>Requested size</returns>
        protected override SizeRequest OnMeasure(double widthConstraint, double heightConstraint)
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

            return new SizeRequest(new Size(request.Request.Width, height));
        }

        /// <summary>
        /// Layout the children of the current layout
        /// </summary>
        /// <param name="x">X delay</param>
        /// <param name="y">Y delay</param>
        /// <param name="width">Width constraint to layout</param>
        /// <param name="height">Height constraint to layout</param>
        protected override void LayoutChildren(double x, double y, double width, double height)
        {
            foreach (var item in VirtualizingItemsControl.GetDisplayedChildren())
                item.View.Layout(new Rectangle(x, y + rowHeight * item.Index, width, rowHeight));
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
            var newViewport = new Rect(ScrollOwner.ScrollX, ScrollOwner.ScrollY, ScrollOwner.Width, ScrollOwner.Height);

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
}
