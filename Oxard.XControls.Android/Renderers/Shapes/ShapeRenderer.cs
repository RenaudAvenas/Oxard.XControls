using Android.Content;
using Oxard.XControls.Droid.NativeControls;
using Oxard.XControls.Droid.Renderers.Shapes;
using Oxard.XControls.Shapes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(Oxard.XControls.Shapes.Rectangle), typeof(ShapeRenderer))]
[assembly: ExportRenderer(typeof(Ellipse), typeof(ShapeRenderer))]

namespace Oxard.XControls.Droid.Renderers.Shapes
{
    public class ShapeRenderer : ViewRenderer<Shape, ShapeView>
    {
        public ShapeRenderer(Context context)
            : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Shape> e)
        {
            base.OnElementChanged(e);
            if (e.OldElement != null)
                e.OldElement.GeometryChanged -= this.ElementOnGeometryChanged;

            if (this.Control == null)
            {
                var view = new ShapeView(this.Context);
                this.SetNativeControl(view);
                view.Source = e.NewElement;
            }

            if (e.NewElement != null)
                e.NewElement.GeometryChanged += this.ElementOnGeometryChanged;
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
            if (this.GetInvalidateDrawProperties().Contains(e.PropertyName))
            {
                this.Control?.Invalidate();
            }
        }

        protected virtual List<string> GetInvalidateDrawProperties()
        {
            return new List<string> { nameof(this.Element.Fill), nameof(this.Element.Stroke), nameof(this.Element.StrokeThickness), nameof(this.Element.Stretch), nameof(this.Element.StrokeDashArray) };
        }

        protected override void Dispose(bool disposing)
        {
            this.Element.GeometryChanged -= this.ElementOnGeometryChanged;
            base.Dispose(disposing);
        }

        private void ElementOnGeometryChanged(object sender, EventArgs eventArgs)
        {
            this.Control?.Invalidate();
        }
    }
}
