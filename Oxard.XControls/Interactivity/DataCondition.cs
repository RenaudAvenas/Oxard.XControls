using Oxard.XControls.Extensions;
using System;
using Xamarin.Forms;

namespace Oxard.XControls.Interactivity
{
    /// <summary>
    /// Describe and valid a condition on a binding
    /// </summary>
    public class DataCondition : Condition
    {
        private Type bindingValueType;
        private object convertedValue;

        /// <summary>
        /// Get or set the binding used to check condition
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
        /// In inherited class, create a condition that can be attached to a specific bindable object.
        /// </summary>
        /// <returns></returns>
        protected override AttachedCondition CreateAttachedCondition()
        {
            return new AttachedDataCondition();
        }

        private class AttachedDataCondition : AttachedCondition
        {
            private readonly BindableProperty localProperty;
            private DataCondition conditionSource;

            /// <summary>
            /// Constructor
            /// </summary>
            public AttachedDataCondition()
            {
                localProperty = BindableProperty.CreateAttached("Bound", typeof(object), typeof(AttachedDataCondition), null, propertyChanged: this.OnLocalPropertyChanged);
            }

            /// <summary>
            /// Called when a bindable object is attached to this trigger.
            /// </summary>
            protected override void OnAttachTo()
            {
                this.conditionSource = this.GetTypedConditionSource<DataCondition>();
                this.Bindable.SetBinding(localProperty, this.conditionSource.Binding.Clone());
                this.CheckIsValid(this.Bindable.GetValue(this.localProperty));
            }

            /// <summary>
            /// Called when bindable object is detached to this trigger.
            /// </summary>
            protected override void OnDetach()
            {
                this.Bindable.RemoveBinding(this.localProperty);
                this.Bindable.ClearValue(this.localProperty);
                this.conditionSource = null;
            }

            private void OnLocalPropertyChanged(BindableObject bindable, object oldValue, object newValue)
            {
                this.CheckIsValid(newValue);
            }

            private void CheckIsValid(object bindingValue)
            {
                if (this.conditionSource.Value == null && bindingValue == null)
                {
                    this.IsValid = true;
                    return;
                }

                if (this.conditionSource.bindingValueType == null)
                    this.conditionSource.bindingValueType = bindingValue?.GetType();

                if (this.conditionSource.convertedValue == null && this.conditionSource.bindingValueType != null)
                {
                    if (this.conditionSource.Value is string stringValue)
                        this.conditionSource.convertedValue = stringValue.ConvertFor(this.conditionSource.bindingValueType);
                    else
                        this.conditionSource.convertedValue = this.conditionSource.Value;
                }

                if (object.Equals(this.conditionSource.convertedValue, bindingValue) && !this.IsValid)
                    this.IsValid = true;
                else
                    this.IsValid = false;
            }
        }
    }
}
