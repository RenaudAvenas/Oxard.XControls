namespace Oxard.Maui.XControls.Interactivity;

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

    /// <summary>
    /// Gets or sets the target object where the setter will be applied. Let this value to null if the target is the object that declare this setter
    /// </summary>
    /// <value>
    /// The target.
    /// </value>
    public BindableObject Target { get; set; }

    /// <summary>
    /// Get or set the binding used to get the original value of property affected by the setter
    /// </summary>
    public BindingBase OriginalValueBinding { get; set; }

    internal object ConvertedValue => convertedValue;

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
