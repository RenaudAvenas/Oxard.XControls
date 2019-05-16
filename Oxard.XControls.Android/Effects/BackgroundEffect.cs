using Oxard.XControls.Droid.Effects;
using Oxard.XControls.Droid.Extensions;
using Oxard.XControls.Droid.Graphics;
using Oxard.XControls.Droid.Interpretors;
using Oxard.XControls.Graphics;
using Oxard.XControls.Interpretors;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ResolutionGroupName("OxardXControls")]
[assembly: ExportEffect(typeof(BackgroundEffect), nameof(BackgroundEffect))]

namespace Oxard.XControls.Droid.Effects
{
    public class BackgroundEffect : PlatformEffect
    {
        private XControls.Effects.BackgroundEffect originalEffect;

        static BackgroundEffect()
        {
            InterpretorManager.RegisterForTypeIfNotExists(typeof(LineSegment), new LineSegmentInterpretor());
            InterpretorManager.RegisterForTypeIfNotExists(typeof(CornerSegment), new CornerSegmentInterpretor());
        }

        protected override void OnAttached()
        {
            var originalEffect = (XControls.Effects.BackgroundEffect)this.Element.Effects.FirstOrDefault(e => e is XControls.Effects.BackgroundEffect);
            this.originalEffect = originalEffect;
            if (this.originalEffect.Background is DrawingBrush drawingBrush)
            {
                var visualElement = this.Element as VisualElement;
                if (visualElement == null)
                    throw new NotSupportedException("BackgroundEffect can be affect on VisualElement only");

                visualElement.SizeChanged += this.VisualElementOnSizeChanged;
                drawingBrush.SetSize(((VisualElement)this.Element).Width, ((VisualElement)this.Element).Height);
                drawingBrush.GeometryChanged += this.DrawingBrushOnGeometryChanged;
            }

            this.ApplyBackground();
        }

        protected override void OnDetached()
        {
            if (this.originalEffect.Background is DrawingBrush drawingBrush)
            {
                var visualElement = this.Element as VisualElement;
                visualElement.SizeChanged -= this.VisualElementOnSizeChanged;
                drawingBrush.GeometryChanged -= this.DrawingBrushOnGeometryChanged;
            }

            this.originalEffect = null;
        }

        protected override void OnElementPropertyChanged(PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(e);
            if (this.GetInvalidateDrawProperties().Contains(e.PropertyName))
            {
                this.ApplyBackground();
            }
        }

        protected virtual List<string> GetInvalidateDrawProperties()
        {
            return new List<string> { nameof(IDrawable.Height), nameof(IDrawable.Width), nameof(IDrawable.Fill), nameof(IDrawable.Stroke), nameof(IDrawable.StrokeThickness), nameof(IDrawable.StrokeDashArray) };
        }

        private void DrawingBrushOnGeometryChanged(object sender, EventArgs e)
        {
            this.ApplyBackground();
        }

        private void VisualElementOnSizeChanged(object sender, EventArgs e)
        {
            ((DrawingBrush)this.originalEffect.Background).SetSize(((VisualElement)this.Element).Width, ((VisualElement)this.Element).Height);
        }

        private void ApplyBackground()
        {
            if (this.originalEffect.Background == null)
                return;

            this.Control.SetBackground(this.ToIDrawable(this.originalEffect.Background).ToDrawable(this.Control.Context));
        }

        private IDrawable ToIDrawable(Brush brush)
        {
            if (brush is IDrawable drawable)
                return drawable;

            return new DrawableAdapter((VisualElement)this.Element, brush);
        }
    }
}