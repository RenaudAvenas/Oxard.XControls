using System;
using Xamarin.Forms;

namespace Oxard.XControls.Graphics
{
    /// <summary>
    /// Create a brush as plain color
    /// </summary>
    public class SolidColorBrush : Brush, IEquatable<SolidColorBrush>
    {
        /// <summary>
        /// Identifies the Color dependency property.
        /// </summary>
        public static readonly BindableProperty ColorProperty = BindableProperty.Create(nameof(Color), typeof(Color), typeof(SolidColorBrush), Color.Transparent);

        /// <summary>
        /// Get or set the plain color
        /// </summary>
        public Color Color
        {
            get => (Color)this.GetValue(ColorProperty);
            set => this.SetValue(ColorProperty, value);
        }

        /// <summary>
        /// Check equality between this instance and the <paramref name="other"/> instance
        /// </summary>
        /// <param name="other">Other instance</param>
        /// <returns>True if color is equals else false</returns>
        public bool Equals(SolidColorBrush other)
        {
            return other?.Color == this.Color;
        }

        /// <summary>
        /// Determines whether the specified object is equal to the current object.
        /// </summary>
        /// <param name="obj">The object to compare with the current object.</param>
        /// <returns>True if the specified object is equal to the current object; otherwise, false.</returns>
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

        /// <summary>
        /// Serves as the default hash function.
        /// </summary>
        /// <returns>A hash code for the current object.</returns>
        public override int GetHashCode()
        {
            return this.Color.GetHashCode();
        }
    }
}
