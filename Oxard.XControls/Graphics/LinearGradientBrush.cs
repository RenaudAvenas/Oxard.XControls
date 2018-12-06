using System.Collections.ObjectModel;
using Xamarin.Forms;

namespace Oxard.XControls.Graphics
{
    public class LinearGradientBrush : GradientBrush
    {
        public static readonly BindableProperty StartPointProperty = BindableProperty.Create(nameof(StartPoint), typeof(Point), typeof(LinearGradientBrush), Point.Zero);
        public static readonly BindableProperty EndPointProperty = BindableProperty.Create(nameof(EndPoint), typeof(Point), typeof(LinearGradientBrush), new Point(1, 0));

        public LinearGradientBrush()
        {
            this.GradientStops = new ObservableCollection<GradientStop>();
        }

        public Point StartPoint
        {
            get => (Point)this.GetValue(StartPointProperty);
            set => this.SetValue(StartPointProperty, value);
        }

        public Point EndPoint
        {
            get => (Point)this.GetValue(EndPointProperty);
            set => this.SetValue(EndPointProperty, value);
        }
    }
}
