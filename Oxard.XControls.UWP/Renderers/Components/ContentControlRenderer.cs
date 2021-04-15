using System.ComponentModel;
using Oxard.XControls.Graphics;
using Oxard.XControls.UWP.NativeControls;
using Oxard.XControls.UWP.Renderers.Components;
using Windows.UI.Xaml;
using Xamarin.Forms;
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
            base.OnElementChanged(e);

            if (e.NewElement != null)
            {
                this.ApplyIsBackgroundManagedByStyle();
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (this.drawingPath != null)
                this.drawingPath.Drawable = null;

            base.Dispose(disposing);
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
            if(e.PropertyName == nameof(VisualElement.Width) || e.PropertyName == nameof(VisualElement.Height))
            {
                if (this.drawingPath != null)
                    this.drawingPath.Drawable.SetSize(this.Element.Width, this.Element.Height);
            }
            if (e.PropertyName == nameof(ContentControl.IsBackgroundManagedByStyle))
                this.ApplyIsBackgroundManagedByStyle();
        }

        private void ApplyIsBackgroundManagedByStyle()
        {
            this.UpdateBackground();
        }

        protected override void UpdateBackground()
        {
            if (this.Element.IsBackgroundManagedByStyle)
            {
                if (this.drawingPath != null)
                {
                    this.drawingPath.Drawable = null;
                    this.Children.Remove(this.drawingPath);
                }

                return;
            }

            if (this.Element.Background is DrawingBrush drawingBrush)
            {
                if (this.Control is Windows.UI.Xaml.Controls.Control control)
                    control.Background = null;

                if (this.drawingPath == null)
                {
                    this.drawingPath = new DrawingPath();
                    this.Children.Insert(0, this.drawingPath);
                }

                drawingBrush.SetSize(this.Element.Width, this.Element.Height);
                this.drawingPath.Drawable = drawingBrush;

                return;
            }
            else
            {
                if (this.drawingPath != null)
                {
                    this.Children.Remove(this.drawingPath);
                    this.drawingPath = null;
                }
            }

            base.UpdateBackground();
        }

        protected override Windows.Foundation.Size MeasureOverride(Windows.Foundation.Size availableSize)
        {
            if (this.Children[0] is DrawingPath)
                Children[0].Measure(availableSize);
            return base.MeasureOverride(availableSize);
        }

        protected override Windows.Foundation.Size ArrangeOverride(Windows.Foundation.Size finalSize)
        {
            if (this.Children[0] is DrawingPath)
                Children[0].Arrange(new Windows.Foundation.Rect(new Windows.Foundation.Point(), finalSize));
            return base.ArrangeOverride(finalSize);
        }
    }
}