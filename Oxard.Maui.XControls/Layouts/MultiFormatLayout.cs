using Microsoft.Maui.Layouts;
using Oxard.Maui.XControls.Layouts.LayoutAlgorithms;

namespace Oxard.Maui.XControls.Layouts
{
    /// <summary>
    /// Layout that can be specified by external layout manager
    /// </summary>
    public class MultiFormatLayout : Layout
    {
        /// <summary>
        /// Identifies the Algorithm property.
        /// </summary>
        public static readonly BindableProperty AlgorithmProperty = BindableProperty.Create(nameof(Algorithm), typeof(LayoutManager), typeof(MultiFormatLayout), null, propertyChanged: OnAlgorithmPropertyChanged);

        /// <summary>
        /// Get or set the current algorithm used to display children
        /// </summary>
        public LayoutManager Algorithm
        {
            get => (LayoutManager)this.GetValue(AlgorithmProperty);
            set => this.SetValue(AlgorithmProperty, value);
        }

        private static void OnAlgorithmPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            (bindable as MultiFormatLayout)?.OnAlgorithmChanged();
        }

        /// <summary>
        /// Create the layout manager used by the current layout
        /// </summary>
        /// <returns>The layout manager</returns>
        protected override ILayoutManager CreateLayoutManager()
        {
            return this.Algorithm ?? new ZStackAlgorithm(this);
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

        private void OnAlgorithmChanged()
        {
            _layoutManager = this.CreateLayoutManager();
            this.InvalidateMeasure();
        }
    }
}
