using System.Collections.ObjectModel;
using Xamarin.Forms;

namespace Oxard.XControls.Graphics
{
    public class RadialGradientBrush : GradientBrush
    {
        public static readonly BindableProperty CenterProperty = BindableProperty.Create(nameof(Center), typeof(Point), typeof(RadialGradientBrush), new Point(0.5, 0.5));
        public static readonly BindableProperty RadiusXProperty = BindableProperty.Create(nameof(RadiusX), typeof(double), typeof(RadialGradientBrush), 0.5d);
        public static readonly BindableProperty RadiusYProperty = BindableProperty.Create(nameof(RadiusY), typeof(double), typeof(RadialGradientBrush), 0.5d);

        public RadialGradientBrush()
        {
            this.GradientStops = new ObservableCollection<GradientStop>();
        }

        public Point Center
        {
            get => (Point)this.GetValue(CenterProperty);
            set => this.SetValue(CenterProperty, value);
        }

        public double RadiusX
        {
            get => (double)this.GetValue(RadiusXProperty);
            set => this.SetValue(RadiusXProperty, value);
        }

        public double RadiusY
        {
            get => (double)this.GetValue(RadiusYProperty);
            set => this.SetValue(RadiusYProperty, value);
        }
    }
}
