using Oxard.XControls.Interpretors;
using Oxard.XControls.Extensions;
using Oxard.XControls.UWP.Extensions;
using Oxard.XControls.UWP.Interpretors;
using System;
using System.ComponentModel;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;
using Windows.UI.Xaml;
using Oxard.XControls.Graphics;

namespace Oxard.XControls.UWP.NativeControls
{
    public class DrawingPath : Path
    {
        public static readonly DependencyProperty DrawableProperty = DependencyProperty.Register(nameof(Drawable), typeof(IDrawable), typeof(DrawingPath), new PropertyMetadata(null, OnDrawablePropertyChanged));

        static DrawingPath()
        {
            InterpretorManager.RegisterForTypeIfNotExists(typeof(Graphics.LineSegment), new LineSegmentInterpretor());
            InterpretorManager.RegisterForTypeIfNotExists(typeof(CornerSegment), new CornerSegmentInterpretor());
        }

        public DrawingPath()
        {
        }

        public IDrawable Drawable
        {
            get { return (IDrawable)GetValue(DrawableProperty); }
            set { SetValue(DrawableProperty, value); }
        }

        public void RefreshDrawable()
        {
            this.UpdatePath();
        }

        private static void OnDrawablePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as DrawingPath)?.OnDrawableChanged(e.OldValue as IDrawable);
        }

        private void OnDrawableChanged(IDrawable oldDrawable)
        {
            if (oldDrawable != null)
            {
                oldDrawable.GeometryChanged -= this.DrawableOnGeometryChanged;
                if (oldDrawable is INotifyPropertyChanged oldNotifier)
                    oldNotifier.PropertyChanged -= NotifierPropertyChanged;
            }

            if (this.Drawable == null)
                return;

            this.SetStroke();
            this.SetStrokeThickness();
            this.SetFill();
            this.SetStrokeDashArray();
            this.UpdatePath();

            this.Drawable.GeometryChanged += this.DrawableOnGeometryChanged;
            if (this.Drawable is INotifyPropertyChanged notifier)
                notifier.PropertyChanged += this.NotifierPropertyChanged;
        }

        private void NotifierPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(IDrawable.Stroke))
                this.SetStroke();
            else if (e.PropertyName == nameof(IDrawable.Fill))
                this.SetFill();
            else if (e.PropertyName == nameof(IDrawable.StrokeThickness))
                this.SetStrokeThickness();
            else if (e.PropertyName == nameof(IDrawable.StrokeDashArray))
                this.SetStrokeDashArray();
        }

        private void DrawableOnGeometryChanged(object sender, EventArgs e)
        {
            this.UpdatePath();
        }

        private void SetStroke()
        {
            this.Stroke = this.Drawable.Stroke.ToBrush();
        }

        private void SetFill()
        {
            this.Fill = this.Drawable.Fill.ToBrush();
        }

        private void SetStrokeThickness()
        {
            this.StrokeThickness = this.Drawable.StrokeThickness;
        }

        private void SetStrokeDashArray()
        {
            if (this.Drawable.StrokeDashArray.Y.DoubleIsEquals(0d))
                this.StrokeDashArray = new DoubleCollection();
            else
                this.StrokeDashArray = new DoubleCollection { this.Drawable.StrokeDashArray.X, this.Drawable.StrokeDashArray.Y };
        }

        private void UpdatePath()
        {
            if (this.Drawable?.Geometry == null)
                return;

            var reader = this.Drawable.Geometry.GetReader();

            var geometry = new PathGeometry();
            var figure = new PathFigure { IsClosed = this.Drawable.Geometry.IsClosed, StartPoint = reader.FromPoint.ToPoint() };

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
            this.Data = geometry;
        }
    }
}
