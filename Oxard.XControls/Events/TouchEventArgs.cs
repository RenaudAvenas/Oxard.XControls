using System;
using Xamarin.Forms;

namespace Oxard.XControls.Events
{
    public class TouchEventArgs : EventArgs
    {
        private readonly Lazy<Point> lazyPosition;

        public TouchEventArgs(Lazy<Point> lazyPosition)
        {
            this.lazyPosition = lazyPosition;
        }

        public TouchEventArgs(Point position)
        {
            this.lazyPosition = new Lazy<Point>(() => position);
        }

        public Point Position => this.lazyPosition.Value;
    }
}
