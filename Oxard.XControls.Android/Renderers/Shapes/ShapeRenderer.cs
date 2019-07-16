using Android.Content;
using Android.Views;
using Oxard.XControls.Droid.NativeControls;
using Oxard.XControls.Droid.Renderers.Shapes;
using Oxard.XControls.Shapes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(Oxard.XControls.Shapes.Rectangle), typeof(FastShapeRenderer))]
[assembly: ExportRenderer(typeof(Ellipse), typeof(FastShapeRenderer))]

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
            return new List<string> { nameof(this.Element.Fill), nameof(this.Element.Stroke), nameof(this.Element.Stretch), nameof(this.Element.StrokeDashArray) };
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

    public class FastShapeRenderer : ShapeView, IVisualElementRenderer
    {
        public FastShapeRenderer(Context context) : base(context)
        {
            this.Tracker = new VisualElementTracker(this);
        }

        public Shape Element { get; private set; }

        VisualElement IVisualElementRenderer.Element => this.Element;

        public VisualElementTracker Tracker { get; private set; }

        public ViewGroup ViewGroup => null;

        public Android.Views.View View => this;

        public event EventHandler<VisualElementChangedEventArgs> ElementChanged;
        public event EventHandler<PropertyChangedEventArgs> ElementPropertyChanged;

        protected override void Dispose(bool disposing)
        {
            this.Element.GeometryChanged -= this.ElementOnGeometryChanged;
            this.Element.PropertyChanged -= this.ElementOnPropertyChanged;
            this.Tracker.Dispose();
            base.Dispose(disposing);
        }

        public SizeRequest GetDesiredSize(int widthConstraint, int heightConstraint)
        {
            return new SizeRequest();
        }

        public void SetElement(VisualElement element)
        {
            var oldElement = this.Element;
            if (oldElement != null)
            {
                oldElement.PropertyChanged -= ElementOnPropertyChanged;
                oldElement.GeometryChanged -= this.ElementOnGeometryChanged;
            }

            this.Element = (Shape)element;
            this.Source = this.Element;
            if (element != null)
            {
                this.Element.PropertyChanged += ElementOnPropertyChanged;
                this.Element.GeometryChanged += ElementOnGeometryChanged;
                this.Invalidate();
            }

            this.ElementChanged?.Invoke(this, new VisualElementChangedEventArgs(oldElement, this.Element));
        }

        public void SetLabelFor(int? id)
        {
        }

        public void UpdateLayout()
        {
            this.Tracker.UpdateLayout();
        }

        protected virtual List<string> GetInvalidateDrawProperties()
        {
            return new List<string> { nameof(this.Element.Fill), nameof(this.Element.Stroke), nameof(this.Element.Stretch), nameof(this.Element.StrokeDashArray) };
        }

        private void ElementOnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            this.ElementPropertyChanged?.Invoke(this, e);
            if (this.GetInvalidateDrawProperties().Contains(e.PropertyName))
                this.Invalidate();
        }

        private void ElementOnGeometryChanged(object sender, EventArgs e)
        {
            this.Invalidate();
        }
    }
}
