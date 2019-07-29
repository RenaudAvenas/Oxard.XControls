using System.Collections.ObjectModel;
using Xamarin.Forms;

namespace Oxard.XControls.Graphics
{
    /// <summary>
    /// Base class for gradient brushes
    /// </summary>
    [ContentProperty("GradientStops")]
    public abstract class GradientBrush : Brush
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

        /// <summary>
        /// Creates a new object that is a copy of the current instance.
        /// </summary>
        /// <returns>A new object that is a copy of this instance.</returns>
        public sealed override object Clone()
        {
            var gradientBrush = this.CloneGradientBrush();

            ObservableCollection<GradientStop> gradientStopsCopy = new ObservableCollection<GradientStop>();
            foreach (var gradientStop in GradientStops)
                gradientStopsCopy.Add(new GradientStop { Color = gradientStop.Color, Offset = gradientStop.Offset });

            gradientBrush.GradientStops = gradientStopsCopy;

            return gradientBrush;
        }

        /// <summary>
        /// Creates a new <see cref="GradientBrush"/> that is a copy of the current instance.
        /// Just clone custom properties of inherited classes. The clone method of GradientBrush already copies its own properties.
        /// </summary>
        /// <returns>A new <see cref="GradientBrush"/> that is a copy of this instance.</returns>
        protected abstract GradientBrush CloneGradientBrush();
    }
}
