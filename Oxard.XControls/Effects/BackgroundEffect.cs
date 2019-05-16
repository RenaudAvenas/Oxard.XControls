using Oxard.XControls.Graphics;
using System;
using Xamarin.Forms;

namespace Oxard.XControls.Effects
{
    public class BackgroundEffect : RoutingEffect
    {
        public BackgroundEffect() : base($"OxardXControls.{nameof(BackgroundEffect)}")
        {
        }
        
        public Brush Background { get; set; }

        protected override void OnAttached()
        {
            ((VisualElement)this.Element).SizeChanged += this.ElementOnSizeChanged;
        }

        protected override void OnDetached()
        {
            ((VisualElement)this.Element).SizeChanged -= this.ElementOnSizeChanged;
        }

        private void ElementOnSizeChanged(object sender, System.EventArgs e)
        {
            if (Background is DrawingBrush drawingBrush)
                drawingBrush.SetSize(((VisualElement)this.Element).Width, ((VisualElement)this.Element).Height);
        }
    }
}
