using Oxard.XControls.Graphics;
using Xamarin.Forms;

namespace Oxard.XControls.Components
{
    public class ContentControl : ContentView
    {
        public static readonly BindableProperty ContentTemplateProperty = BindableProperty.Create(nameof(ContentTemplate), typeof(DataTemplate), typeof(ContentControl), propertyChanged: OnContentTemplatePropertyChanged);
        public static readonly BindableProperty ContentTemplateSelectorProperty = BindableProperty.Create(nameof(ContentTemplateSelector), typeof(DataTemplateSelector), typeof(ContentControl), propertyChanged: OnContentTemplateSelectorPropertyChanged);
        public static readonly BindableProperty BackgroundProperty = BindableProperty.Create(nameof(Background), typeof(Brush), typeof(ContentControl), Brushes.Transparent);
        public static readonly BindableProperty ForegroundProperty = BindableProperty.Create(nameof(Foreground), typeof(Color?), typeof(ContentControl));
        public static readonly BindableProperty BorderThicknessProperty = BindableProperty.Create(nameof(BorderThickness), typeof(double), typeof(ContentControl), 0d);
        public static readonly BindableProperty BorderColorProperty = BindableProperty.Create(nameof(BorderColor), typeof(Color), typeof(ContentControl), Color.Transparent);
        public static readonly BindableProperty HorizontalContentOptionsProperty = BindableProperty.Create(nameof(HorizontalContentOptions), typeof(LayoutOptions), typeof(ContentControl), LayoutOptions.Fill);
        public static readonly BindableProperty VerticalContentOptionsProperty = BindableProperty.Create(nameof(VerticalContentOptions), typeof(LayoutOptions), typeof(ContentControl), LayoutOptions.Fill);

        public DataTemplate ContentTemplate
        {
            get => (DataTemplate)this.GetValue(ContentTemplateProperty);
            set => this.SetValue(ContentTemplateProperty, value);
        }

        public DataTemplateSelector ContentTemplateSelector
        {
            get => (DataTemplateSelector)this.GetValue(ContentTemplateSelectorProperty);
            set => this.SetValue(ContentTemplateSelectorProperty, value);
        }

        public Brush Background
        {
            get => (Brush)this.GetValue(BackgroundProperty);
            set => this.SetValue(BackgroundProperty, value);
        }

        public Color? Foreground
        {
            get => (Color?)this.GetValue(ForegroundProperty);
            set => this.SetValue(ForegroundProperty, value);
        }

        public double BorderThickness
        {
            get => (double)this.GetValue(BorderThicknessProperty);
            set => this.SetValue(BorderThicknessProperty, value);
        }

        public Color BorderColor
        {
            get => (Color)this.GetValue(BorderColorProperty);
            set => this.SetValue(BorderColorProperty, value);
        }

        public LayoutOptions HorizontalContentOptions
        {
            get => (LayoutOptions)this.GetValue(HorizontalContentOptionsProperty);
            set => this.SetValue(HorizontalContentOptionsProperty, value);
        }

        public LayoutOptions VerticalContentOptions
        {
            get => (LayoutOptions)this.GetValue(VerticalContentOptionsProperty);
            set => this.SetValue(VerticalContentOptionsProperty, value);
        }

        private static void OnContentTemplatePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            (bindable as ContentControl).OnContentTemplateChanged();
        }

        private static void OnContentTemplateSelectorPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            (bindable as ContentControl).OnContentTemplateSelectorChanged();
        }

        private void OnContentTemplateChanged()
        {
            if (this.ContentTemplate == null && this.ContentTemplateSelector == null)
            {
                this.Content = null;
                return;
            }

            // ContentTemplateSelector property has priority
            if (this.ContentTemplateSelector != null)
                return;

            var content = (View)this.ContentTemplate.CreateContent();
            content.BindingContext = this.Content;

            this.Content = content;
        }

        private void OnContentTemplateSelectorChanged()
        {
            if (this.ContentTemplate == null && this.ContentTemplateSelector == null)
            {
                this.Content = null;
                return;
            }

            var content = (View)this.ContentTemplateSelector.SelectTemplate(this.Content, this).CreateContent();
            content.BindingContext = this.Content;

            this.Content = content;
        }
    }
}
