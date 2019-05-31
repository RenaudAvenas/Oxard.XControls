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
    }
}
