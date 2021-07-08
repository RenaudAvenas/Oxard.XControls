using Oxard.XControls.Layouts.LayoutAlgorithms;
using System;
using Xamarin.Forms;

namespace Oxard.XControls.Layouts
{
    /// <summary>
    /// Layout that stack horizontally or vertically all children and wrap them if necessary
    /// </summary>
    public class WrapLayout : BaseLayout<WrapAlgorithm>
    {
        /// <summary>
        /// Identifies the Orientation property.
        /// </summary>
        public static readonly BindableProperty OrientationProperty = BindableProperty.Create(nameof(Orientation), typeof(StackOrientation), typeof(WrapLayout), StackOrientation.Horizontal, propertyChanged:OnOrientationPropertyChanged);

        /// <summary>
        /// Identifies the Spacing property.
        /// </summary>
        public static readonly BindableProperty SpacingProperty = BindableProperty.Create(nameof(Spacing), typeof(double), typeof(WrapLayout), default(double), propertyChanged: OnSpacingPropertyChanged);

        /// <summary>
        /// Identifies the WrapSpacing property.
        /// </summary>
        public static readonly BindableProperty WrapSpacingProperty = BindableProperty.Create(nameof(WrapSpacing), typeof(double), typeof(WrapLayout), default(double), propertyChanged: OnWrapSpacingPropertyChanged);

        /// <summary>
        /// Identifies the ChildAlignment property.
        /// </summary>
        public static readonly BindableProperty ChildAlignmentProperty = BindableProperty.Create(nameof(ChildAlignment), typeof(ChildAlignment), typeof(WrapAlgorithm), ChildAlignment.LeftOrTop, propertyChanged: OnChildAlignmentPropertyChanged);

        /// <summary>
        /// Get or set the orientation of the WrapLayout
        /// </summary>
        public StackOrientation Orientation
        {
            get => (StackOrientation)this.GetValue(OrientationProperty);
            set => this.SetValue(OrientationProperty, value);
        }

        /// <summary>
        /// Get or set the space between each child
        /// </summary>
        public double Spacing
        {
            get => (double)this.GetValue(SpacingProperty);
            set => this.SetValue(SpacingProperty, value);
        }

        /// <summary>
        /// Get or set spacing between each wrap of children
        /// </summary>
        public double WrapSpacing
        {
            get => (double)this.GetValue(WrapSpacingProperty);
            set => this.SetValue(WrapSpacingProperty, value);
        }

        /// <summary>
        /// Get or set the childrend alignment by rows or columns in the wrap layout
        /// </summary>
        public ChildAlignment ChildAlignment
        {
            get => (ChildAlignment)this.GetValue(ChildAlignmentProperty);
            set => this.SetValue(ChildAlignmentProperty, value);
        }

        /// <summary>
        /// Called when <see cref="Orientation"/> property changed. It affects orientation property of its internal <seealso cref="WrapAlgorithm"/>
        /// </summary>
        protected virtual void OnOrientationChanged()
        {
            this.Algorithm.Orientation = this.Orientation;
        }

        /// <summary>
        /// Called when <see cref="Spacing"/> property changed. It affects spacing property of its internal <seealso cref="WrapAlgorithm"/>
        /// </summary>
        protected virtual void OnSpacingChanged()
        {
            this.Algorithm.Spacing = this.Spacing;
        }

        /// <summary>
        /// Called when <see cref="WrapSpacing"/> property changed. It affects wrap spacing property of its internal <seealso cref="WrapAlgorithm"/>
        /// </summary>
        protected virtual void OnWrapSpacingChanged()
        {
            this.Algorithm.WrapSpacing = this.WrapSpacing;
        }

        /// <summary>
        /// Called when <see cref="ChildAlignment"/> property changed. It affects ChildAlignment property of its internal <seealso cref="WrapAlgorithm"/>
        /// </summary>
        protected virtual void OnChildAlignmentChanged()
        {
            this.Algorithm.ChildAlignment = this.ChildAlignment;
        }

        private static void OnOrientationPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            (bindable as WrapLayout)?.OnOrientationChanged();
        }

        private static void OnSpacingPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            (bindable as WrapLayout)?.OnSpacingChanged();
        }

        private static void OnWrapSpacingPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            (bindable as WrapLayout)?.OnWrapSpacingChanged();
        }

        private static void OnChildAlignmentPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            (bindable as WrapLayout)?.OnChildAlignmentChanged();
        }
    }
}
