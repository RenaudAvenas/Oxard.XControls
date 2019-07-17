using Oxard.XControls.Graphics;
using Xamarin.Forms;

namespace Oxard.XControls.Effects
{
    /// <summary>
    /// Add a background effect using a <see cref="Brush"/> instead of <see cref="Color"/> (BackgroundColor property will be overrided)
    /// </summary>
    public class BackgroundEffect : RoutingEffect
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        public BackgroundEffect() : base($"OxardXControls.{nameof(BackgroundEffect)}")
        {
        }
        
        /// <summary>
        /// Get or set the <see cref="Brush"/> to use as background
        /// </summary>
        public Brush Background { get; set; }
    }
}
