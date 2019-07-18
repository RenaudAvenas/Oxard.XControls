using System.Collections.ObjectModel;
using Xamarin.Forms;

namespace Oxard.XControls.Graphics
{
    /// <summary>
    /// Create a linear gradient brush
    /// </summary>
    public class LinearGradientBrush : GradientBrush
    {
        /// <summary>
        /// Identifies the StartPoint dependency property.
        /// </summary>
        public static readonly BindableProperty StartPointProperty = BindableProperty.Create(nameof(StartPoint), typeof(Point), typeof(LinearGradientBrush), Point.Zero);
        /// <summary>
        /// Identifies the EndPoint dependency property.
        /// </summary>
        public static readonly BindableProperty EndPointProperty = BindableProperty.Create(nameof(EndPoint), typeof(Point), typeof(LinearGradientBrush), new Point(1, 0));

        /// <summary>
        /// Default constructor
        /// </summary>
        public LinearGradientBrush()
        {
            this.GradientStops = new ObservableCollection<GradientStop>();
        }

        /// <summary>
        /// Get or set the start relative point (in percent of the gradient size) of the brush
        /// </summary>
        public Point StartPoint
        {
            get => (Point)this.GetValue(StartPointProperty);
            set => this.SetValue(StartPointProperty, value);
        }

        /// <summary>
        /// Get or set the end relative point (in percent of the gradient size) of the brush
        /// </summary>
        public Point EndPoint
        {
            get => (Point)this.GetValue(EndPointProperty);
            set => this.SetValue(EndPointProperty, value);
        }
    }
}
