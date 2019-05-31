using System;
using System.ComponentModel;
using Android.Content;
using Oxard.XControls.Components;
using Oxard.XControls.Droid.Events;
using Oxard.XControls.Droid.Renderers.Components;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(ContentControl), typeof(ContentControlRenderer<ContentControl>))]

namespace Oxard.XControls.Droid.Renderers.Components
{
    public class ContentControlRenderer<T> : VisualElementRenderer<T> where T : ContentControl
    {
        private BackgroundHelper backgroundHelper;

        public ContentControlRenderer(Context context) 
            : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<T> e)
        {
            base.OnElementChanged(e);
            this.ApplyIsBackgroundManagedByStyle();
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            if (e.PropertyName == nameof(ContentControl.Background) && this.backgroundHelper != null)
                this.backgroundHelper.ChangeBackground(this.Element.Background);
            else if (e.PropertyName == nameof(ContentControl.IsBackgroundManagedByStyle))
                this.ApplyIsBackgroundManagedByStyle();
        }

        private void ApplyIsBackgroundManagedByStyle()
        {
            if (this.Element.IsBackgroundManagedByStyle)
            {
                this.backgroundHelper?.Dispose();
                this.backgroundHelper = null;
                ((IVisualElementRenderer)this).View.SetBackgroundColor(Color.Transparent.ToAndroid());
            }
            else
                this.backgroundHelper = new BackgroundHelper(((IVisualElementRenderer)this).View, this.Element, this.Element.Background);
        }
    }
}