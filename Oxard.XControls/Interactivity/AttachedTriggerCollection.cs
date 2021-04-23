using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace Oxard.XControls.Interactivity
{
    /// <summary>
    /// Manage a TriggerCollection for a specific bindable object
    /// </summary>
    internal class AttachedTriggerCollection
    {
        private static readonly BindableProperty SettersByBindableProperty = BindableProperty.CreateAttached("SettersByBindable", typeof(Dictionary<BindableProperty, SetterValueCollection>), typeof(AttachedTriggerCollection), null);
        private readonly List<AttachedTriggerBase> attachedTriggers = new List<AttachedTriggerBase>();
        private BindableObject attachedObject;

        internal void AttachTo(TriggerCollection collection, BindableObject bindable)
        {
            this.attachedObject = bindable;
            foreach (var trigger in collection)
            {
                var associatedTrigger = trigger.AttachTo(this.attachedObject);
                associatedTrigger.IsActiveChanged += this.OnTriggerIsActiveChanged;
                this.attachedTriggers.Add(associatedTrigger);

                foreach (var setter in associatedTrigger.Setters)
                {
                    var target = setter.Target ?? this.attachedObject;
                    var setterValueCollections = GetSettersByBindable(target);

                    if (!setterValueCollections.ContainsKey(setter.Property))
                        setterValueCollections[setter.Property] = new SetterValueCollection(target, setter.Property);

                    setterValueCollections[setter.Property].RegisterTrigger(associatedTrigger);

                    if (!associatedTrigger.IsActive)
                        continue;

                    setterValueCollections[setter.Property].ApplySetter(associatedTrigger, setter);
                }
            }
        }

        internal void DetachTo()
        {
            this.attachedObject = null;
            foreach (var trigger in this.attachedTriggers)
            {
                trigger.DetachTo();
                trigger.IsActiveChanged -= this.OnTriggerIsActiveChanged;

                foreach (var setter in trigger.Setters)
                {
                    var target = setter.Target ?? this.attachedObject;
                    var setterValueCollections = GetSettersByBindable(target);

                    setterValueCollections[setter.Property].UnregisterTrigger(trigger);
                    if (setterValueCollections[setter.Property].IsEmpty)
                        setterValueCollections.Remove(setter.Property);

                    if (setterValueCollections.Count == 0)
                        target.SetValue(SettersByBindableProperty, null);
                }
            }

            this.attachedTriggers.Clear();
        }

        private static Dictionary<BindableProperty, SetterValueCollection> GetSettersByBindable(BindableObject bindable)
        {
            var result = (Dictionary<BindableProperty, SetterValueCollection>)bindable.GetValue(SettersByBindableProperty);
            if (result == null)
            {
                result = new Dictionary<BindableProperty, SetterValueCollection>();
                bindable.SetValue(SettersByBindableProperty, result);
            }

            return result;
        }

        private void ApplyTrigger(AttachedTriggerBase triggerToApply)
        {
            foreach (var setter in triggerToApply.Setters)
            {
                var target = setter.Target ?? this.attachedObject;
                var setterValueCollections = GetSettersByBindable(target);

                setterValueCollections[setter.Property].ApplySetter(triggerToApply, setter);
            }
        }

        private void UnapplyTrigger(AttachedTriggerBase triggerToUnapply)
        {
            foreach (var setter in triggerToUnapply.Setters)
            {
                var target = setter.Target ?? this.attachedObject;
                var valuesByProperty = GetSettersByBindable(target);
                valuesByProperty[setter.Property].UnapplySetter(triggerToUnapply, setter);
            }
        }

        private void OnTriggerIsActiveChanged(object sender, EventArgs e)
        {
            var trigger = (AttachedTriggerBase)sender;
            if (trigger.IsActive)
                this.ApplyTrigger(trigger);
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
