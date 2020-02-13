using System.ComponentModel;
using Xamarin.Forms;

namespace Oxard.XControls.Interactivity
{
    /// <summary>
    /// Describe and valid a condition on a bindable property
    /// </summary>
    public class PropertyCondition : Condition
    {
        private object convertedValue;

        /// <summary>
        /// Gets or sets the property where condition is applied.
        /// </summary>
        /// <value>
        /// The property.
        /// </value>
        public BindableProperty Property { get; set; }

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
            return new AttachedPropertyCondition();
        }

        private class AttachedPropertyCondition : AttachedCondition
        {
            private PropertyCondition conditionSource;

            /// <summary>
            /// Called when a bindable object is attached to this condition.
            /// </summary>
            protected override void OnAttachTo()
            {
                this.conditionSource = this.GetTypedConditionSource<PropertyCondition>();
                this.Bindable.PropertyChanged += this.OnBindablePropertyChanged;
                this.CheckIsValid();
            }

            /// <summary>
            /// Called when bindable object is detached to this condition.
            /// </summary>
            protected override void OnDetach()
            {
                this.Bindable.PropertyChanged -= this.OnBindablePropertyChanged;
                this.conditionSource = null;
            }

            private void OnBindablePropertyChanged(object sender, PropertyChangedEventArgs e)
            {
                if (e.PropertyName == this.conditionSource.Property.PropertyName)
                {
                    this.CheckIsValid();
                }
            }

            private void CheckIsValid()
            {
                if (this.conditionSource.convertedValue == null && this.conditionSource.Value != null)
                {
                    if (this.conditionSource.Value is string stringValue)
                        this.conditionSource.convertedValue = stringValue.ConvertFor(this.conditionSource.Property.ReturnType);
                    else
                        this.conditionSource.convertedValue = this.conditionSource.Value;
                }

                if (object.Equals(this.conditionSource.convertedValue, this.Bindable.GetValue(this.conditionSource.Property)))
                    this.IsValid = true;
                else
                    this.IsValid = false;
            }
        }
    }
}
