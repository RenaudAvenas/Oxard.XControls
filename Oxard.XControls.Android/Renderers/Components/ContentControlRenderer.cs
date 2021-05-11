using Android.Content;
using Oxard.XControls.Components;
using Oxard.XControls.Droid.Graphics;
using Oxard.XControls.Droid.Renderers.Components;
using Oxard.XControls.Graphics;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(ContentControl), typeof(ContentControlRenderer<ContentControl>))]

namespace Oxard.XControls.Droid.Renderers.Components
{
    public class ContentControlRenderer<T> : VisualElementRenderer<T> where T : ContentControl
    {
        private DrawingBrushDrawable drawableBackground;

        public ContentControlRenderer(Context context)
            : base(context)
        {
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            if (e.PropertyName == nameof(this.Element.IsBackgroundManagedByStyle))
                this.UpdateBackground();
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            this.DisposeDrawableBackground();
        }

        protected override void UpdateBackground()
        {
            if (this.Element.IsBackgroundManagedByStyle)
            {
                this.DisposeDrawableBackground();
                this.SetBackground(null);
                return;
            }

            if (this.Element.Background is DrawingBrush drawingBrush)
                this.drawableBackground = this.UpdateBackground(this.Element, drawingBrush);
            else
            {
                base.UpdateBackground(); 
                this.DisposeDrawableBackground();
            }
        }

        private void DisposeDrawableBackground()
        {
            this.drawableBackground?.Dispose();
            this.drawableBackground = null;
        }
    }
}