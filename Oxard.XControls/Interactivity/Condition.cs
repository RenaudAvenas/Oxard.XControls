using Xamarin.Forms;

namespace Oxard.XControls.Interactivity
{
    /// <summary>
    /// Base class for trigger condition
    /// </summary>
    public abstract class Condition
    {
        internal AttachedCondition AttachTo(BindableObject bindable)
        {
            var associatedCondition = this.CreateAttachedCondition();
            associatedCondition.AttachTo(this, bindable);
            return associatedCondition;
        }

        /// <summary>
        /// In inherited class, create a condition that can be attached to a specific bindable object.
        /// </summary>
        /// <returns></returns>
        protected abstract AttachedCondition CreateAttachedCondition();
    }
}
