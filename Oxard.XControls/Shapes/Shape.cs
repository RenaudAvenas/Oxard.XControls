using Oxard.XControls.Graphics;
using Oxard.XControls.Helpers;
using System;
using Xamarin.Forms;

namespace Oxard.XControls.Shapes
{
    /// <summary>
    /// Base class for shapes
    /// </summary>
    public abstract class Shape : View, IDrawable
    {
        /// <summary>
        /// Identifies the Fill dependency property.
        /// </summary>
        public static readonly BindableProperty FillProperty = BindableProperty.Create("Fill", typeof(Brush), typeof(Shape), Brushes.Transparent);
        /// <summary>
        /// Identifies the Stroke dependency property.
        /// </summary>
        public static readonly BindableProperty StrokeProperty = BindableProperty.Create("Stroke", typeof(Color), typeof(Shape), Color.Transparent);
        /// <summary>
        /// Identifies the StrokeThickness dependency property.
        /// </summary>
        public static readonly BindableProperty StrokeThicknessProperty = BindableProperty.Create("StrokeThickness", typeof(double), typeof(Shape), 0d, propertyChanged: OnStrokeThicknessPropertyChanged);
        /// <summary>
        /// Identifies the Stretch dependency property.
        /// </summary>
        public static readonly BindableProperty StretchProperty = BindableProperty.Create(nameof(Stretch), typeof(Stretch), typeof(Shape), Stretch.Uniform, propertyChanged: StretchPropertyChanged);
        /// <summary>
        /// Identifies the StrokeDash dependency property.
        /// </summary>
        public static readonly BindableProperty StrokeDashArrayProperty = BindableProperty.Create(nameof(StrokeDashArray), typeof(Point), typeof(Shape), new Point(1, 0));

        /// <summary>
        /// Event called when shape geometry changed
        /// </summary>
        public event EventHandler GeometryChanged;

        /// <summary>
        /// Get or set the <see cref="Brush"/> used to fill the shape
        /// </summary>
        public Brush Fill
        {
            get => (Brush)this.GetValue(FillProperty);
            set => this.SetValue(FillProperty, value);
        }

        /// <summary>
        /// Get or set the <see cref="Color"/> used to draw the outline of the shape
        /// </summary>
        public Color Stroke
        {
            get => (Color)this.GetValue(StrokeProperty);
            set => this.SetValue(StrokeProperty, value);
        }

        /// <summary>
        /// Get or set the thickness of the stroke
        /// </summary>
        public double StrokeThickness
        {
            get => (double)this.GetValue(StrokeThicknessProperty);
            set => this.SetValue(StrokeThicknessProperty, value);
        }

        /// <summary>
        /// Get or set the stroke dash
        /// </summary>
        public Point StrokeDashArray
        {
            get => (Point)this.GetValue(StrokeDashArrayProperty);
            set => this.SetValue(StrokeDashArrayProperty, value);
        }

        /// <summary>
        /// Get or set the stretch mode used to draw the shape
        /// </summary>
        public Stretch Stretch
        {
            get => (Stretch)this.GetValue(StretchProperty);
            set => this.SetValue(StretchProperty, value);
        }

        /// <summary>
        /// Get the geometry of the shape
        /// </summary>
        public abstract Geometry Geometry { get; }

        private static void OnStrokeThicknessPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            (bindable as Shape)?.OnStrokeThicknessChanged();
        }

        private static void StretchPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            (bindable as Shape)?.OnStretchChanged();
        }

        /// <summary>
        /// Measure the shape
        /// </summary>
        /// <param name="widthConstraint">Width used for measure</param>
        /// <param name="heightConstraint">Height used for measure</param>
        /// <returns>Shape size request</returns>
        protected override SizeRequest OnMeasure(double widthConstraint, double heightConstraint) => this.GetStandardMeasure(widthConstraint, heightConstraint);

        /// <summary>
        /// Raise <see cref="GeometryChanged"/> event and force shape to redraw
        /// </summary>
        protected void InvalidateGeometry()
        {
            this.GeometryChanged?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Called when stretch property changed
        /// </summary>
        protected virtual void OnStretchChanged()
        {
        }

        private void OnStrokeThicknessChanged()
        {
            this.InvalidateGeometry();
        }
    }
}
