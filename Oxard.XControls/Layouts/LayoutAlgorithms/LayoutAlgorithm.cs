using System;
using Xamarin.Forms;

namespace Oxard.XControls.Layouts.LayoutAlgorithms
{
    /// <summary>
    /// Base class for layout algorithms
    /// </summary>
    public abstract class LayoutAlgorithm : BindableObject
    {
        /// <summary>
        /// Event raised when current algorithm should change disposition or measure.
        /// </summary>
        public event EventHandler Invalidated;

        /// <summary>
        /// Get or set the layout that use current algorithm
        /// </summary>
        public Layout<View> ParentLayout { get; set; }

        /// <summary>
        /// Method called when a measurement is asked.
        /// </summary>
        /// <param name="widthConstraint">Width constraint</param>
        /// <param name="heightConstraint">Height constraint</param>
        /// <returns>Requested size</returns>
        public SizeRequest Measure(double widthConstraint, double heightConstraint)
        {
            if (this.ParentLayout == null)
                return new SizeRequest(Size.Zero);

            return this.OnMeasure(widthConstraint, heightConstraint);
        }

        /// <summary>
        /// Layout the children of the current layout
        /// </summary>
        /// <param name="x">X delay</param>
        /// <param name="y">Y delay</param>
        /// <param name="width">Width constraint to layout</param>
        /// <param name="height">Height constraint to layout</param>
        public void LayoutChildren(double x, double y, double width, double height)
        {
            if (this.ParentLayout == null)
                return;

            this.OnLayoutChildren(x, y, width, height);
        }

        /// <summary>
        /// Method called when a measurement is asked.
        /// </summary>
        /// <param name="widthConstraint">Width constraint</param>
        /// <param name="heightConstraint">Height constraint</param>
        /// <returns>Requested size</returns>
        protected abstract SizeRequest OnMeasure(double widthConstraint, double heightConstraint);

        /// <summary>
        /// Layout the children of the current layout
        /// </summary>
        /// <param name="x">X delay</param>
        /// <param name="y">Y delay</param>
        /// <param name="width">Width constraint to layout</param>
        /// <param name="height">Height constraint to layout</param>
        protected abstract void OnLayoutChildren(double x, double y, double width, double height);

        /// <summary>
        /// Invalidate last disposition and measure of this algorithm
        /// </summary>
        protected void Invalidate()
        {
            this.Invalidated?.Invoke(this, EventArgs.Empty);
        }
    }
}
