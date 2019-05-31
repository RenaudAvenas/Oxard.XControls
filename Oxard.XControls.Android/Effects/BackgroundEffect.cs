using Oxard.XControls.Droid.Effects;
using Oxard.XControls.Droid.Events;
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
        private BackgroundHelper backgroundHelper;

        protected override void OnAttached()
        {
            var originalEffect = (XControls.Effects.BackgroundEffect)this.Element.Effects.FirstOrDefault(e => e is XControls.Effects.BackgroundEffect);
            this.originalEffect = originalEffect;

            if (this.originalEffect.Background is DrawingBrush drawingBrush)
            {
                var visualElement = this.Element as VisualElement;
                if (visualElement == null)
                    throw new NotSupportedException("BackgroundEffect can be affect on VisualElement only");
            }

            this.backgroundHelper = new BackgroundHelper(this.Control, this.Element as VisualElement, this.originalEffect.Background);
        }

        protected override void OnDetached()
        {
            this.backgroundHelper.Dispose();
            this.originalEffect = null;
        }
    }
}