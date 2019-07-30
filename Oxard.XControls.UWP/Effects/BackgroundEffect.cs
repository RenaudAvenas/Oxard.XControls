using Oxard.XControls.Graphics;
using Oxard.XControls.Interpretors;
using Oxard.XControls.UWP.Effects;
using Oxard.XControls.UWP.Extensions;
using Oxard.XControls.UWP.Interpretors;
using System;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Xamarin.Forms;
using Xamarin.Forms.Platform.UWP;

[assembly: ResolutionGroupName("OxardXControls")]
[assembly: ExportEffect(typeof(BackgroundEffect), nameof(BackgroundEffect))]

namespace Oxard.XControls.UWP.Effects
{
    public class BackgroundEffect : PlatformEffect
    {
        private XControls.Effects.BackgroundEffect originalEffect;
        private FrameworkElement backgroundControl;

        static BackgroundEffect()
        {
            InterpretorManager.RegisterForTypeIfNotExists(typeof(Graphics.LineSegment), new LineSegmentInterpretor());
            InterpretorManager.RegisterForTypeIfNotExists(typeof(CornerSegment), new CornerSegmentInterpretor());
        }

        protected override void OnAttached()
        {
            this.backgroundControl = this.Control ?? this.Container;
            if(this.backgroundControl == null)
                return;

            this.originalEffect = (XControls.Effects.BackgroundEffect)this.Element.Effects.First(e => e is XControls.Effects.BackgroundEffect);
            this.originalEffect.BackgroundChanged += this.OriginalEffectOnBackgroundChanged;
            if (this.originalEffect.Background is DrawingBrush drawingBrush)
            {
                var visualElement = this.Element as VisualElement;
                if (visualElement == null)
                    throw new NotSupportedException("BackgroundEffect can be affect on VisualElement only");

                visualElement.SizeChanged += this.VisualElementOnSizeChanged;
                drawingBrush.GeometryChanged += this.DrawingBrushOnGeometryChanged;
            }

            this.ApplyBackground();
        }

        protected override void OnDetached()
        {
            if (this.backgroundControl == null)
                return;

            if (this.originalEffect.Background is DrawingBrush drawingBrush)
            {
                var visualElement = this.Element as VisualElement;
                visualElement.SizeChanged -= this.VisualElementOnSizeChanged;
                drawingBrush.GeometryChanged -= this.DrawingBrushOnGeometryChanged;
            }

            this.originalEffect.BackgroundChanged -= this.OriginalEffectOnBackgroundChanged;
            this.originalEffect = null;
        }

        private void VisualElementOnSizeChanged(object sender, EventArgs e)
        {
            if (this.originalEffect.Background is DrawingBrush drawingBrush)
            {
                var visualElement = this.Element as VisualElement;
                drawingBrush.SetSize(visualElement.Width, visualElement.Height);
            }
        }

        private void ApplyBackground()
        {
            if (this.originalEffect.Background == null)
                return;

            if (this.backgroundControl is Control control)
                control.Background = this.originalEffect.Background.ToBrush();
            else if (this.backgroundControl is Panel panel)
                panel.Background = this.originalEffect.Background.ToBrush();
        }

        private void OriginalEffectOnBackgroundChanged(object sender, EventArgs e)
        {
            this.ApplyBackground();
        }

        private void DrawingBrushOnGeometryChanged(object sender, EventArgs e)
        {
            this.ApplyBackground();
        }
    }
}
