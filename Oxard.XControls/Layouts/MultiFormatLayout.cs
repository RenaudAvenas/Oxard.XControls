using System;
using Oxard.XControls.Layouts.LayoutAlgorithms;
using Xamarin.Forms;

namespace Oxard.XControls.Layouts
{
    /// <summary>
    /// Layout that can be specified by external layout algorithm (see <seeaslo cref="LayoutAlgorithm"/>)
    /// </summary>
    public class MultiFormatLayout : Layout<View>
    {
        private bool isMeasuring;
        private bool isLayouting;

        /// <summary>
        /// Identifies the Algorithm property.
        /// </summary>
        public static readonly BindableProperty AlgorithmProperty = BindableProperty.Create(nameof(Algorithm), typeof(LayoutAlgorithm), typeof(MultiFormatLayout), null, propertyChanged: OnAlgorithmPropertyChanged);

        /// <summary>
        /// Get or set the current algorithm used to display children
        /// </summary>
        public LayoutAlgorithm Algorithm
        {
            get => (LayoutAlgorithm)this.GetValue(AlgorithmProperty);
            set => this.SetValue(AlgorithmProperty, value);
        }

        private static void OnAlgorithmPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            (bindable as MultiFormatLayout)?.OnAlgorithmChanged(oldValue as LayoutAlgorithm);
        }

        /// <summary>
        /// Method called when a measurement is asked.
        /// </summary>
        /// <param name="widthConstraint">Width constraint</param>
        /// <param name="heightConstraint">Height constraint</param>
        /// <returns>Requested size</returns>
        protected override sealed SizeRequest OnMeasure(double widthConstraint, double heightConstraint)
        {
            this.isMeasuring = true;

            this.BeforeMeasure(widthConstraint, heightConstraint);
            if (this.Algorithm == null)
                this.Algorithm = new ZStackAlgorithm();

            var measure = this.Algorithm.Measure(widthConstraint, heightConstraint);

            this.isMeasuring = false;

            return measure;
        }

        /// <summary>
        /// Layout the children of the current layout
        /// </summary>
        /// <param name="x">X delay</param>
        /// <param name="y">Y delay</param>
        /// <param name="width">Width constraint to layout</param>
        /// <param name="height">Height constraint to layout</param>
        protected override sealed void LayoutChildren(double x, double y, double width, double height)
        {
            this.isLayouting = true;

            this.BeforeLayoutChildren(x, y, width, height);
            if (this.Algorithm == null)
                this.Algorithm = new ZStackAlgorithm();

            this.Algorithm.LayoutChildren(x, y, width, height);

            this.isLayouting = false;
        }

        /// <summary>
        /// Method called before a measurement is asked.
        /// </summary>
        /// <param name="widthConstraint">Width constraint</param>
        /// <param name="heightConstraint">Height constraint</param>
        protected virtual void BeforeMeasure(double widthConstraint, double heightConstraint)
        {
        }

        /// <summary>
        /// Called before layout the children of the current layout
        /// </summary>
        /// <param name="x">X delay</param>
        /// <param name="y">Y delay</param>
        /// <param name="width">Width constraint to layout</param>
        /// <param name="height">Height constraint to layout</param>
        protected virtual void BeforeLayoutChildren(double x, double y, double width, double height)
        {
        }

        private void OnAlgorithmChanged(LayoutAlgorithm oldAlgorithm)
        {
            if (oldAlgorithm != null)
            {
                oldAlgorithm.Invalidated -= this.OnAlgorithmInvalidated;
                oldAlgorithm.ParentLayout = null;
            }

            this.Algorithm.ParentLayout = this;
            this.Algorithm.Invalidated += this.OnAlgorithmInvalidated;

            if (!this.isMeasuring && !this.isLayouting)
            {
                this.InvalidateMeasure();
                this.InvalidateLayout();
            }
        }

        private void OnAlgorithmInvalidated(object sender, EventArgs e)
        {
            if (!this.isMeasuring && !this.isLayouting)
            {
                this.InvalidateMeasure();
                this.InvalidateLayout();
            }
        }
    }
}
