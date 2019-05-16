using System;
using Xamarin.Forms;

namespace Oxard.XControls.Graphics
{
    public abstract class DrawingBrush : Brush, IDrawable
    {
        public static readonly BindableProperty FillProperty = BindableProperty.Create(nameof(Fill), typeof(Brush), typeof(DrawingBrush), Brushes.Transparent);
        public static readonly BindableProperty StrokeProperty = BindableProperty.Create(nameof(Stroke), typeof(Color), typeof(DrawingBrush), Color.Transparent);
        public static readonly BindableProperty StrokeThicknessProperty = BindableProperty.Create(nameof(StrokeThickness), typeof(double), typeof(DrawingBrush), 0d);
        public static readonly BindableProperty StrokeDashArrayProperty = BindableProperty.Create(nameof(StrokeDashArray), typeof(Point), typeof(DrawingBrush), new Point(1, 0));

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

        public abstract Geometry Geometry { get; }

        public double Width { get; private set; }

        public double Height { get; private set; }

        public void SetSize(double width, double height)
        {
            this.Width = width;
            this.Height = height;
            this.OnSizeAllocated(width, height);
        }

        protected virtual void OnSizeAllocated(double width, double height)
        {
        }
        
        protected void InvalidateGeometry()
        {
            this.GeometryChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
