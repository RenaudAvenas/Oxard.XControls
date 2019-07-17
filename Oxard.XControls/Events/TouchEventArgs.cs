using System;
using Xamarin.Forms;

namespace Oxard.XControls.Events
{
    /// <summary>
    /// Event argument for device Touch
    /// </summary>
    public class TouchEventArgs : EventArgs
    {
        private readonly Lazy<Point> lazyPosition;

        /// <summary>
        /// Constructor that define a lazy object to get device touch position
        /// </summary>
        /// <param name="lazyPosition">Lazy object to get device touch position</param>
        public TouchEventArgs(Lazy<Point> lazyPosition)
        {
            this.lazyPosition = lazyPosition;
        }

        /// <summary>
        /// Constructor that define device touch position
        /// </summary>
        /// <param name="position">Device touch position</param>
        public TouchEventArgs(Point position)
        {
            this.lazyPosition = new Lazy<Point>(() => position);
        }

        /// <summary>
        /// Get the device touch position
        /// </summary>
        public Point Position => this.lazyPosition.Value;
    }
}
