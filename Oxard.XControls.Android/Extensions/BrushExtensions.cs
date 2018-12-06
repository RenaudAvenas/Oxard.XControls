using Android.Graphics;
using Oxard.XControls.Graphics;
using System;
using System.Collections.ObjectModel;
using Xamarin.Forms.Platform.Android;

namespace Oxard.XControls.Droid.Extensions
{
    public static class BrushExtensions
    {
        /// <summary>
        /// Create the shader corresponding to the Brush.
        /// </summary>
        /// <param name="brush">Brush to use</param>
        /// <param name="paint">Paint used to draw shape</param>
        /// <param name="width">Width of the Brush</param>
        /// <param name="height">Height of the Brush</param>
        public static void ToBackground(this Brush brush, Paint paint, float width = 0, float height = 0)
        {
            if (brush is SolidColorBrush solid)
                paint.Color = solid.Color.ToAndroid();
            else if (brush is LinearGradientBrush linear)
                SetLinearShader(paint, linear, width, height);
            else if (brush is RadialGradientBrush radial)
                SetRadialShader(paint, radial, width, height);
        }

        public static void ToAndroidGradientStops(this ObservableCollection<GradientStop> gradientStops, out int[] colors, out float[] positions)
        {
            colors = new int[gradientStops.Count];
            positions = new float[gradientStops.Count];

            for (int i = 0; i < gradientStops.Count; i++)
            {
                var gradientStop = gradientStops[i];
                colors[i] = gradientStop.Color.ToAndroid();
                positions[i] = (float)gradientStop.Offset;
            }
        }

        private static void SetLinearShader(Paint paint, LinearGradientBrush linear, float width, float height)
        {
            float x0 = (float)linear.StartPoint.X * width;
            float y0 = (float)linear.StartPoint.Y * height;
            float x1 = (float)linear.EndPoint.X * width;
            float y1 = (float)linear.EndPoint.Y * height;

            linear.GradientStops.ToAndroidGradientStops(out int[] colors, out float[] positions);

            var gradient = new LinearGradient(
                x0,
                y0,
                x1,
                y1,
                colors,
                positions,
                Shader.TileMode.Mirror);

            paint.Dither = true;
            paint.SetShader(gradient);
        }

        private static void SetRadialShader(Paint paint, RadialGradientBrush radial, float width, float height)
        {
            float centerX = (float)radial.Center.X * width;
            float centerY = (float)radial.Center.Y * height;
            var radiusX = width * (float)radial.RadiusX;
            var radiusY = height * (float)radial.RadiusY;
            float radius = Math.Min(radiusX, radiusY);

            radial.GradientStops.ToAndroidGradientStops(out int[] colors, out float[] positions);

            var gradient = new RadialGradient(
                centerX,
                centerY,
                radius,
                colors,
                positions,
                Shader.TileMode.Clamp);

            var matrix = new Matrix();
            var scaleX = radiusX / radius;
            var scaleY = radiusY / radius;
            matrix.PostScale(scaleX, scaleY);
            //matrix.PreTranslate(-width / 2, 0);
            matrix.PostTranslate(-((width / 2f * scaleX) - (width / 2f)), -((height / 2f * scaleY) - (height / 2f)));
            gradient.SetLocalMatrix(matrix);

            paint.Dither = true;
            paint.SetShader(gradient);
        }
    }
}