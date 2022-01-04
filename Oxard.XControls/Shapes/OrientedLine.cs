using System;
using Xamarin.Forms;
using Xamarin.Forms.Shapes;

namespace Oxard.XControls.Shapes
{
    /// <summary>
    /// Class that draw a vertical or horizontal line.
    /// </summary>
    public class OrientedLine : Path
    {
        private bool isLoaded;

        /// <summary>
        /// Identifies the Orientation dependency property.
        /// </summary>
        public static readonly BindableProperty OrientationProperty = BindableProperty.Create(nameof(Orientation), typeof(LineOrientation), typeof(OrientedLine), LineOrientation.Vertical, propertyChanged: OnOrientationPropertyChanged);

        /// <summary>
        /// Get or set the orientation of the line
        /// </summary>
        public LineOrientation Orientation
        {
            get => (LineOrientation)GetValue(OrientationProperty);
            set => SetValue(OrientationProperty, value);
        }

        /// <summary>
        /// Called when instance size changed (Width and Height).
        /// </summary>
        /// <param name="width">New width</param>
        /// <param name="height">New height</param>
        protected override void OnSizeAllocated(double width, double height)
        {
            this.isLoaded = true;
            this.RefreshGeometry();
            base.OnSizeAllocated(width, height);
        }

        /// <summary>
        /// Refresh the data use to draw the line
        /// </summary>
        protected void RefreshGeometry()
        {
            if (!this.isLoaded)
                return;

            this.Data = Graphics.GeometryHelper.GetOrientedLine(this.StrokeThickness, this.Orientation, this.Orientation == LineOrientation.Vertical ? this.Height : this.Width, this.Orientation == LineOrientation.Vertical ? this.Width : this.Height);
        }

        /// <summary>
        /// Called when <see cref="Orientation"/> property changed. By default, it call <see cref="RefreshGeometry"/>.
        /// </summary>
        protected virtual void OnOrientationChanged()
        {
            this.RefreshGeometry();
        }

        private static void OnOrientationPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            (bindable as OrientedLine)?.OnOrientationChanged();
        }
    }
}