namespace Oxard.Maui.XControls.Interactivity;

/// <summary>
/// Manage <see cref="Condition"/> for a specific bindable object
/// </summary>
public abstract class AttachedCondition
{
    private bool isValid;

    internal event EventHandler ConditionChanged;
    
    /// <summary>
    /// Gets the condition that instanciate this instance.
    /// </summary>
    /// <value>
    /// The condition source.
    /// </value>
    protected Condition ConditionSource { get; private set; }

    /// <summary>
    /// Gets the attached bindable for this condition instance.
    /// </summary>
    /// <value>
    /// The bindable.
    /// </value>
    protected BindableObject Bindable { get; private set; }

    /// <summary>
    /// Gets or sets a value indicating whether this condition is valid.
    /// </summary>
    /// <value>
    ///   <c>true</c> if this instance is valid; otherwise, <c>false</c>.
    /// </value>
    public bool IsValid
    {
        get => this.isValid;
        protected set
        {
            if (this.isValid != value)
            {
                this.isValid = value;
                this.ConditionChanged?.Invoke(this, EventArgs.Empty);
            }
        }
    }

    internal void AttachTo(Condition source, BindableObject bindable)
    {
        this.ConditionSource = source;
        this.Bindable = bindable;
        this.OnAttachTo();
    }

    internal void DetachTo()
    {
        this.OnDetach();
        this.Bindable = null;
    }

    /// <summary>
    /// Called when a bindable object is attached to this condition.
    /// </summary>
    protected abstract void OnAttachTo();

    /// <summary>
    /// Called when bindable object is detached to this condition.
    /// </summary>
    protected abstract void OnDetach();

    /// <summary>
    /// Gets the condition source casted in specific type <typeparamref name="TCondition"/>.
    /// </summary>
    /// <typeparam name="TCondition">The type of the condition.</typeparam>
    /// <returns></returns>
    protected TCondition GetTypedConditionSource<TCondition>() where TCondition : Condition
    {
        return (TCondition)this.ConditionSource;
    }
}
