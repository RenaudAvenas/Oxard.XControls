using Oxard.Maui.XControls.Extensions;

namespace Oxard.Maui.XControls.Interactivity;

/// <summary>
/// Class that describe behavior on binding value changed
/// </summary>
public class DataTrigger : TriggerBase
{
    private Type bindingValueType;
    private object convertedValue;

    /// <summary>
    /// Get or set the binding used to check trigger activation
    /// </summary>
    public BindingBase Binding { get; set; }

    /// <summary>
    /// Gets or sets the condition value.
    /// </summary>
    /// <value>
    /// The value.
    /// </value>
    public object Value { get; set; }

    /// <summary>
    /// In inherited class, create a trigger that can be attached to a specific bindable object.
    /// </summary>
    /// <returns></returns>
    protected override AttachedTriggerBase CreateAttachedTrigger() => new AttachedDataTrigger();

    private class AttachedDataTrigger : AttachedTriggerBase
    {
        private readonly BindableProperty localProperty;
        private DataTrigger triggerSource;

        /// <summary>
        /// Constructor
        /// </summary>
        public AttachedDataTrigger()
        {
            localProperty = BindableProperty.CreateAttached("Bound", typeof(object), typeof(AttachedDataTrigger), null, propertyChanged: this.OnLocalPropertyChanged);
        }

        /// <summary>
        /// Called when a bindable object is attached to this trigger.
        /// </summary>
        protected override void OnAttachTo()
        {
            this.triggerSource = this.GetTypedTriggerSource<DataTrigger>();
            this.Bindable.SetBinding(localProperty, this.triggerSource.Binding.Clone());
            this.CheckIsActive(this.Bindable.GetValue(this.localProperty));
        }

        /// <summary>
        /// Called when bindable object is detached to this trigger.
        /// </summary>
        protected override void OnDetach()
        {
            this.Bindable.RemoveBinding(this.localProperty);
            this.Bindable.ClearValue(this.localProperty);
            this.triggerSource = null;
        }

        private void OnLocalPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            this.CheckIsActive(newValue);
        }

        private void CheckIsActive(object bindingValue)
        {
            if (this.triggerSource.Value == null && bindingValue == null)
            {
                this.IsActive = true;
                return;
            }

            if (this.triggerSource.bindingValueType == null)
                this.triggerSource.bindingValueType = bindingValue?.GetType();

            if (this.triggerSource.convertedValue == null && this.triggerSource.bindingValueType != null)
            {
                if (this.triggerSource.Value is string stringValue)
                    this.triggerSource.convertedValue = stringValue.ConvertFor(this.triggerSource.bindingValueType);
                else
                    this.triggerSource.convertedValue = this.triggerSource.Value;
            }

            if (object.Equals(this.triggerSource.convertedValue, bindingValue))
                this.IsActive = true;
            else
                this.IsActive = false;
        }
    }
}
