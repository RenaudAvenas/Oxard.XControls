using Oxard.XControls.Droid.Extensions;
using Oxard.XControls.Droid.Graphics;
using Oxard.XControls.Droid.Interpretors;
using Oxard.XControls.Graphics;
using Oxard.XControls.Interpretors;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using View = Android.Views.View;

namespace Oxard.XControls.Droid.Events
{
    public class BackgroundHelper : IDisposable
    {
        private readonly View control;
        private readonly VisualElement element;
        private Brush background;

        static BackgroundHelper()
        {
            InterpretorManager.RegisterForTypeIfNotExists(typeof(LineSegment), new LineSegmentInterpretor());
            InterpretorManager.RegisterForTypeIfNotExists(typeof(CornerSegment), new CornerSegmentInterpretor());
        }

        public BackgroundHelper(View androidView, VisualElement visualElement, Brush background)
        {
            this.control = androidView;
            this.element = visualElement;
            this.background = background;

            element.SizeChanged += this.ElementOnSizeChanged;
            this.HandleEvents();
            this.ApplyBackground();
        }

        public void ChangeBackground(Brush newBackground)
        {
            if (this.background != null)
                this.UnhandleEvents();

            this.background = newBackground;
            this.HandleEvents();
            this.ApplyBackground();
        }

        public void Dispose()
        {
            element.SizeChanged -= this.ElementOnSizeChanged;
            this.UnhandleEvents();
        }

        private void HandleEvents()
        {
            background.PropertyChanged += this.BrushOnPropertyChanged;
            if (this.background is DrawingBrush drawingBrush)
                drawingBrush.GeometryChanged += this.DrawingBrushOnGeometryChanged;
        }

        private void UnhandleEvents()
        {
            background.PropertyChanged -= this.BrushOnPropertyChanged;
            if (this.background is DrawingBrush drawingBrush)
                drawingBrush.GeometryChanged -= this.DrawingBrushOnGeometryChanged;
        }

        private void ElementOnSizeChanged(object sender, EventArgs e)
        {
            this.ApplyBackground();
        }

        private void DrawingBrushOnGeometryChanged(object sender, EventArgs e)
        {
            this.ApplyBackground();
        }

        private void BrushOnPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            this.ApplyBackground();
        }

        private void ApplyBackground()
        {
            this.control.SetBackground(this.ToIDrawable(this.background).ToDrawable(this.control.Context));
        }

        private IDrawable ToIDrawable(Brush brush)
        {
            if (brush is IDrawable drawable)
                return drawable;

            return new DrawableAdapter(this.element, brush);
        }
    }
}