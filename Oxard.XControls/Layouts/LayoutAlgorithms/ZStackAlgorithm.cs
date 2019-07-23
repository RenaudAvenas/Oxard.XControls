using System;
using Xamarin.Forms;

namespace Oxard.XControls.Layouts.LayoutAlgorithms
{
    /// <summary>
    /// Algorithm that stack all children on top of each other
    /// </summary>
    public class ZStackAlgorithm : LayoutAlgorithm
    {
        /// <summary>
        /// Method called when a measurement is asked.
        /// </summary>
        /// <param name="widthConstraint">Width constraint</param>
        /// <param name="heightConstraint">Height constraint</param>
        /// <returns>Requested size</returns>
        protected override SizeRequest OnMeasure(double widthConstraint, double heightConstraint)
        {
            var maxSize = Size.Zero;
            foreach (var item in this.ParentLayout.Children)
            {
                var measuredSize = item.Measure(widthConstraint, heightConstraint, MeasureFlags.IncludeMargins);
                maxSize.Width = Math.Max(measuredSize.Request.Width, maxSize.Width);
                maxSize.Height = Math.Max(measuredSize.Request.Height, maxSize.Height);
            }

            return new SizeRequest(maxSize);
        }

        /// <summary>
        /// Layout the children of the current layout
        /// </summary>
        /// <param name="x">X delay</param>
        /// <param name="y">Y delay</param>
        /// <param name="width">Width constraint to layout</param>
        /// <param name="height">Height constraint to layout</param>
        protected override void OnLayoutChildren(double x, double y, double width, double height)
        {
            var rectangle = new Rectangle(x, y, width, height);
            foreach (var child in this.ParentLayout.Children)
            {
                Layout.LayoutChildIntoBoundingRegion(child, rectangle);
            }
        }
    }
}
