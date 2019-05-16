using Oxard.XControls.Graphics;
using Oxard.XControls.Interpretors;
using Oxard.XControls.UWP.Interpretors;
using System;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;
using Xamarin.Forms;

namespace Oxard.XControls.UWP.Extensions
{
    public static class BrushExtensions
    {
        public static Windows.UI.Xaml.Media.Brush ToBrush(this Color color)
        {
            return new Windows.UI.Xaml.Media.SolidColorBrush(Windows.UI.Color.FromArgb((byte)Math.Round(color.A * 255), (byte)Math.Round(color.R * 255), (byte)Math.Round(color.G * 255), (byte)Math.Round(color.B * 255)));
        }

        public static Windows.UI.Color ToColor(this Color color)
        {
            return Windows.UI.Color.FromArgb((byte)Math.Round(color.A * 255), (byte)Math.Round(color.R * 255), (byte)Math.Round(color.G * 255), (byte)Math.Round(color.B * 255));
        }

        public static Windows.UI.Xaml.Media.Brush ToBrush(this Graphics.Brush brush)
        {
            if (brush is Graphics.SolidColorBrush solid)
                return solid.Color.ToBrush();

            if (brush is Graphics.LinearGradientBrush linear)
                return ToLinearGradientBrush(linear);

            if (brush is RadialGradientBrush radial)
                return ToRadialGradientBrush(radial);

            if (brush is DrawingBrush drawingBrush)
                return ToDrawingBrush(drawingBrush);

            return null;
        }

        public static void FillGradientBrush(Graphics.GradientBrush gradient, Windows.UI.Xaml.Media.GradientBrush nativeGradient)
        {
            foreach (var gradientStop in gradient.GradientStops)
                nativeGradient.GradientStops.Add(new Windows.UI.Xaml.Media.GradientStop { Color = gradientStop.Color.ToColor(), Offset = gradientStop.Offset });
        }

        private static Windows.UI.Xaml.Media.Brush ToLinearGradientBrush(Graphics.LinearGradientBrush linear)
        {
            Windows.UI.Xaml.Media.LinearGradientBrush nativeGradient = new Windows.UI.Xaml.Media.LinearGradientBrush();
            nativeGradient.StartPoint = linear.StartPoint.ToPoint();
            nativeGradient.EndPoint = linear.EndPoint.ToPoint();

            FillGradientBrush(linear, nativeGradient);

            return nativeGradient;
        }

        private static Windows.UI.Xaml.Media.Brush ToRadialGradientBrush(RadialGradientBrush radial)
        {
            var interpretor = InterpretorManager.Get<IBrushInterpretor>();
            if (interpretor != null)
                return interpretor.RadialToBrush(radial);

            throw new NotSupportedException("Radial brush is not supported by UWP, register an interpretor of type IBrushInterpretor in InterpretorManager or remove RadialGradientBrush");
        }

        private static Windows.UI.Xaml.Media.Brush ToDrawingBrush(DrawingBrush drawingBrush)
        {
            var interpretor = InterpretorManager.Get<IBrushInterpretor>();
            if (interpretor != null)
                return interpretor.DrawingToBrush(drawingBrush);

            throw new NotSupportedException("Drawing brush is not supported by UWP, register an interpretor of type IBrushInterpretor in InterpretorManager or remove DrawingBrush");
        }
    }
}
