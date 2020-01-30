using System.ComponentModel;
using Xamarin.Forms;

namespace Oxard.XControls.Interactivity
{
    /// <summary>
    /// Class that describe behavior on bindable property changed
    /// </summary>
    public class Trigger : TriggerBase
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
        /// In inherited class, create a trigger that can be attached to a specific bindable object.
        /// </summary>
        /// <returns></returns>
        protected override AttachedTriggerBase CreateAttachedTrigger()
        {
            return new AttachedTrigger();
        }

        private class AttachedTrigger : AttachedTriggerBase
        {
            private Trigger triggerSource;

            /// <summary>
            /// Called when a bindable object is attached to this trigger.
            /// </summary>
            protected override void OnAttachTo()
            {
                this.triggerSource = this.GetTypedTriggerSource<Trigger>();
                this.Bindable.PropertyChanged += this.OnBindablePropertyChanged;
                this.CheckIsActive();
            }

            /// <summary>
            /// Called when bindable object is detached to this trigger.
            /// </summary>
            protected override void OnDetach()
            {
                this.Bindable.PropertyChanged -= this.OnBindablePropertyChanged;
                this.triggerSource = null;
            }

            private void OnBindablePropertyChanged(object sender, PropertyChangedEventArgs e)
            {
                if (e.PropertyName == this.triggerSource.Property.PropertyName)
                {
                    this.CheckIsActive();
                }
            }

            private void CheckIsActive()
            {
                if (this.triggerSource.convertedValue == null && this.triggerSource.Value != null)
                {
                    if (this.triggerSource.Value is string stringValue)
                        this.triggerSource.convertedValue = stringValue.ConvertFor(this.triggerSource.Property.ReturnType);
                    else
                        this.triggerSource.convertedValue = this.triggerSource.Value;
                }

                if (object.Equals(this.triggerSource.convertedValue, this.Bindable.GetValue(this.triggerSource.Property)) && !this.IsActive)
                    this.IsActive = true;
                else
                    this.IsActive = false;
            }
        }
    }
}
