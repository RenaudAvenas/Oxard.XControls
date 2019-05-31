using Oxard.XControls.Graphics;
using System;
using Xamarin.Forms;

namespace Oxard.XControls
{
    public interface IDrawable
    {
        event EventHandler GeometryChanged;

        double Height { get; }

        double Width { get; }

        Brush Fill { get; }

        Color Stroke { get; }

        double StrokeThickness { get; }

        Point StrokeDashArray { get; }

        Geometry Geometry { get; }
    }
}
