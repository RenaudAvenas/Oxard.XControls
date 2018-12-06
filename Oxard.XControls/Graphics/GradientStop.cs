using Xamarin.Forms;

namespace Oxard.XControls.Graphics
{
    public class GradientStop : BindableObject
    {
        public static readonly BindableProperty ColorProperty = BindableProperty.Create(nameof(Color), typeof(Color), typeof(GradientStop), Color.Transparent);
        public static readonly BindableProperty OffsetProperty = BindableProperty.Create(nameof(Offset), typeof(double), typeof(GradientStop), 0d);

        public Color Color
        {
            get => (Color)this.GetValue(ColorProperty);
            set => this.SetValue(ColorProperty, value);
        }

        public double Offset
        {
            get => (double)this.GetValue(OffsetProperty);
            set => this.SetValue(OffsetProperty, value);
        }
    }
}
