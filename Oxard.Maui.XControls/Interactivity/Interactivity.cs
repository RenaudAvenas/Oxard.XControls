namespace Oxard.Maui.XControls.Interactivity;

/// <summary>
/// Allow you to attached a collection of triggers on bindable object
/// </summary>
public static class Interactivity
{
    /// <summary>
    /// Identifies the Triggers dependency property.
    /// </summary>
    public static readonly BindableProperty TriggersProperty = BindableProperty.CreateAttached("Triggers", typeof(TriggerCollection), typeof(Interactivity), null, propertyChanged: OnTriggersPropertyChanged);
    
    /// <summary>
    /// Get the Triggers property value for the specified bindable object
    /// </summary>
    /// <param name="bindableObject">Object on which we want the value of the property</param>
    /// <returns>Triggers property value for the bindable object</returns>
    public static TriggerCollection GetTriggers(BindableObject bindableObject) => (TriggerCollection)bindableObject.GetValue(TriggersProperty);

    /// <summary>
    /// Set the Triggers property value for the specified bindable object
    /// </summary>
    /// <param name="bindableObject">Object that takes the value</param>
    /// <param name="value">The value to affect</param>
    public static void SetTriggers(BindableObject bindableObject, TriggerCollection value) => bindableObject.SetValue(TriggersProperty, value);
    
    private static void OnTriggersPropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (oldValue != null)
            (oldValue as TriggerCollection)?.DetachTo();
        if (newValue is TriggerCollection triggerCollection)
            triggerCollection.AttachTo(bindable);
    }
}
