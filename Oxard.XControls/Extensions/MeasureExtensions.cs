using Oxard.XControls.Extensions;
using Xamarin.Forms;

namespace Oxard.XControls.Helpers
{
    /// <summary>
    /// Helpers for controls measurements
    /// </summary>
    public static class MeasureExtensions
    {
        /// <summary>
        /// Return a standard SizeRequest based on widthConstraint and heightConstraint or Width and Height requested.
        /// </summary>
        /// <example>
        /// If constraints are infinity and size undefined (equals to -1), this method returns a <see cref="Xamarin.Forms.SizeRequest"/> with <see cref="Xamarin.Forms.Size.Zero"/> (or <paramref name="defaultValue"/> if it is specified)
        /// If constraints are infinity but width is set to 10, the return <see cref="Xamarin.Forms.SizeRequest"/> will have a <see cref="Xamarin.Forms.SizeRequest.Request"/> set to width = 10 and height = <paramref name="defaultValue"/>
        /// If constraints are defined and size undefined but alignements are not stretch, this method returns a <see cref="Xamarin.Forms.SizeRequest"/> with <see cref="Xamarin.Forms.Size.Zero"/> (or <paramref name="defaultValue"/> if it is specified)
        /// </example>
        /// <param name="view">Element to measure</param>
        /// <param name="widthConstraint">Width which can be infinity</param>
        /// <param name="heightConstraint">Height which can be infinity</param>
        /// <param name="defaultValue">Value to use if width or height are infinity and request size undefined</param>
        /// <returns>A SizeRequest with correct sizes for measure pass</returns>
        public static SizeRequest GetStandardMeasure(this View view, double widthConstraint, double heightConstraint, double defaultValue = 0d)
        {
            double widthConsideredConstraint = view.HorizontalOptions.Expands ? widthConstraint : defaultValue;
            double heightConsideredConstraint = view.VerticalOptions.Expands ? heightConstraint : defaultValue;

            if (view.WidthRequest >= 0d)
                widthConsideredConstraint = view.WidthRequest;

            if (view.HeightRequest >= 0d)
                heightConsideredConstraint = view.HeightRequest;

            return new SizeRequest(new Size(widthConsideredConstraint.TranslateIfInfinity(defaultValue), heightConsideredConstraint.TranslateIfInfinity(defaultValue)));
        }
    }
}
