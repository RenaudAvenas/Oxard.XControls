using Xamarin.Forms;

namespace Oxard.XControls.Interactivity
{
    /// <summary>
    /// Define a property setter for oxard trigger (<seealso cref="TriggerBase"/>)
    /// </summary>
    public class Setter
    {
        private object convertedValue;

        /// <summary>
        /// Gets or sets the property to set.
        /// </summary>
        /// <value>
        /// The property.
        /// </value>
        public BindableProperty Property { get; set; }

        /// <summary>
        /// Gets or sets value to apply to the specified <see cref="Property"/>.
        /// </summary>
        /// <value>
        /// The value.
        /// </value>
        public object Value { get; set; }

        internal void Apply(BindableObject bindable)
        {
            if(this.convertedValue == null && this.Value != null)
            {
                if (this.Value is string stringValue)
                    this.convertedValue = stringValue.ConvertFor(this.Property.ReturnType);
                else
                    this.convertedValue = this.Value;
            }

            bindable.SetValue(this.Property, this.convertedValue);
        }
    }
}
