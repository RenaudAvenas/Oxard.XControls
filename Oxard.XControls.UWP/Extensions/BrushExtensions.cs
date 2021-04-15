using Xamarin.Forms;
using Xamarin.Forms.Shapes;

namespace Oxard.XControls.UWP.Extensions
{
    public static class BrushExtensions
    {
        public static Windows.UI.Xaml.Media.Stretch ToWindows(this Stretch stretch)
        {
            var result = Windows.UI.Xaml.Media.Stretch.Fill;

            switch (stretch)
            {
                case Stretch.None:
                    return Windows.UI.Xaml.Media.Stretch.None;
                case Stretch.Fill:
                    return Windows.UI.Xaml.Media.Stretch.Fill;
                case Stretch.Uniform:
                    return Windows.UI.Xaml.Media.Stretch.Uniform;
                case Stretch.UniformToFill:
                    return Windows.UI.Xaml.Media.Stretch.UniformToFill;
            }

            return result;
        }

        public static Windows.UI.Xaml.Media.PenLineCap ToWindows(this PenLineCap penLineCap)
        {
            var result = Windows.UI.Xaml.Media.PenLineCap.Flat;

            switch (penLineCap)
            {
                case PenLineCap.Flat:
                   return Windows.UI.Xaml.Media.PenLineCap.Flat;
                case PenLineCap.Square:
                    return Windows.UI.Xaml.Media.PenLineCap.Square;
                case PenLineCap.Round:
                    return Windows.UI.Xaml.Media.PenLineCap.Round;
            }

            return result;
        }

        public static Windows.UI.Xaml.Media.PenLineJoin ToWindows(this PenLineJoin penLineJoin)
        {
            var result = Windows.UI.Xaml.Media.PenLineJoin.Bevel;

            switch (penLineJoin)
            {
                case PenLineJoin.Miter:
                    return Windows.UI.Xaml.Media.PenLineJoin.Miter;
                case PenLineJoin.Bevel:
                    return Windows.UI.Xaml.Media.PenLineJoin.Bevel;
                case PenLineJoin.Round:
                    return Windows.UI.Xaml.Media.PenLineJoin.Round;
            }

            return result;
        }
    }
}
