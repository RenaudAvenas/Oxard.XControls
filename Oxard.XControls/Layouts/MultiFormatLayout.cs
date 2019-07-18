using System;
using Oxard.XControls.Layouts.LayoutAlgorythms;
using Xamarin.Forms;

namespace Oxard.XControls.Layouts
{
    /// <summary>
    /// Layout that can be specified by external layout algorythm (see <seeaslo cref="LayoutAlgorythm"/>)
    /// </summary>
    public class MultiFormatLayout : Layout<View>
    {
        /// <summary>
        /// Identifies the Algorythm property.
        /// </summary>
        public static readonly BindableProperty AlgorythmProperty = BindableProperty.Create(nameof(Algorythm), typeof(LayoutAlgorythm), typeof(MultiFormatLayout), null, propertyChanged: OnAlgorythmPropertyChanged);
        
        /// <summary>
        /// Get or set the current algorythm used to display children
        /// </summary>
        public LayoutAlgorythm Algorythm
        {
            get => (LayoutAlgorythm)this.GetValue(AlgorythmProperty);
            set => this.SetValue(AlgorythmProperty, value);
        }
        
        private static void OnAlgorythmPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            (bindable as MultiFormatLayout)?.OnAlgorythmChanged(oldValue as LayoutAlgorythm);
        }

        /// <summary>
        /// Method called when a measurement is asked.
        /// </summary>
        /// <param name="widthConstraint">Width constraint</param>
        /// <param name="heightConstraint">Height constraint</param>
        /// <returns>Requested size</returns>
        protected override SizeRequest OnMeasure(double widthConstraint, double heightConstraint)
        {
            var algorythm = this.Algorythm ?? new ZStackAlgorythm();
            return base.OnMeasure(widthConstraint, heightConstraint);
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
            var algorythm = this.Algorythm ?? new ZStackAlgorythm();
            algorythm.LayoutChildren(x, y, width, height);
        }

        private void OnAlgorythmChanged(LayoutAlgorythm oldAlgorythm)
        {
            if (oldAlgorythm != null)
            {
                oldAlgorythm.Invalidated -= this.OnAlgorythmInvalidated;
                oldAlgorythm.ParentLayout = null;
            }

            this.Algorythm.ParentLayout = this;
            this.Algorythm.Invalidated += this.OnAlgorythmInvalidated;
            this.InvalidateMeasure();
            this.InvalidateLayout();
        }

        private void OnAlgorythmInvalidated(object sender, EventArgs e)
        {
            this.InvalidateMeasure();
        }
    }
}
