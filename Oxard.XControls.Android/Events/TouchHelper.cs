using Android.Views;
using Oxard.XControls.Events;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

namespace Oxard.XControls.Droid.Events
{
    public class TouchHelper
    {
        private readonly TouchManager touchManager;
        private readonly Android.Views.View view;
        private bool isTouchIn;

        public TouchHelper(TouchManager touchManager, Android.Views.View view)
        {
            this.touchManager = touchManager;
            this.view = view;
        }

        private static Point CreatePoint(float x, float y)
        {
            var xDpi = Android.App.Application.Context.FromPixels(x);
            var yDpi = Android.App.Application.Context.FromPixels(y);
            return new Point(xDpi, yDpi);
        }

        public bool OnTouchEvent(MotionEvent e)
        {
            float x = e.GetX();
            float y = e.GetY();
            var touchEventArgs = new TouchEventArgs(new Lazy<Point>(() => CreatePoint(x, y)));
            
            switch (e.Action)
            {
                case MotionEventActions.Down:
                    this.isTouchIn = true;
                    this.touchManager.OnTouchDown(touchEventArgs);
                    return true;
                case MotionEventActions.Move:

                    this.touchManager.OnTouchMove(touchEventArgs);

                    if (x <= this.view.Width && y <= this.view.Height && x >= 0 && y >= 0)
                    {
                        if (!this.isTouchIn)
                        {
                            this.isTouchIn = true;
                            this.touchManager.OnTouchEnter(touchEventArgs);
                        }
                    }
                    else
                    {
                        if (this.isTouchIn)
                        {
                            this.isTouchIn = false;
                            this.touchManager.OnTouchLeave(touchEventArgs);
                        }
                    }

                    return true;
                case MotionEventActions.Up:
                    this.touchManager.OnTouchUp(touchEventArgs);
                    return true;
                case MotionEventActions.Cancel:
                    this.touchManager.OnTouchCancel(touchEventArgs);
                    return true;
                default:
                    return false;
            }

        }
    }
}