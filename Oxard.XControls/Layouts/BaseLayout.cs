using Oxard.XControls.Layouts.LayoutAlgorithms;
using Xamarin.Forms;

namespace Oxard.XControls.Layouts
{
    /// <summary>
    /// Base layout class that can be inherited to use a specific <seealso cref="LayoutAlgorithm"/> for layouts passes.
    /// </summary>
    public abstract class BaseLayout<TAlgorithm> : Layout<View> where TAlgorithm : LayoutAlgorithm, new()
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BaseLayout{TAlgorithm}"/> class.
        /// </summary>
        protected BaseLayout()
        {
            this.Algorithm.ParentLayout = this;
        }

        /// <summary>
        /// Gets the algorithm that is used to layout children of the current layout.
        /// </summary>
        /// <value>
        /// The algorithm.
        /// </value>
        protected TAlgorithm Algorithm { get; } = new TAlgorithm();

        /// <summary>
        /// Method called when a measurement is asked.
        /// </summary>
        /// <param name="widthConstraint">Width constraint</param>
        /// <param name="heightConstraint">Height constraint</param>
        /// <returns>Requested size</returns>
        protected override SizeRequest OnMeasure(double widthConstraint, double heightConstraint)
        {
            return this.Algorithm.Measure(widthConstraint, heightConstraint);
        }

        /// <summary>
        /// Layout the children of the current layout
        /// </summary>
        /// <param name="x">X delay</param>
        /// <param name="y">Y delay</param>
        /// <param name="width">Width constraint to layout</param>
        /// <param name="height">Height constraint to layout</param>
        protected override void LayoutChildren(double x, double y, double width, double height)
        {
            this.Algorithm.LayoutChildren(x, y, width, height);
        }
    }
}
