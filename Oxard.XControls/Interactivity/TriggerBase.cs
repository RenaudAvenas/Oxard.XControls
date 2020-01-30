using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace Oxard.XControls.Interactivity
{
    /// <summary>
    /// Base class for Oxard triggers
    /// </summary>
    [ContentProperty(nameof(Setters))]
    public abstract class TriggerBase
    {
        /// <summary>
        /// Gets the setters that will be applied if trigger's conditions are valid.
        /// </summary>
        /// <value>
        /// The setters.
        /// </value>
        public List<Setter> Setters { get; } = new List<Setter>();

        internal AttachedTriggerBase AttachTo(BindableObject bindable)
        {
            var associatedTrigger = this.CreateAttachedTrigger();
            associatedTrigger.AttachTo(this, bindable);
            return associatedTrigger;
        }

        /// <summary>
        /// In inherited class, create a trigger that can be attached to a specific bindable object.
        /// </summary>
        /// <returns></returns>
        protected abstract AttachedTriggerBase CreateAttachedTrigger();
    }
}
