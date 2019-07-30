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
            this.control = androidView ?? throw new ArgumentNullException(nameof(androidView));
            this.element = visualElement ?? throw new ArgumentNullException(nameof(visualElement));
            this.background = background ?? throw new ArgumentNullException(nameof(background));

            element.SizeChanged += this.ElementOnSizeChanged;
            this.ChangeBackground(background);
        }

        public void ChangeBackground(Brush newBackground)
        {
            if (this.background != null)
                this.UnhandleEvents();

            this.background = newBackground;
            if (this.background is DrawingBrush drawingBrush)
                drawingBrush.SetSize(this.element.Width, this.element.Height);

            this.HandleEvents();
            this.ApplyBackground();
        }

        public void ReapplyBackground()
        {
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
            if (this.element.Width <= 0 || this.element.Height <= 0)
                return;

            if (this.background is DrawingBrush drawingBrush)
                drawingBrush.SetSize(this.element.Width, this.element.Height);

            this.ApplyBackground();
        }

        private void DrawingBrushOnGeometryChanged(object sender, EventArgs e)
        {
            this.ApplyBackground();
        }

        private void BrushOnPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(this.element.Width) || e.PropertyName == nameof(this.element.Height))
                return;

            this.ApplyBackground();
        }

        private void ApplyBackground()
        {
            if (this.element.Width <= 0 || this.element.Height <= 0)
                return;

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