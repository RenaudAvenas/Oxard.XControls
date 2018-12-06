using System;
using Xamarin.Forms;

namespace Oxard.XControls.Graphics
{
    public class SolidColorBrush : Brush, IEquatable<SolidColorBrush>
    {
        public static readonly BindableProperty ColorProperty = BindableProperty.Create(nameof(Color), typeof(Color), typeof(SolidColorBrush), Color.Transparent);

        public Color Color
        {
            get => (Color)this.GetValue(ColorProperty);
            set => this.SetValue(ColorProperty, value);
        }

        public bool Equals(SolidColorBrush other)
        {
            return other?.Color == this.Color;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
                return false;
            if (ReferenceEquals(this, obj))
                return true;
            if (obj.GetType() != this.GetType())
                return false;
            return this.Equals((SolidColorBrush)obj);
        }

        public override int GetHashCode()
        {
            return this.Color.GetHashCode();
        }
    }
}
