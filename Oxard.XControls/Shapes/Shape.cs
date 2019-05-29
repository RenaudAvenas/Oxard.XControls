using Oxard.XControls.Graphics;
using Oxard.XControls.Helpers;
using System;
using Xamarin.Forms;

namespace Oxard.XControls.Shapes
{
    public abstract class Shape : View, IDrawable
    {
        public static readonly BindableProperty FillProperty = BindableProperty.Create("Fill", typeof(Brush), typeof(Shape), Brushes.Transparent);
        public static readonly BindableProperty StrokeProperty = BindableProperty.Create("Stroke", typeof(Color), typeof(Shape), Color.Transparent);
        public static readonly BindableProperty StrokeThicknessProperty = BindableProperty.Create("StrokeThickness", typeof(double), typeof(Shape), 0d);
        public static readonly BindableProperty StretchProperty = BindableProperty.Create(nameof(Stretch), typeof(Stretch), typeof(Shape), Stretch.Uniform, propertyChanged: StretchPropertyChanged);
        public static readonly BindableProperty StrokeDashArrayProperty = BindableProperty.Create(nameof(StrokeDashArray), typeof(Point), typeof(Shape), new Point(1, 0));

        public event EventHandler GeometryChanged;

        public Brush Fill
        {
            get => (Brush)this.GetValue(FillProperty);
            set => this.SetValue(FillProperty, value);
        }

        public Color Stroke
        {
            get => (Color)this.GetValue(StrokeProperty);
            set => this.SetValue(StrokeProperty, value);
        }

        public double StrokeThickness
        {
            get => (double)this.GetValue(StrokeThicknessProperty);
            set => this.SetValue(StrokeThicknessProperty, value);
        }

        public Point StrokeDashArray
        {
            get => (Point)this.GetValue(StrokeDashArrayProperty);
            set => this.SetValue(StrokeDashArrayProperty, value);
        }

        public Stretch Stretch
        {
            get => (Stretch)this.GetValue(StretchProperty);
            set => this.SetValue(StretchProperty, value);
        }

        public abstract Geometry Geometry { get; }

        protected override SizeRequest OnMeasure(double widthConstraint, double heightConstraint) => this.GetStandardMeasure(widthConstraint, heightConstraint);

        protected void InvalidateGeometry()
        {
            this.GeometryChanged?.Invoke(this, EventArgs.Empty);
        }

        protected virtual void OnStretchChanged()
        {
        }

        private static void StretchPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            (bindable as Shape)?.OnStretchChanged();
        }
    }
}
