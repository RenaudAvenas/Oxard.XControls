using System.Collections.Generic;
using System.ComponentModel;
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
        private bool isApplyingSetter;

        internal SetterValueCollection(BindableObject bindable, BindableProperty property)
        {
            this.bindable = bindable;
            this.property = property;

            this.bindable.PropertyChanged += OnBindablePropertyChanged;
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
            this.bindable.PropertyChanged -= OnBindablePropertyChanged;

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

            this.bindable.PropertyChanged += OnBindablePropertyChanged;
        }

        public void UnapplySetter(AttachedTriggerBase trigger, Setter setter)
        {
            this.bindable.PropertyChanged -= OnBindablePropertyChanged;

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

            this.bindable.PropertyChanged += OnBindablePropertyChanged;
        }

        private void OnBindablePropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            // We handle event only when changes is not caused by setters
            // So the value of the property is the original value
            // and we reapply last active setter
            if (e.PropertyName == this.property.PropertyName)
            {
                if (isApplyingSetter)
                {
                    isApplyingSetter = false;
                    return;
                }

                this.originalValue = this.bindable.GetValue(this.property);
                this.originalValueSet = true;

                var lastActiveTrigger = setters.Keys.LastOrDefault(t => t.IsActive);
                if (lastActiveTrigger == null)
                    return;

                var setter = lastActiveTrigger.Setters.Last(s => s.Property == this.property);
                if (Equals(setter.ConvertedValue, this.originalValue))
                    return;

                // Applying setter will call OnBindablePropertyChanged again just after this method ends.
                // We need to ignore the next change with a isApplyingSetter set to true
                isApplyingSetter = true;
                setter.Apply(this.bindable);
            }
        }
    }
}
