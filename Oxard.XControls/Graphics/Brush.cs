using System;
using Xamarin.Forms;

namespace Oxard.XControls.Graphics
{
    /// <summary>
    /// Base class for brushes
    /// </summary>
    public abstract class Brush : BindableObject, ICloneable
    {
        /// <summary>
        /// Creates a new object that is a copy of the current instance.
        /// </summary>
        /// <returns>A new object that is a copy of this instance.</returns>
        public abstract object Clone();
    }
}
