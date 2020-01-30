using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace Oxard.XControls.Interactivity
{
    /// <summary>
    /// Manage <see cref="TriggerBase"/> for a specific bindable object
    /// </summary>
    public abstract class AttachedTriggerBase
    {
        private bool isActive;

        internal event EventHandler IsActiveChanged;

        internal IEnumerable<Setter> Setters => this.TriggerSource.Setters;

        /// <summary>
        /// Gets the trigger that instanciate this instance.
        /// </summary>
        /// <value>
        /// The trigger source.
        /// </value>
        protected TriggerBase TriggerSource { get; private set; }

        /// <summary>
        /// Gets the attached bindable for this trigger instance.
        /// </summary>
        /// <value>
        /// The bindable.
        /// </value>
        protected BindableObject Bindable { get; private set; }

        /// <summary>
        /// Gets or sets a value indicating whether this trigger is active.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is active; otherwise, <c>false</c>.
        /// </value>
        public bool IsActive
        {
            get => this.isActive;
            protected set
            {
                if (this.isActive != value)
                {
                    this.isActive = value;
                    this.IsActiveChanged?.Invoke(this, EventArgs.Empty);
                }
            }
        }

        internal void AttachTo(TriggerBase source, BindableObject bindable)
        {
            this.TriggerSource = source;
            this.Bindable = bindable;
            this.OnAttachTo();
        }

        internal void DetachTo()
        {
            this.OnDetach();
            this.Bindable = null;
        }

        /// <summary>
        /// Called when a bindable object is attached to this trigger.
        /// </summary>
        protected abstract void OnAttachTo();

        /// <summary>
        /// Called when bindable object is detached to this trigger.
        /// </summary>
        protected abstract void OnDetach();

        /// <summary>
        /// Gets the trigger source casted in specific type <typeparamref name="TTrigger"/>.
        /// </summary>
        /// <typeparam name="TTrigger">The type of the trigger.</typeparam>
        /// <returns></returns>
        protected TTrigger GetTypedTriggerSource<TTrigger>() where TTrigger : TriggerBase
        {
            return (TTrigger)this.TriggerSource; 
        }
    }
}
