using Oxard.XControls.Graphics;
using Oxard.XControls.Interpretors;
using Windows.UI.Xaml.Media;
using Xamarin.Forms;

namespace Oxard.XControls.UWP.Interpretors
{
    public interface ISegmentInterpretor : IInterpretor
    {
        PathSegment ToNativeSegment(GeometrySegment segment, Point fromPoint);
    }
}
