using System;
using Oxard.XControls.Graphics;
using Oxard.XControls.Shapes;
using Xamarin.Forms;

namespace Oxard.XControls.Droid.Graphics
{
    public class DrawableAdapter : IDrawable
    {
        public DrawableAdapter(VisualElement element, Brush brush)
        {
            this.Height = element.Height;
            this.Width = element.Width;
            this.Fill = brush;
            this.Geometry = GeometryHelper.GetRectangle(element.Width, element.Height, 0, CornerRadius.Zero, CornerRadius.Zero, CornerRadius.Zero, CornerRadius.Zero);
        }

        public event EventHandler GeometryChanged;

        public double Height { get; }

        public double Width { get; }

        public Brush Fill { get; }

        public Color Stroke => Color.Transparent;

        public double StrokeThickness => 0;

        public Point StrokeDashArray { get; } = Point.Zero;

        public Geometry Geometry { get; }
    }
}