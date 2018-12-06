using Oxard.XControls.Graphics;
using Oxard.XControls.Interpretors;

namespace Oxard.XControls.UWP.Interpretors
{
    public interface IBrushInterpretor : IInterpretor
    {
        Windows.UI.Xaml.Media.Brush RadialToBrush(RadialGradientBrush radial);
    }
}
