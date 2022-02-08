using Android.Views;
using System;

namespace Oxard.XControls.Droid.Events
{
    public interface IKeyboardListener
    {
        /// <summary>
        /// Must be raised in DispatchKey overrided method.
        /// </summary>
        event EventHandler<AndroidKeyboardEventArgs> PreviewKeyEvent;

        /// <summary>
        /// Must be raised in OnKeyUp and OnKeyDown overrided method.
        /// </summary>
        event EventHandler<AndroidKeyboardEventArgs> KeyEvent;
    }

    public class AndroidKeyboardEventArgs : EventArgs
    {
        public AndroidKeyboardEventArgs(KeyEvent keyEvent)
        {
            KeyEvent = keyEvent;
        }

        public KeyEvent KeyEvent { get; }

        public bool? Handled { get; set; }
    }
}