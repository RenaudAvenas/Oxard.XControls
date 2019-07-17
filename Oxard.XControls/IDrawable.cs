using Oxard.XControls.Graphics;
using System;
using Xamarin.Forms;

namespace Oxard.XControls
{
    /// <summary>
    /// Interface that defines a drawable. It can be a Shape or a drawingBrush.
    /// </summary>
    public interface IDrawable
    {
        /// <summary>
        /// Event called when shape geometry changed
        /// </summary>
        event EventHandler GeometryChanged;

        /// <summary>
        /// Get the height of the drawable
        /// </summary>
        double Height { get; }

        /// <summary>
        /// Get the width of the drawable
        /// </summary>
        double Width { get; }

        /// <summary>
        /// Get the <see cref="Brush"/> used to fill the shape
        /// </summary>
        Brush Fill { get; }

        /// <summary>
        /// Get the <see cref="Color"/> used to draw the outline of the shape
        /// </summary>
        Color Stroke { get; }

        /// <summary>
        /// Get the thickness of the stroke
        /// </summary>
        double StrokeThickness { get; }

        /// <summary>
        /// Get the stroke dash
        /// </summary>
        Point StrokeDashArray { get; }

        /// <summary>
        /// Get the geometry of the drawable
        /// </summary>
        Geometry Geometry { get; }
    }
}
