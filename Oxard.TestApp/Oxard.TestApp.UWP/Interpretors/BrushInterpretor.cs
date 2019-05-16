using Oxard.XControls.Graphics;
using Oxard.XControls.UWP.Extensions;
using Oxard.XControls.UWP.Interpretors;
using System;

namespace Oxard.TestApp.UWP.Interpretors
{
    public class BrushInterpretor : IBrushInterpretor
    {
        public Windows.UI.Xaml.Media.Brush DrawingToBrush(DrawingBrush drawingBrush)
        {
            return drawingBrush.Fill.ToBrush();
        }

        public Windows.UI.Xaml.Media.Brush RadialToBrush(RadialGradientBrush radial)
        {
            throw new NotImplementedException();
        }
    }
}
