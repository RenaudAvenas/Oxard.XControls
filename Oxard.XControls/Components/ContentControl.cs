using Oxard.XControls.Effects;
using Oxard.XControls.Graphics;
using Xamarin.Forms;

namespace Oxard.XControls.Components
{
    /// <summary>
    /// Class that manage a content width template or template selector
    /// </summary>
    public class ContentControl : ContentView
    {
        /// <summary>
        /// Identifies the ContentTemplate dependency property.
        /// </summary>
        public static readonly BindableProperty ContentTemplateProperty = BindableProperty.Create(nameof(ContentTemplate), typeof(DataTemplate), typeof(ContentControl), propertyChanged: OnContentTemplatePropertyChanged);
        /// <summary>
        /// Identifies the ContentTemplateSelector dependency property.
        /// </summary>
        public static readonly BindableProperty ContentTemplateSelectorProperty = BindableProperty.Create(nameof(ContentTemplateSelector), typeof(DataTemplateSelector), typeof(ContentControl), propertyChanged: OnContentTemplateSelectorPropertyChanged);
        /// <summary>
        /// Identifies the Background dependency property.
        /// </summary>
        public static readonly BindableProperty BackgroundProperty = BindableProperty.Create(nameof(Background), typeof(Brush), typeof(ContentControl), Brushes.Transparent, propertyChanged: OnBackgroundPropertyChanged);
        /// <summary>
        /// Identifies the Foreground dependency property.
        /// </summary>
        public static readonly BindableProperty ForegroundProperty = BindableProperty.Create(nameof(Foreground), typeof(Color?), typeof(ContentControl));
        /// <summary>
        /// Identifies the BorderThickness dependency property.
        /// </summary>
        public static readonly BindableProperty BorderThicknessProperty = BindableProperty.Create(nameof(BorderThickness), typeof(double), typeof(ContentControl), 0d);
        /// <summary>
        /// Identifies the BorderColor dependency property.
        /// </summary>
        public static readonly BindableProperty BorderColorProperty = BindableProperty.Create(nameof(BorderColor), typeof(Color), typeof(ContentControl), Color.Transparent);
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
        /// Get or set the background of the ContentControl
        /// </summary>
        public Brush Background
        {
            get => (Brush)this.GetValue(BackgroundProperty);
            set => this.SetValue(BackgroundProperty, value);
        }

        /// <summary>
        /// Get or set the foreground of the ContentControl
        /// </summary>
        public Color? Foreground
        {
            get => (Color?)this.GetValue(ForegroundProperty);
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
        public Color BorderColor
        {
            get => (Color)this.GetValue(BorderColorProperty);
            set => this.SetValue(BorderColorProperty, value);
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
        /// Get or set a value that indicates if <see cref="Background"/> property is managed in the control template or not.
        /// Set false to use a <see cref="BackgroundEffect"/>.
        /// </summary>
        public bool IsBackgroundManagedByStyle
        {
            get => (bool)this.GetValue(IsBackgroundManagedByStyleProperty);
            set => this.SetValue(IsBackgroundManagedByStyleProperty, value);
        }

        private static void OnContentTemplatePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            (bindable as ContentControl).OnContentTemplateChanged();
        }

        private static void OnContentTemplateSelectorPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            (bindable as ContentControl).OnContentTemplateSelectorChanged();
        }

        private static void OnBackgroundPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            (bindable as ContentControl).OnBackgroundChanged(oldValue as Brush);
        }

        /// <summary>
        /// Called when Background property changed
        /// </summary>
        protected virtual void OnBackgroundChanged(Brush oldValue)
        {
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
