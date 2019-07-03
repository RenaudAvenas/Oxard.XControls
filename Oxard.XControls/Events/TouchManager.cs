using System;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Oxard.XControls.Events
{
    public delegate void TouchEventHandler(object sender, TouchEventArgs args);

    public class TouchManager
    {
        private const double moveTolerance = 10;
        private StartTouch currentStartTouch;
        private bool currentTouchDisabled;
        private Task longPressDelayTask;
        private CancellationTokenSource cancellationTokenSource;

        public TouchManager()
        {
        }

        public event TouchEventHandler TouchDown;
        public event TouchEventHandler TouchUp;
        public event TouchEventHandler TouchMove;
        public event TouchEventHandler TouchEnter;
        public event TouchEventHandler TouchLeave;
        public event TouchEventHandler TouchCanceled;
        public event EventHandler Clicked;
        public event TouchEventHandler LongPressed;

        /// <summary>
        /// Get or set the time in milliseconds when the manager consider a touch as a long press. The default value is 1000
        /// </summary>
        public int LongPressTime { get; set; } = 1000;

        /// <summary>
        /// Get or set a value indicates that a long press must cancel a click gesture or not. Default value is false
        /// </summary>
        public bool LongPressCancelClick { get; set; }

        /// <summary>
        /// Get a value that indicates if a click is in progress.
        /// </summary>
        public bool IsClicking { get; private set; }

        public void OnTouchCancel(TouchEventArgs touchEventArgs)
        {
            if (this.currentTouchDisabled)
            {
                this.currentTouchDisabled = false;
                return;
            }

            this.currentTouchDisabled = false;
            this.IsClicking = false;
            this.TouchCanceled?.Invoke(this, touchEventArgs);
        }

        public void OnTouchDown(TouchEventArgs touchEventArgs)
        {
            this.currentStartTouch = new StartTouch(touchEventArgs.Position);
            this.IsClicking = true;

            if (this.cancellationTokenSource != null)
            {
                this.cancellationTokenSource.Cancel();
                this.cancellationTokenSource = null;
            }

            this.cancellationTokenSource = new CancellationTokenSource();

            this.longPressDelayTask = Task.Delay(this.LongPressTime, cancellationTokenSource.Token);
            this.longPressDelayTask.ContinueWith(t =>
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    this.cancellationTokenSource = null;

                    if (this.currentStartTouch == null || t.IsCanceled)
                        return;
                    
                    if (this.LongPressCancelClick)
                        this.IsClicking = false;

                    this.LongPressed?.Invoke(this, new TouchEventArgs(this.currentStartTouch.Position));
                    this.currentStartTouch = null;
                });
            });

            this.TouchDown?.Invoke(this, touchEventArgs);
        }

        public void OnTouchEnter(TouchEventArgs touchEventArgs)
        {
            if (this.currentTouchDisabled)
                return;

            this.TouchEnter?.Invoke(this, touchEventArgs);
        }

        public void OnTouchLeave(TouchEventArgs touchEventArgs)
        {
            if (this.currentTouchDisabled)
                return;

            this.currentStartTouch = null;
            this.TouchLeave?.Invoke(this, touchEventArgs);
        }

        public void OnTouchMove(TouchEventArgs touchEventArgs)
        {
            if (this.currentTouchDisabled)
                return;

            if (this.currentStartTouch != null && this.currentStartTouch.Position.Distance(touchEventArgs.Position) > moveTolerance)
                this.currentStartTouch = null;

            this.TouchMove?.Invoke(this, touchEventArgs);
        }

        public void OnTouchUp(TouchEventArgs touchEventArgs)
        {
            if (this.currentTouchDisabled)
            {
                this.currentTouchDisabled = false;
                return;
            }

            this.currentStartTouch = null;

            if (this.IsClicking)
                this.Clicked?.Invoke(this, EventArgs.Empty);

            this.IsClicking = false;

            if (this.cancellationTokenSource != null)
            {
                this.cancellationTokenSource.Cancel();
                this.cancellationTokenSource = null;
            }

            this.TouchUp?.Invoke(this, touchEventArgs);
        }

        public void DisableCurrentTouch()
        {
            this.IsClicking = false;
            this.currentStartTouch = null;
            this.currentTouchDisabled = true;
        }

        private class StartTouch
        {
            public StartTouch(Point position)
            {
                this.Position = position;
                this.Date = DateTime.Now;
            }

            public Point Position { get; }

            public DateTime Date { get; }
        }
    }
}
