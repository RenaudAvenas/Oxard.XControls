using System;
using Xamarin.Forms;

namespace Oxard.XControls.Interactivity
{
    internal class BindablePropertyValueExtension : IEquatable<BindablePropertyValueExtension>
    {
        public BindablePropertyValueExtension(BindableObject target, BindableProperty property)
        {
            this.Target = target;
            this.Property = property;
        }

        public object Value
        {
            get => this.Target.GetValue(Property);
            set => this.Target.SetValue(Property, value);
        }

        public BindableObject Target { get; }

        public BindableProperty Property { get; }

        public bool Equals(BindablePropertyValueExtension other)
        {
            if (other == null)
                return false;

            return this.Target.Equals(other.Target) && this.Property.Equals(other.Property);
        }

        public override bool Equals(object obj)
        {
            return this.Equals(obj as BindablePropertyValueExtension);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
