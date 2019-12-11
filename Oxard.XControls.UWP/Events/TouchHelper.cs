using Oxard.XControls.Events;
using Oxard.XControls.UWP.Extensions;
using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Input;

namespace Oxard.XControls.UWP.Events
{
    public class TouchHelper : IDisposable
    {
        private readonly TouchManager touchManager;
        private readonly UIElement control;

        public TouchHelper(TouchManager touchManager, UIElement control)
        {
            this.touchManager = touchManager;
            this.control = control;

            this.control.PointerPressed += this.ControlOnPointerPressed;
            this.control.PointerMoved += this.ControlOnPointerMoved;
            this.control.PointerEntered += this.ControlOnPointerEntered;
            this.control.PointerExited += this.ControlOnPointerExited;
            this.control.PointerCanceled += this.ControlOnPointerCanceled;
            this.control.PointerReleased += this.ControlOnPointerReleased;
        }

        private void ControlOnPointerPressed(object sender, PointerRoutedEventArgs e)
        {
            this.touchManager.OnTouchDown(new TouchEventArgs(e.GetCurrentPoint(this.control).Position.ToXamarinPoint()));
            this.control.CapturePointer(e.Pointer);
        }

        private void ControlOnPointerMoved(object sender, PointerRoutedEventArgs e)
        {
            if (this.control.PointerCaptures?.Count > 0)
                this.touchManager.OnTouchMove(new TouchEventArgs(e.GetCurrentPoint(this.control).Position.ToXamarinPoint()));
        }

        private void ControlOnPointerReleased(object sender, PointerRoutedEventArgs e)
        {
            this.touchManager.OnTouchUp(new TouchEventArgs(e.GetCurrentPoint(this.control).Position.ToXamarinPoint()));
            this.control.ReleasePointerCapture(e.Pointer);
        }

        private void ControlOnPointerCanceled(object sender, PointerRoutedEventArgs e)
        {
            this.touchManager.OnTouchCancel(new TouchEventArgs(e.GetCurrentPoint(this.control).Position.ToXamarinPoint()));
            this.control.ReleasePointerCapture(e.Pointer);
        }

        private void ControlOnPointerExited(object sender, PointerRoutedEventArgs e)
        {
            if (this.control.PointerCaptures?.Count > 0)
                this.touchManager.OnTouchLeave(new TouchEventArgs(e.GetCurrentPoint(this.control).Position.ToXamarinPoint()));
        }

        private void ControlOnPointerEntered(object sender, PointerRoutedEventArgs e)
        {
            if (this.control.PointerCaptures?.Count > 0)
                this.touchManager.OnTouchEnter(new TouchEventArgs(e.GetCurrentPoint(this.control).Position.ToXamarinPoint()));
        }

        public void Dispose()
        {
            this.control.PointerPressed -= this.ControlOnPointerPressed;
            this.control.PointerMoved -= this.ControlOnPointerMoved;
            this.control.PointerEntered -= this.ControlOnPointerEntered;
            this.control.PointerExited -= this.ControlOnPointerExited;
            this.control.PointerCanceled -= this.ControlOnPointerCanceled;
            this.control.PointerReleased -= this.ControlOnPointerReleased;
        }
    }
}
