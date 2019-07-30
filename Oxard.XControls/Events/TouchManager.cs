using System;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Oxard.XControls.Events
{
    /// <summary>
    /// Delegate used to define touch events
    /// </summary>
    /// <param name="sender">The sender object</param>
    /// <param name="args">The touch argument</param>
    public delegate void TouchEventHandler(object sender, TouchEventArgs args);

    /// <summary>
    /// Class that managed touch events
    /// </summary>
    public class TouchManager
    {
        private const double moveTolerance = 10;
        private StartTouch currentStartTouch;
        private bool currentTouchDisabled;
        private Task longPressDelayTask;
        private CancellationTokenSource cancellationTokenSource;

        /// <summary>
        /// Raised when touch down
        /// </summary>
        public event TouchEventHandler TouchDown;
        /// <summary>
        /// Raised when touch up
        /// </summary>
        public event TouchEventHandler TouchUp;
        /// <summary>
        /// Raised when touch move
        /// </summary>
        public event TouchEventHandler TouchMove;
        /// <summary>
        /// Raised when touch enter
        /// </summary>
        public event TouchEventHandler TouchEnter;
        /// <summary>
        /// Raised when touch leave
        /// </summary>
        public event TouchEventHandler TouchLeave;
        /// <summary>
        /// Raised when touch canceled
        /// </summary>
        public event TouchEventHandler TouchCanceled;
        /// <summary>
        /// Raised when a touch is considered as a click
        /// </summary>
        public event EventHandler Clicked;
        /// <summary>
        /// Raised when a touch is considered as a long press or click
        /// </summary>
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

        /// <summary>
        /// Called when touch is canceled
        /// </summary>
        /// <param name="touchEventArgs">Touch arguments</param>
        public void OnTouchCancel(TouchEventArgs touchEventArgs)
        {
            this.currentStartTouch = null;
            this.IsClicking = false;

            if (this.cancellationTokenSource != null)
            {
                this.cancellationTokenSource.Cancel();
                this.cancellationTokenSource = null;
            }

            if (this.currentTouchDisabled)
            {
                this.currentTouchDisabled = false;
                return;
            }

            this.TouchCanceled?.Invoke(this, touchEventArgs);
        }

        /// <summary>
        /// Called when touch is down
        /// </summary>
        /// <param name="touchEventArgs">Touch arguments</param>
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

        /// <summary>
        /// Called when touch enter
        /// </summary>
        /// <param name="touchEventArgs">Touch arguments</param>
        public void OnTouchEnter(TouchEventArgs touchEventArgs)
        {
            if (this.currentTouchDisabled)
                return;

            this.IsClicking = true;
            this.TouchEnter?.Invoke(this, touchEventArgs);
        }

        /// <summary>
        /// Called when touch leave
        /// </summary>
        /// <param name="touchEventArgs">Touch arguments</param>
        public void OnTouchLeave(TouchEventArgs touchEventArgs)
        {
            if (this.currentTouchDisabled)
                return;

            this.IsClicking = false;
            this.currentStartTouch = null;
            this.TouchLeave?.Invoke(this, touchEventArgs);
        }

        /// <summary>
        /// Called when touch move
        /// </summary>
        /// <param name="touchEventArgs">Touch arguments</param>
        public void OnTouchMove(TouchEventArgs touchEventArgs)
        {
            if (this.currentTouchDisabled)
                return;

            if (this.currentStartTouch != null && this.currentStartTouch.Position.Distance(touchEventArgs.Position) > moveTolerance)
                this.currentStartTouch = null;

            this.TouchMove?.Invoke(this, touchEventArgs);
        }

        /// <summary>
        /// Called when touch up
        /// </summary>
        /// <param name="touchEventArgs">Touch arguments</param>
        public void OnTouchUp(TouchEventArgs touchEventArgs)
        {
            if (this.currentTouchDisabled)
            {
                this.currentTouchDisabled = false;
                return;
            }

            this.currentStartTouch = null;

            if (this.cancellationTokenSource != null)
            {
                this.cancellationTokenSource.Cancel();
                this.cancellationTokenSource = null;
            }

            this.TouchUp?.Invoke(this, touchEventArgs);

            if (this.IsClicking)
                this.Clicked?.Invoke(this, EventArgs.Empty);

            this.IsClicking = false;
        }

        /// <summary>
        /// this method disable the current tracking of a current touch device
        /// </summary>
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
