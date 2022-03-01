using Microsoft.Maui.Layouts;

namespace Oxard.Maui.XControls.Layouts.LayoutAlgorithms
{
    internal class VirtualizingStackAlgorithm : LayoutManager
    {
        private VirtualizingStackLayout layout;

        public VirtualizingStackAlgorithm(Microsoft.Maui.ILayout layout) : base(layout)
        {
            this.layout = layout as VirtualizingStackLayout;
        }

        public override Size ArrangeChildren(Rectangle bounds)
        {
            return layout.NeedLayout(bounds);
        }

        public override Size Measure(double widthConstraint, double heightConstraint)
        {
            return layout.NeedMeasure(widthConstraint, heightConstraint);
        }
    }
}
