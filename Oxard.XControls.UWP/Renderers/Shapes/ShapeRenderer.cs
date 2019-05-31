using Oxard.XControls.UWP.Renderers.Shapes;
using Xamarin.Forms.Platform.UWP;
using Shape = Oxard.XControls.Shapes.Shape;
using Rectangle = Oxard.XControls.Shapes.Rectangle;
using Ellipse = Oxard.XControls.Shapes.Ellipse;
using Oxard.XControls.UWP.NativeControls;

[assembly: ExportRenderer(typeof(Rectangle), typeof(ShapeRenderer))]
[assembly: ExportRenderer(typeof(Ellipse), typeof(ShapeRenderer))]

namespace Oxard.XControls.UWP.Renderers.Shapes
{
    public class ShapeRenderer : ViewRenderer<Shape, DrawingPath>
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Shape> e)
        {
            if (e.OldElement != null && this.Control != null)
                this.Control.Drawable = null;

            base.OnElementChanged(e);

            if (e.NewElement != null)
            {
                if (this.Control == null)
                    this.SetNativeControl(new DrawingPath());

                this.Control.Drawable = this.Element;
            }
        }

        protected override void Dispose(bool disposing)
        {
            this.Control.Drawable = null;
            base.Dispose(disposing);
        }
    }
}
