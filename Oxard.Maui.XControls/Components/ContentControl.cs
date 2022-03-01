namespace Oxard.Maui.XControls.Components;

/// <summary>
/// Class that manage a content width template or template selector
/// </summary>
public class ContentControl : ContentView
{
    private bool needReapplyContentDataTemplate;

    /// <summary>
    /// Identifies the ContentData dependency property.
    /// </summary>
    public static readonly BindableProperty ContentDataProperty = BindableProperty.Create(nameof(ContentData), typeof(object), typeof(ContentControl), propertyChanged: OnContentDataPropertyChanged);
    /// <summary>
    /// Identifies the ContentTemplate dependency property.
    /// </summary>
    public static readonly BindableProperty ContentTemplateProperty = BindableProperty.Create(nameof(ContentTemplate), typeof(DataTemplate), typeof(ContentControl), propertyChanged: OnContentTemplatePropertyChanged);
    /// <summary>
    /// Identifies the ContentTemplateSelector dependency property.
    /// </summary>
    public static readonly BindableProperty ContentTemplateSelectorProperty = BindableProperty.Create(nameof(ContentTemplateSelector), typeof(DataTemplateSelector), typeof(ContentControl), propertyChanged: OnContentTemplateSelectorPropertyChanged);
    /// <summary>
    /// Identifies the Foreground dependency property.
    /// </summary>
    public static readonly BindableProperty ForegroundProperty = BindableProperty.Create(nameof(Foreground), typeof(Color), typeof(ContentControl));
    /// <summary>
    /// Identifies the BorderThickness dependency property.
    /// </summary>
    public static readonly BindableProperty BorderThicknessProperty = BindableProperty.Create(nameof(BorderThickness), typeof(double), typeof(ContentControl), 0d);
    /// <summary>
    /// Identifies the BorderColor dependency property.
    /// </summary>
    public static readonly BindableProperty BorderBrushProperty = BindableProperty.Create(nameof(BorderBrush), typeof(Brush), typeof(ContentControl), Brush.Transparent);
    /// <summary>
    /// Identifies the HorizontalContentOptions dependency property.
    /// </summary>
    public static readonly BindableProperty HorizontalContentOptionsProperty = BindableProperty.Create(nameof(HorizontalContentOptions), typeof(LayoutOptions), typeof(ContentControl), LayoutOptions.Fill);
    /// <summary>
    /// Identifies the VerticalContentOptions dependency property.
    /// </summary>
    public static readonly BindableProperty VerticalContentOptionsProperty = BindableProperty.Create(nameof(VerticalContentOptions), typeof(LayoutOptions), typeof(ContentControl), LayoutOptions.Fill);
    /// <summary>
    /// Identifies the IsBackgroundManagedByStyle dependency property.
    /// </summary>
    public static readonly BindableProperty IsBackgroundManagedByStyleProperty = BindableProperty.Create(nameof(IsBackgroundManagedByStyle), typeof(bool), typeof(ContentControl), false);

    /// <summary>
    /// Gets or sets the content data to display (affect Content by ContentTemplate or ContentTemplateSelector property).
    /// </summary>
    /// <value>
    /// The content data.
    /// </value>
    public object ContentData
    {
        get => this.GetValue(ContentDataProperty);
        set => this.SetValue(ContentDataProperty, value);
    }

    /// <summary>
    /// Get or set the data template used to display content
    /// </summary>
    public DataTemplate ContentTemplate
    {
        get => (DataTemplate)this.GetValue(ContentTemplateProperty);
        set => this.SetValue(ContentTemplateProperty, value);
    }

    /// <summary>
    /// Get or set the data template selector used to display content
    /// </summary>
    public DataTemplateSelector ContentTemplateSelector
    {
        get => (DataTemplateSelector)this.GetValue(ContentTemplateSelectorProperty);
        set => this.SetValue(ContentTemplateSelectorProperty, value);
    }

    /// <summary>
    /// Get or set the foreground of the ContentControl
    /// </summary>
    public Color Foreground
    {
        get => (Color)this.GetValue(ForegroundProperty);
        set => this.SetValue(ForegroundProperty, value);
    }

    /// <summary>
    /// Get or set the border thickness
    /// </summary>
    public double BorderThickness
    {
        get => (double)this.GetValue(BorderThicknessProperty);
        set => this.SetValue(BorderThicknessProperty, value);
    }

    /// <summary>
    /// Get or set the border color
    /// </summary>
    public Brush BorderBrush
    {
        get => (Brush)this.GetValue(BorderBrushProperty);
        set => this.SetValue(BorderBrushProperty, value);
    }

    /// <summary>
    /// Get or set the horizontal layout options used to display the content
    /// </summary>
    public LayoutOptions HorizontalContentOptions
    {
        get => (LayoutOptions)this.GetValue(HorizontalContentOptionsProperty);
        set => this.SetValue(HorizontalContentOptionsProperty, value);
    }

    /// <summary>
    /// Get or set the vertical layout options used to display the content
    /// </summary>
    public LayoutOptions VerticalContentOptions
    {
        get => (LayoutOptions)this.GetValue(VerticalContentOptionsProperty);
        set => this.SetValue(VerticalContentOptionsProperty, value);
    }

    /// <summary>
    /// Get or set a value that indicates if <see cref="VisualElement.Background"/> property is managed in the control template or not.
    /// </summary>
    public bool IsBackgroundManagedByStyle
    {
        get => (bool)this.GetValue(IsBackgroundManagedByStyleProperty);
        set => this.SetValue(IsBackgroundManagedByStyleProperty, value);
    }

    /// <summary>
    /// Method called when BindingContext changed.
    /// </summary>
    protected override void OnBindingContextChanged()
    {
        base.OnBindingContextChanged();
        if (this.needReapplyContentDataTemplate)
        {
            this.needReapplyContentDataTemplate = false;
            this.OnContentDataChanged();
        }
    }

    private static void OnContentTemplatePropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        (bindable as ContentControl)?.OnContentTemplateChanged();
    }

    private static void OnContentTemplateSelectorPropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        (bindable as ContentControl)?.OnContentTemplateSelectorChanged();
    }

    private static void OnContentDataPropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        (bindable as ContentControl)?.OnContentDataChanged();
    }

    private void OnContentDataChanged()
    {
        if (this.ContentTemplate != null)
            this.OnContentTemplateChanged();
        else if (this.ContentTemplateSelector != null)
            this.OnContentTemplateSelectorChanged();
    }

    private void OnContentTemplateChanged()
    {
        if (this.BindingContext == null)
        {
            this.needReapplyContentDataTemplate = true;
            return;
        }

        if (this.ContentTemplate == null && this.ContentTemplateSelector == null)
        {
            this.Content = null;
            return;
        }

        // ContentTemplateSelector property has priority
        if (this.ContentTemplateSelector != null)
            return;

        var content = (View)this.ContentTemplate.CreateContent();
        content.BindingContext = this.ContentData;

        this.Content = content;
    }

    private void OnContentTemplateSelectorChanged()
    {
        if (this.BindingContext == null)
        {
            this.needReapplyContentDataTemplate = true;
            return;
        }

        if (this.ContentTemplate == null && this.ContentTemplateSelector == null)
        {
            this.Content = null;
            this.needReapplyContentDataTemplate = true;
            return;
        }

        var content = (View)this.ContentTemplateSelector.SelectTemplate(this.ContentData, this).CreateContent();
        content.BindingContext = this.ContentData;

        this.Content = content;
    }
}
