using Oxard.XControls.Graphics;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Shapes;

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
        /// Get the <see cref="Brush"/> used to draw the outline of the shape
        /// </summary>
        Brush Stroke { get; }

        /// <summary>
        /// Get the thickness of the stroke
        /// </summary>
        double StrokeThickness { get; }

        /// <summary>
        /// Get the stroke dash array
        /// </summary>
        DoubleCollection StrokeDashArray { get; }

        /// <summary>
        /// Get the geometry of the drawable
        /// </summary>
        Geometry Geometry { get; }

        /// <summary>
        /// Gets or the stroke dash offset.
        /// </summary>
        double StrokeDashOffset { get; }

        /// <summary>
        /// Gets the stroke line cap.
        /// </summary>
        /// <value>
        /// The stroke line cap.
        /// </value>
        PenLineCap StrokeLineCap { get; }

        /// <summary>
        /// Gets the stroke line join.
        /// </summary>
        /// <value>
        /// The stroke line join.
        /// </value>
        PenLineJoin StrokeLineJoin { get; }

        /// <summary>
        /// Gets the stroke miter limit.
        /// </summary>
        /// <value>
        /// The stroke miter limit.
        /// </value>
        double StrokeMiterLimit { get; }

        /// <summary>
        /// Gets the aspect.
        /// </summary>
        /// <value>
        /// The aspect.
        /// </value>
        Stretch Aspect { get; }
    }
}
