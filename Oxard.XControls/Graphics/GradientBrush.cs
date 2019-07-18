using System.Collections.ObjectModel;
using Xamarin.Forms;

namespace Oxard.XControls.Graphics
{
    /// <summary>
    /// Base class for gradient brushes
    /// </summary>
    [ContentProperty("GradientStops")]
    public class GradientBrush : Brush
    {
        /// <summary>
        /// Identifies the GradientStops dependency property.
        /// </summary>
        public static readonly BindableProperty GradientStopsProperty = BindableProperty.Create(nameof(GradientStops), typeof(ObservableCollection<GradientStop>), typeof(GradientBrush));

        /// <summary>
        /// Get or set the <see cref="GradientStop"/> that define the gradient brush
        /// </summary>
        public ObservableCollection<GradientStop> GradientStops
        {
            get => (ObservableCollection<GradientStop>)this.GetValue(GradientStopsProperty);
            set => this.SetValue(GradientStopsProperty, value);
        }
    }
}
