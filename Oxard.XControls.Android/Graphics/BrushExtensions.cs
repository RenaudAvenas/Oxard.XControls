using Android.Graphics;
using Oxard.XControls.Graphics;
using Xamarin.Forms;
using AView = Android.Views.View;
using Xamarin.Forms.Platform.Android;
using Xamarin.Forms.Shapes;

namespace Oxard.XControls.Droid.Graphics
{
    /// <summary>
    /// Add extensions to brushes to support DrawingBrush
    /// </summary>
    public static partial class BrushExtensions
    {
        /// <summary>
        /// Update an element background with a <see cref="DrawingBrush"/>
        /// </summary>
        /// <param name="view">Android element</param>
        /// <param name="element">Xamarin.Forms element</param>
        /// <param name="drawingBrush">The brush to apply</param>
        public static void UpdateBackground(this AView view, VisualElement element, DrawingBrush drawingBrush)
        {
            var drawingBrushDrawable = new DrawingBrushDrawable(view, element, drawingBrush);
            view.SetBackground(drawingBrushDrawable);
        }

        /// <summary>
        /// Get the corresponding <see cref="Paint.Cap"/> value from a <see cref="PenLineCap"/> value
        /// </summary>
        /// <param name="penLineCap"><see cref="PenLineCap"/> value</param>
        /// <returns><see cref="Paint.Cap"/> value</returns>
        public static Paint.Cap ToAndroid(this PenLineCap penLineCap)
        {
            PenLineCap lineCap = penLineCap;
            Paint.Cap aLineCap = Paint.Cap.Butt;

            switch (lineCap)
            {
                case PenLineCap.Flat:
                    aLineCap = Paint.Cap.Butt;
                    break;
                case PenLineCap.Square:
                    aLineCap = Paint.Cap.Square;
                    break;
                case PenLineCap.Round:
                    aLineCap = Paint.Cap.Round;
                    break;
            }

            return aLineCap;
        }

        /// <summary>
        /// Get the corresponding <see cref="Paint.Join"/> value from a <see cref="PenLineJoin"/> value
        /// </summary>
        /// <param name="penLineCap"><see cref="PenLineJoin"/> value</param>
        /// <returns><see cref="Paint.Join"/> value</returns>
        public static Paint.Join ToAndroid(this PenLineJoin penLineJoin)
        {
            PenLineJoin lineJoin = penLineJoin;
            Paint.Join aLineJoin = Paint.Join.Miter;

            switch (lineJoin)
            {
                case PenLineJoin.Miter:
                    aLineJoin = Paint.Join.Miter;
                    break;
                case PenLineJoin.Bevel:
                    aLineJoin = Paint.Join.Bevel;
                    break;
                case PenLineJoin.Round:
                    aLineJoin = Paint.Join.Round;
                    break;
            }

            return aLineJoin;
        }
    }
}