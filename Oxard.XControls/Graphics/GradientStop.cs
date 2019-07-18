using Xamarin.Forms;

namespace Oxard.XControls.Graphics
{
    /// <summary>
    /// Define a gradient color with relative offset
    /// </summary>
    public class GradientStop : BindableObject
    {
        /// <summary>
        /// Identifies the Color dependency property.
        /// </summary>
        public static readonly BindableProperty ColorProperty = BindableProperty.Create(nameof(Color), typeof(Color), typeof(GradientStop), Color.Transparent);
        /// <summary>
        /// Identifies the Offset dependency property.
        /// </summary>
        public static readonly BindableProperty OffsetProperty = BindableProperty.Create(nameof(Offset), typeof(double), typeof(GradientStop), 0d);

        /// <summary>
        /// Get or set the color at the <see cref="Offset"/>
        /// </summary>
        public Color Color
        {
            get => (Color)this.GetValue(ColorProperty);
            set => this.SetValue(ColorProperty, value);
        }

        /// <summary>
        /// Get or set the offset where the <see cref="Color"/> should be displayed. This is a relative value in percent of the gradient size
        /// </summary>
        public double Offset
        {
            get => (double)this.GetValue(OffsetProperty);
            set => this.SetValue(OffsetProperty, value);
        }
    }
}
