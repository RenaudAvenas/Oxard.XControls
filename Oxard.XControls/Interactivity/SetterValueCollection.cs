using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;

namespace Oxard.XControls.Interactivity
{
    internal class SetterValueCollection
    {
        private readonly Dictionary<AttachedTriggerBase, List<Setter>> setters = new Dictionary<AttachedTriggerBase, List<Setter>>();
        private readonly BindableObject bindable;
        private readonly BindableProperty property;
        private object originalValue;
        private bool originalValueSet;

        internal SetterValueCollection(BindableObject bindable, BindableProperty property)
        {
            this.bindable = bindable;
            this.property = property;
        }

        internal bool IsEmpty => setters.Count == 0;

        public void RegisterTrigger(AttachedTriggerBase trigger)
        {
            if (setters.ContainsKey(trigger))
                return;

            setters[trigger] = null;
        }

        public void UnregisterTrigger(AttachedTriggerBase trigger)
        {
            var settersToUnapply = setters[trigger];
            if(settersToUnapply != null)
                settersToUnapply.ForEach(s => this.UnapplySetter(trigger, s));

            setters.Remove(trigger);
        }

        public void ApplySetter(AttachedTriggerBase trigger, Setter setter)
        {
            var lastActiveTrigger = setters.Keys.Last(t => t.IsActive);
            bool affectsValue = lastActiveTrigger == trigger;

            if (!this.originalValueSet)
            {
                this.originalValue = this.bindable.GetValue(this.property);
                this.originalValueSet = true;
            }

            if (setters[trigger] == null)
                setters[trigger] = new List<Setter>();

            setters[trigger].Add(setter);

            if (affectsValue)
                setter.Apply(this.bindable);
        }

        public void UnapplySetter(AttachedTriggerBase trigger, Setter setter)
        {
            var lastActiveTrigger = setters.Keys.LastOrDefault(t => t.IsActive);
            bool affectsValue = lastActiveTrigger != trigger;

            setters[trigger].Remove(setter);
            if (setters[trigger].Count == 0)
                setters[trigger] = null;

            if (affectsValue)
            {
                if (lastActiveTrigger == null)
                    this.bindable.SetValue(this.property, this.originalValue);
                else
                    setters[lastActiveTrigger].Last().Apply(this.bindable);
            }
        }
    }
}
