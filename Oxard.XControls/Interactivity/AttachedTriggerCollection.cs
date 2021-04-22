using System;
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
        private readonly Dictionary<BindablePropertyValueExtension, object> originalPropertyValues = new Dictionary<BindablePropertyValueExtension, object>();
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
            var settersToApplyByProperties = new Dictionary<BindablePropertyValueExtension, Setter>();
            foreach (var trigger in this.attachedTriggers)
            {
                if (!trigger.IsActive)
                    continue;

                foreach (var setter in trigger.Setters)
                    settersToApplyByProperties[new BindablePropertyValueExtension(setter.Target ?? this.attachedObject, setter.Property)] = setter;
            }

            foreach (var kvp in settersToApplyByProperties)
            {
                if (!this.originalPropertyValues.ContainsKey(kvp.Key))
                    this.originalPropertyValues[kvp.Key] = kvp.Key.Value;

                kvp.Value.Apply(kvp.Value.Target ?? this.attachedObject);
            }
        }

        private void UnapplyTrigger(AttachedTriggerBase triggerToUnapply)
        {
            var impactedProperties = triggerToUnapply.Setters.ToDictionary(s => new BindablePropertyValueExtension(s.Target ?? this.attachedObject, s.Property), s =>
            {
                this.originalPropertyValues.GetIfContains(k => k.Target == (s.Target ?? this.attachedObject) && k.Property == s.Property, out var result);
                return result.Value;
            });

            var settersToApplyByProperties = new Dictionary<BindablePropertyValueExtension, Setter>();

            foreach (var trigger in this.attachedTriggers)
            {
                if (!trigger.IsActive)
                    continue;

                foreach (var setter in trigger.Setters)
                {
                    if (impactedProperties.GetIfContains(k => k.Target == (setter.Target ?? this.attachedObject) && k.Property == setter.Property, out var kvp))
                    {
                        settersToApplyByProperties[kvp.Key] = setter;
                        impactedProperties.Remove(kvp.Key);
                    }
                }
            }

            foreach (var setter in settersToApplyByProperties.Values)
                setter.Apply(setter.Target ?? this.attachedObject);
            foreach (var returnToOriginKeyValuePair in impactedProperties)
                returnToOriginKeyValuePair.Key.Value = returnToOriginKeyValuePair.Value;

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

    internal static class DictionaryBindablePropertyValueExtension
    {
        internal static bool GetIfContains<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, Func<TKey, bool> condition, out KeyValuePair<TKey, TValue> kvpResult)
        {
            foreach (var kvp in dictionary)
            {
                if (condition(kvp.Key))
                {
                    kvpResult = kvp;
                    return true;
                }
            }

            kvpResult = default;
            return false;
        }
    }
}
