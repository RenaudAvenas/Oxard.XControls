using System.Collections.ObjectModel;
using Xamarin.Forms;

namespace Oxard.XControls.Graphics
{
    [ContentProperty("GradientStops")]
    public class GradientBrush : Brush
    {
        public static readonly BindableProperty GradientStopsProperty = BindableProperty.Create(nameof(GradientStops), typeof(ObservableCollection<GradientStop>), typeof(GradientBrush));

        public ObservableCollection<GradientStop> GradientStops
        {
            get => (ObservableCollection<GradientStop>)this.GetValue(GradientStopsProperty);
            set => this.SetValue(GradientStopsProperty, value);
        }
    }
}
