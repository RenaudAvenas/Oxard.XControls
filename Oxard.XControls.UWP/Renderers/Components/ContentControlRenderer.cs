using System.ComponentModel;
using Oxard.XControls.Graphics;
using Oxard.XControls.UWP.NativeControls;
using Oxard.XControls.UWP.Renderers.Components;
using Windows.Foundation;
using Windows.UI.Xaml;
using Xamarin.Forms.Platform.UWP;
using ContentControl = Oxard.XControls.Components.ContentControl;

[assembly: ExportRenderer(typeof(ContentControl), typeof(ContentControlRenderer<ContentControl>))]

namespace Oxard.XControls.UWP.Renderers.Components
{
    public class ContentControlRenderer<T> : ViewRenderer<T, FrameworkElement> where T : ContentControl
    {
        private DrawingPath drawingPath;

        protected override void OnElementChanged(ElementChangedEventArgs<T> e)
        {
            if (e.OldElement != null)
                e.OldElement.SizeChanged -= this.ElementOnSizeChanged;

            base.OnElementChanged(e);

            if (e.NewElement != null)
            {
                this.Element.SizeChanged += this.ElementOnSizeChanged;
                this.ApplyIsBackgroundManagedByStyle();
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (this.drawingPath != null)
                this.drawingPath.Drawable = null;

            this.Element.SizeChanged -= this.ElementOnSizeChanged;
            base.Dispose(disposing);
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
            if (e.PropertyName == nameof(ContentControl.Background))
                this.ApplyBackground();
            else if (e.PropertyName == nameof(ContentControl.IsBackgroundManagedByStyle))
                this.ApplyIsBackgroundManagedByStyle();
        }

        private void ElementOnSizeChanged(object sender, System.EventArgs e)
        {
            if (this.Element.Background is DrawingBrush drawable)
            {
                drawable.SetSize(this.Element.Width, this.Element.Height);
                this.drawingPath.RefreshDrawable();
            }
        }

        private void ApplyIsBackgroundManagedByStyle()
        {
            this.ApplyBackground();
        }

        private void ApplyBackground()
        {
            if (this.Element.Background == null || this.Element.IsBackgroundManagedByStyle)
            {
                if (this.drawingPath != null)
                {
                    this.drawingPath.Drawable = null;
                    this.Children.Remove(this.drawingPath);
                }

                return;
            }

            if (this.drawingPath == null)
            {
                this.drawingPath = new DrawingPath();
                this.Children.Insert(0, this.drawingPath);
            }

            if (this.Element.Background is DrawingBrush drawingBrush)
            {
                drawingBrush.SetSize(this.Element.Width, this.Element.Height);
                this.drawingPath.Drawable = drawingBrush;
            }
            else
            {
                var rectangleBrush = new RectangleBrush { Fill = this.Element.Background };
                rectangleBrush.SetSize(this.Element.Width, this.Element.Height);
                this.drawingPath.Drawable = rectangleBrush;
            }
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            if (this.Children[0] is DrawingPath)
                Children[0].Measure(availableSize);
            return base.MeasureOverride(availableSize);
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            if (this.Children[0] is DrawingPath)
                Children[0].Arrange(new Rect(new Point(), finalSize));
            return base.ArrangeOverride(finalSize);
        }
    }
}