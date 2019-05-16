using Oxard.XControls.Extensions;
using Oxard.XControls.Graphics;
using Oxard.XControls.Interpretors;
using Oxard.XControls.UWP.Extensions;
using Oxard.XControls.UWP.Interpretors;
using Oxard.XControls.UWP.Renderers.Shapes;
using System;
using System.ComponentModel;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;
using Xamarin.Forms.Platform.UWP;
using Shape = Oxard.XControls.Shapes.Shape;
using Rectangle = Oxard.XControls.Shapes.Rectangle;
using Ellipse = Oxard.XControls.Shapes.Ellipse;

[assembly: ExportRenderer(typeof(Rectangle), typeof(ShapeRenderer))]
[assembly: ExportRenderer(typeof(Ellipse), typeof(ShapeRenderer))]

namespace Oxard.XControls.UWP.Renderers.Shapes
{
    public class ShapeRenderer : ViewRenderer<Shape, Path>
    {
        static ShapeRenderer()
        {
            InterpretorManager.RegisterForTypeIfNotExists(typeof(Graphics.LineSegment), new LineSegmentInterpretor());
            InterpretorManager.RegisterForTypeIfNotExists(typeof(CornerSegment), new CornerSegmentInterpretor());
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Shape> e)
        {
            base.OnElementChanged(e);
            if (e.OldElement != null)
                e.OldElement.GeometryChanged -= this.ElementOnGeometryChanged;

            if (e.NewElement != null)
            {
                if (this.Control == null)
                    this.SetNativeControl(new Path());

                this.SetStroke();
                this.SetStrokeThickness();
                this.SetFill();
                this.SetStrokeDashArray();

                e.NewElement.GeometryChanged += this.ElementOnGeometryChanged;
            }

            this.UpdatePath(this.Element);
        }

        protected override void Dispose(bool disposing)
        {
            this.Element.GeometryChanged -= this.ElementOnGeometryChanged;
            base.Dispose(disposing);
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
            if (e.PropertyName == nameof(Shape.Stroke))
                this.SetStroke();
            if (e.PropertyName == nameof(Shape.Fill))
                this.SetFill();
            if (e.PropertyName == nameof(Shape.StrokeThickness))
                this.SetStrokeThickness();
            if (e.PropertyName == nameof(Shape.StrokeDashArray))
                this.SetStrokeDashArray();
        }

        private void ElementOnGeometryChanged(object sender, EventArgs e)
        {
            this.UpdatePath(this.Element);
        }

        private void SetStroke()
        {
            this.Control.Stroke = this.Element.Stroke.ToBrush();
        }

        private void SetFill()
        {
            this.Control.Fill = this.Element.Fill.ToBrush();
        }

        private void SetStrokeThickness()
        {
            this.Control.StrokeThickness = this.Element.StrokeThickness;
        }

        private void SetStrokeDashArray()
        {
            if (this.Element.StrokeDashArray.Y.DoubleIsEquals(0d))
                this.Control.StrokeDashArray = new DoubleCollection();
            else
                this.Control.StrokeDashArray = new DoubleCollection { this.Element.StrokeDashArray.X, this.Element.StrokeDashArray.Y };
        }

        private void UpdatePath(Shape source)
        {
            if (source?.Geometry == null)
                return;

            var reader = source.Geometry.GetReader();

            var geometry = new PathGeometry();
            var figure = new PathFigure { IsClosed = true, StartPoint = reader.FromPoint.ToPoint() };

            var segment = reader.GetNext();
            while (segment != null)
            {
                var interpretor = InterpretorManager.GetForType(segment.GetType());

                if (interpretor == null)
                    throw new InvalidOperationException($"Interpretor for type {segment.GetType()} was not found. Use InterpretorManager.RegisterForType method to register your interpretor.");

                if (!(interpretor is ISegmentInterpretor segmentInterpretor))
                    throw new InvalidOperationException($"Interpretor for type {segment.GetType()} was found but it is not an ISegmentInterpretor.");

                figure.Segments.Add(segmentInterpretor.ToNativeSegment(segment, reader.FromPoint));

                segment = reader.GetNext();
            }

            geometry.Figures.Add(figure);
            this.Control.Data = geometry;
        }
    }
}
