namespace Oxard.Maui.XControls.Components;

/// <summary>
/// Selectable item in a <see cref="ListBox"/>
/// </summary>
public class ListBoxItem : LongPressButton
{
    /// <summary>
    /// Identifies the IsSelected dependency property.
    /// </summary>
    public static readonly BindableProperty IsSelectedProperty = BindableProperty.Create(nameof(IsSelected), typeof(bool), typeof(ListBoxItem), false, defaultBindingMode: BindingMode.TwoWay, propertyChanged: OnIsSelectedPropertyChanged);

    /// <summary>
    /// Get or set a value indicates if the item is selected
    /// </summary>
    public bool IsSelected
    {
        get => (bool)this.GetValue(IsSelectedProperty);
        set => this.SetValue(IsSelectedProperty, value);
    }

    internal ListBox ListBox { get; set; }

    private static void OnIsSelectedPropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        (bindable as ListBoxItem)?.OnIsSelectedChanged();
    }

    /// <summary>
    /// Called when <see cref="IsSelected"/> changed
    /// </summary>
    protected virtual void OnIsSelectedChanged()
    {
    }

    /// <summary>
    /// Invoke <see cref="Button.Clicked"/> event and call Execute method of <see cref="Command"/> property
    /// </summary>
    protected override void OnClicked()
    {
        ListBox.SelectedItem = this.BindingContext;
        base.OnClicked();
    }

    /// <summary>
    /// Invoke the <see cref="LongPressButton.LongPressed"/> event.
    /// </summary>
    protected override void OnLongPressed()
    {
        if (LongPressedCommandParameter == null)
            LongPressedCommandParameter = this.BindingContext;

        base.OnLongPressed();
    }
}
