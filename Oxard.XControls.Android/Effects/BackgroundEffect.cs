using Oxard.XControls.Droid.Effects;
using Oxard.XControls.Droid.Events;
using Oxard.XControls.Graphics;
using System;
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
        private BackgroundHelper backgroundHelper;
        private Android.Views.View backgroundControl;

        protected override void OnAttached()
        {
            this.backgroundControl = this.Control ?? this.Container;
            if (this.backgroundControl == null)
                return;

            this.originalEffect = (XControls.Effects.BackgroundEffect)this.Element.Effects.First(e => e is XControls.Effects.BackgroundEffect);
            this.originalEffect.BackgroundChanged += OriginalEffectOnBackgroundChanged;

            if (this.originalEffect.Background is DrawingBrush)
            {
                var visualElement = this.Element as VisualElement;
                if (visualElement == null)
                    throw new NotSupportedException("BackgroundEffect can be setted on VisualElement only");
            }
            this.backgroundHelper = new BackgroundHelper(this.backgroundControl, this.Element as VisualElement, this.originalEffect.Background);
        }

        protected override void OnDetached()
        {
            if (this.backgroundControl == null)
                return;

            this.backgroundHelper.Dispose();
            this.originalEffect.BackgroundChanged -= this.OriginalEffectOnBackgroundChanged;
            this.originalEffect = null;
        }

        private void OriginalEffectOnBackgroundChanged(object sender, EventArgs e)
        {
            this.backgroundHelper.ChangeBackground(this.originalEffect.Background);
        }
    }
}