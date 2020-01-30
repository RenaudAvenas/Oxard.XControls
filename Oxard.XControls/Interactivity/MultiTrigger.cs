using System.Collections.Generic;

namespace Oxard.XControls.Interactivity
{
    /// <summary>
    /// Class that describe behavior on multiple condition
    /// </summary>
    public class MultiTrigger : TriggerBase
    {
        /// <summary>
        /// Get the conditions that activate this multi trigger
        /// </summary>
        public List<Condition> Conditions { get; } = new List<Condition>();

        /// <summary>
        /// In inherited class, create a trigger that can be attached to a specific bindable object.
        /// </summary>
        /// <returns></returns>
        protected override AttachedTriggerBase CreateAttachedTrigger()
        {
            return new AttachedMultiTrigger();
        }

        private class AttachedMultiTrigger : AttachedTriggerBase
        {
            private readonly List<AttachedCondition> attachedConditions = new List<AttachedCondition>();
            private MultiTrigger triggerSource;
            private int validConditionCount;

            protected override void OnAttachTo()
            {
                triggerSource = this.GetTypedTriggerSource<MultiTrigger>();
                foreach (var condition in triggerSource.Conditions)
                {
                    var associatedCondition = condition.AttachTo(this.Bindable);
                    associatedCondition.ConditionChanged += this.OnConditionChanged;
                    this.attachedConditions.Add(associatedCondition);

                    if (associatedCondition.IsValid)
                        validConditionCount++;
                }

                this.IsActive = validConditionCount == this.triggerSource.Conditions.Count;
            }

            protected override void OnDetach()
            {
                foreach (var condition in this.attachedConditions)
                {
                    condition.DetachTo();
                    condition.ConditionChanged -= this.OnConditionChanged;
                }

                this.attachedConditions.Clear();
                this.triggerSource = null;
                this.validConditionCount = 0;
            }

            private void OnConditionChanged(object sender, System.EventArgs e)
            {
                var condition = (AttachedCondition)sender;
                if (condition.IsValid)
                    this.validConditionCount++;
                else
                    this.validConditionCount--;

                this.IsActive = validConditionCount == this.triggerSource.Conditions.Count;
            }
        }
    }
}
