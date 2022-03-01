using Microsoft.Maui.Layouts;
using Oxard.Maui.XControls.Layouts.LayoutAlgorithms;

namespace Oxard.Maui.XControls.Layouts
{
    /// <summary>
    /// Layout that stack all children on top of each other
    /// </summary>
    public class ZStackLayout : Layout
    {
        /// <summary>
        /// Create the layout manager used by the current layout
        /// </summary>
        /// <returns>The layout manager</returns>
        protected override sealed ILayoutManager CreateLayoutManager()
        {
            return new ZStackAlgorithm(this);
        }
    }
}
