using Oxard.XControls.Graphics;
using Xamarin.Forms;

namespace Oxard.XControls
{
    public interface IDrawable
    {
        double Height { get; }

        double Width { get; }

        Brush Fill { get; }

        Color Stroke { get; }

        double StrokeThickness { get; }

        Point StrokeDashArray { get; }

        Geometry Geometry { get; }
    }
}
