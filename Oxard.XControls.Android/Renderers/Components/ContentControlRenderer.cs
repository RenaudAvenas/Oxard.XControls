using System.ComponentModel;
using Android.Content;
using Oxard.XControls.Components;
using Oxard.XControls.Droid.Events;
using Oxard.XControls.Droid.Renderers.Components;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Color = Xamarin.Forms.Color;

[assembly: ExportRenderer(typeof(ContentControl), typeof(ContentControlRenderer<ContentControl>))]

namespace Oxard.XControls.Droid.Renderers.Components
{
    public class ContentControlRenderer<T> : VisualElementRenderer<T> where T : ContentControl
    {
        private bool isDisposed;
        private BackgroundHelper backgroundHelper;

        public ContentControlRenderer(Context context)
            : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<T> e)
        {
            this.backgroundHelper?.Dispose();

            base.OnElementChanged(e);

            this.ApplyIsBackgroundManagedByStyle();
        }

        protected override void Dispose(bool disposing)
        {
            this.isDisposed = true;
            this.backgroundHelper?.Dispose();
            base.Dispose(disposing);
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            if (e.PropertyName == nameof(ContentControl.Background)
                || e.PropertyName == nameof(ContentControl.IsBackgroundManagedByStyle))
                this.ApplyIsBackgroundManagedByStyle();

        }

        public override void SetBackgroundColor(Android.Graphics.Color color)
        {
            if(this.Element.IsBackgroundManagedByStyle)
                base.SetBackgroundColor(color);
        }

        protected override void UpdateBackgroundColor()
        {
            if (this.Element.IsBackgroundManagedByStyle)
                base.UpdateBackgroundColor();
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
            {
                if (this.backgroundHelper == null)
                    this.backgroundHelper = new BackgroundHelper(((IVisualElementRenderer)this).View, this.Element, this.Element.Background);
                else
                    this.backgroundHelper.ChangeBackground(this.Element.Background);
            }
        }
    }
}