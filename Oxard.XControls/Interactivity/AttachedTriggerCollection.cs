using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;

namespace Oxard.XControls.Interactivity
{
    /// <summary>
    /// Manage a TriggerCollection for a specific bindable object
    /// </summary>
    internal class AttachedTriggerCollection
    {
        private readonly List<AttachedTriggerBase> attachedTriggers = new List<AttachedTriggerBase>();
        private readonly Dictionary<BindableProperty, object> originalPropertyValues = new Dictionary<BindableProperty, object>();
        private BindableObject attachedObject;
                
        internal void AttachTo(TriggerCollection collection, BindableObject bindable)
        {
            this.attachedObject = bindable;
            foreach (var trigger in collection)
            {
                var associatedTrigger = trigger.AttachTo(this.attachedObject);
                associatedTrigger.IsActiveChanged += this.OnTriggerIsActiveChanged;
                this.attachedTriggers.Add(associatedTrigger);
            }

            this.ApplyTriggers();
        }

        internal void DetachTo()
        {
            this.attachedObject = null;
            foreach (var trigger in this.attachedTriggers)
            {
                trigger.DetachTo();
                trigger.IsActiveChanged -= this.OnTriggerIsActiveChanged;
            }

            this.originalPropertyValues.Clear();
            this.attachedTriggers.Clear();
        }

        private void ApplyTriggers()
        {
            var settersToApplyByProperties = new Dictionary<BindableProperty, Setter>();
            foreach (var trigger in this.attachedTriggers)
            {
                if (!trigger.IsActive)
                    continue;

                foreach (var setter in trigger.Setters)
                    settersToApplyByProperties[setter.Property] = setter;
            }

            foreach (var setter in settersToApplyByProperties.Values)
            {
                if (!this.originalPropertyValues.ContainsKey(setter.Property))
                    this.originalPropertyValues[setter.Property] = this.attachedObject.GetValue(setter.Property);

                setter.Apply(this.attachedObject);
            }
        }

        private void UnapplyTrigger(AttachedTriggerBase triggerToUnapply)
        {
            var impactedProperties = triggerToUnapply.Setters.ToDictionary(s => s.Property, s => this.originalPropertyValues[s.Property]);
            var settersToApplyByProperties = new Dictionary<BindableProperty, Setter>();

            foreach (var trigger in this.attachedTriggers)
            {
                if (!trigger.IsActive)
                    continue;

                foreach (var setter in trigger.Setters)
                {
                    if (impactedProperties.ContainsKey(setter.Property))
                    {
                        settersToApplyByProperties[setter.Property] = setter;
                        impactedProperties.Remove(setter.Property);
                    }
                }
            }

            foreach (var setter in settersToApplyByProperties.Values)
                setter.Apply(this.attachedObject);
            foreach (var returnToOriginKeyValuePair in impactedProperties)
                this.attachedObject.SetValue(returnToOriginKeyValuePair.Key, returnToOriginKeyValuePair.Value);

        }

        private void OnTriggerIsActiveChanged(object sender, System.EventArgs e)
        {
            var trigger = (AttachedTriggerBase)sender;
            if (trigger.IsActive)
                this.ApplyTriggers();
            else
                this.UnapplyTrigger(trigger);
        }
    }
}
