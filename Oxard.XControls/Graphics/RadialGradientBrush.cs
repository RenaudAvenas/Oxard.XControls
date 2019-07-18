using System.Collections.ObjectModel;
using Xamarin.Forms;

namespace Oxard.XControls.Graphics
{
    /// <summary>
    /// Create a radial gradient brush
    /// </summary>
    public class RadialGradientBrush : GradientBrush
    {
        /// <summary>
        /// Identifies the Center dependency property.
        /// </summary>
        public static readonly BindableProperty CenterProperty = BindableProperty.Create(nameof(Center), typeof(Point), typeof(RadialGradientBrush), new Point(0.5, 0.5));
        /// <summary>
        /// Identifies the RadiusX dependency property.
        /// </summary>
        public static readonly BindableProperty RadiusXProperty = BindableProperty.Create(nameof(RadiusX), typeof(double), typeof(RadialGradientBrush), 0.5d);
        /// <summary>
        /// Identifies the RadiusY dependency property.
        /// </summary>
        public static readonly BindableProperty RadiusYProperty = BindableProperty.Create(nameof(RadiusY), typeof(double), typeof(RadialGradientBrush), 0.5d);

        /// <summary>
        /// Default constructor
        /// </summary>
        public RadialGradientBrush()
        {
            this.GradientStops = new ObservableCollection<GradientStop>();
        }

        /// <summary>
        /// Get or set the relative center point of the gradient
        /// </summary>
        public Point Center
        {
            get => (Point)this.GetValue(CenterProperty);
            set => this.SetValue(CenterProperty, value);
        }

        /// <summary>
        /// Get or set the relative radius on x axis
        /// </summary>
        public double RadiusX
        {
            get => (double)this.GetValue(RadiusXProperty);
            set => this.SetValue(RadiusXProperty, value);
        }

        /// <summary>
        /// Get or set the relative radius on y axis
        /// </summary>
        public double RadiusY
        {
            get => (double)this.GetValue(RadiusYProperty);
            set => this.SetValue(RadiusYProperty, value);
        }
    }
}
