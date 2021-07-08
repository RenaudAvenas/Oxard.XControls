using System;
using Xamarin.Forms;

namespace Oxard.XControls.Layouts.LayoutAlgorithms
{
    /// <summary>
    /// Delegate used to launched <see cref="LayoutAlgorithm.Invalidated"/> event
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="args">The <see cref="LayoutAlgorithmInvalidatedEventArgs"/> instance containing the event data.</param>
    public delegate void LayoutAlgorithmInvalidatedEventHandler(object sender, LayoutAlgorithmInvalidatedEventArgs args);

    /// <summary>
    /// Class used to invalidate measure, layout or both
    /// </summary>
    /// <seealso cref="System.EventArgs" />
    /// <seealso cref="LayoutAlgorithm"/>
    public class LayoutAlgorithmInvalidatedEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LayoutAlgorithmInvalidatedEventArgs"/> class.
        /// </summary>
        /// <param name="invalidateMeasure">Initialize <see cref="InvalidateMeasure"/> with the value</param>
        /// <param name="invalidateLayout">Initialize <see cref="InvalidateLayout"/> with the value</param>
        public LayoutAlgorithmInvalidatedEventArgs(bool invalidateMeasure, bool invalidateLayout)
        {
            this.InvalidateMeasure = invalidateMeasure;
            this.InvalidateLayout = invalidateLayout;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LayoutAlgorithmInvalidatedEventArgs"/> class.
        /// <see cref="InvalidateMeasure"/> and <see cref="InvalidateLayout"/> properties will be set to <c>true</c>
        /// </summary>
        public LayoutAlgorithmInvalidatedEventArgs() : this(true, true)
        {

        }

        /// <summary>
        /// Gets a value indicating whether the algorithm need to invalidate measure.
        /// </summary>
        /// <value>
        ///   <c>true</c> if need to invalidate measure; otherwise, <c>false</c>.
        /// </value>
        public bool InvalidateMeasure { get; }

        /// <summary>
        /// Gets a value indicating whether the algorithm need to invalidate layout.
        /// </summary>
        /// <value>
        ///   <c>true</c> if need to invalidate layout; otherwise, <c>false</c>.
        /// </value>
        public bool InvalidateLayout { get; }
    }

    /// <summary>
    /// Base class for layout algorithms
    /// </summary>
    public abstract class LayoutAlgorithm : BindableObject
    {
        /// <summary>
        /// Event raised when current algorithm should change disposition or measure.
        /// </summary>
        public event LayoutAlgorithmInvalidatedEventHandler Invalidated;

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
        /// Call this method when measure or layout is requested in inherited classes.
        /// </summary>
        /// <param name="bindable">The bindable layout algorithm.</param>
        /// <param name="oldValue">The old value of a property.</param>
        /// <param name="newValue">The new value of a property.</param>
        protected static void OnMeasureLayoutRequested(BindableObject bindable, object oldValue, object newValue)
        {
            (bindable as LayoutAlgorithm)?.Invalidate();
        }

        /// <summary>
        /// Call this method when only layout is requested in inherited classes.
        /// </summary>
        /// <param name="bindable">The bindable layout algorithm.</param>
        /// <param name="oldValue">The old value of a property.</param>
        /// <param name="newValue">The new value of a property.</param>
        protected static void OnLayoutOnlyRequested(BindableObject bindable, object oldValue, object newValue)
        {
            (bindable as LayoutAlgorithm)?.InvalidateLayout();
        }

        /// <summary>
        /// Call this method when only measure is requested in inherited classes.
        /// </summary>
        /// <param name="bindable">The bindable layout algorithm.</param>
        /// <param name="oldValue">The old value of a property.</param>
        /// <param name="newValue">The new value of a property.</param>
        protected static void OnMeasureOnlyRequested(BindableObject bindable, object oldValue, object newValue)
        {
            (bindable as LayoutAlgorithm)?.InvalidateMeasure();
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
            this.Invalidated?.Invoke(this, new LayoutAlgorithmInvalidatedEventArgs());
        }

        /// <summary>
        /// Invalidate last disposition of this algorithm
        /// </summary>
        protected void InvalidateLayout()
        {
            this.Invalidated?.Invoke(this, new LayoutAlgorithmInvalidatedEventArgs(false, true));
        }

        /// <summary>
        /// Invalidate last measure of this algorithm
        /// </summary>
        protected void InvalidateMeasure()
        {
            this.Invalidated?.Invoke(this, new LayoutAlgorithmInvalidatedEventArgs(true, false));
        }
    }
}
