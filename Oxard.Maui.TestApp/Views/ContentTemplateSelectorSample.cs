namespace Oxard.Maui.TestApp.Views;
public class ContentTemplateSelectorSample : DataTemplateSelector
{
    public DataTemplate NullTemplate { get; set; }

    public DataTemplate TrueTemplate { get; set; }

    public DataTemplate FalseTemplate { get; set; }

    protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
    {
        var value = (bool?)item;

        if (!value.HasValue)
            return this.NullTemplate;

        return value.Value ? TrueTemplate : FalseTemplate;
    }
}
