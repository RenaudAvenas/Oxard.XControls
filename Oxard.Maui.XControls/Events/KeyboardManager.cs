using System;

namespace Oxard.XControls.Events
{
    /// <summary>
    /// Delegate used to define keyboard events
    /// </summary>
    /// <param name="sender">The sender object</param>
    /// <param name="args">The keyboard argument</param>
    public delegate void KeyboardEventHandler(object sender, KeyboardEventArgs args);

    /// <summary>
    /// Class that managed keyboard events
    /// </summary>
    public class KeyboardManager
    {
        /// <summary>
        /// Raised when <see cref="ShowVirtualKeyboard"/> method called. Don't forget to initialize KeyboardHelpers on each platform.
        /// </summary>
        public static event EventHandler VirtualKeyboardRequested;

        /// <summary>
        /// Raised when <see cref="HideVirtualKeyboard"/> method called. Don't forget to initialize KeyboardHelpers on each platform.
        /// </summary>
        public static event EventHandler HideVirtualKeyboardRequested;

        /// <summary>
        /// Raised when key down occurs on application. Don't forget to initialize KeyboardHelpers on each platform.
        /// </summary>
        /// <remarks></remarks>
        public static event KeyboardEventHandler ApplicationKeyDown;

        /// <summary>
        /// Raised when key up occurs on application. Don't forget to initialize KeyboardHelpers on each platform.
        /// </summary>
        public static event KeyboardEventHandler ApplicationKeyUp;

        /// <summary>
        /// Raised when key down occurs on application. Don't forget to initialize KeyboardHelpers on each platform.
        /// </summary>
        /// <remarks></remarks>
        public static event KeyboardEventHandler ApplicationPreviewKeyDown;

        /// <summary>
        /// Raised when key up occurs on application. Don't forget to initialize KeyboardHelpers on each platform.
        /// </summary>
        public static event KeyboardEventHandler ApplicationPreviewKeyUp;

        /// <summary>
        /// Raised when key down occurs.
        /// </summary>
        public event KeyboardEventHandler KeyDown;

        /// <summary>
        /// Raised when key up occurs.
        /// </summary>
        public event KeyboardEventHandler KeyUp;

        /// <summary>
        /// Raised when key down occurs before it was handled by visual tree.
        /// </summary>
        public event KeyboardEventHandler PreviewKeyDown;

        /// <summary>
        /// Raised when key up occurs before it was handled by visual tree.
        /// </summary>
        public event KeyboardEventHandler PreviewKeyUp;

        /// <summary>
        /// Ask to the system to show the virtual keyboard. Don't forget to initialize KeyboardHelpers on each platform.
        /// </summary>
        /// <remarks>Virtual keyboard can't be shown by code with UWP.</remarks>
        public static void ShowVirtualKeyboard()
        {
            VirtualKeyboardRequested?.Invoke(null, EventArgs.Empty);
        }

        /// <summary>
        /// Ask to the system to hide the virtual keyboard. Don't forget to initialize KeyboardHelpers on each platform.
        /// </summary>
        /// <remarks>Virtual keyboard can't be shown or hide by code with UWP.</remarks>
        public static void HideVirtualKeyboard()
        {
            HideVirtualKeyboardRequested?.Invoke(null, EventArgs.Empty);
        }

        /// <summary>
        /// Raise the <see cref="ApplicationKeyDown"/> event.
        /// </summary>
        /// <param name="args">Argument of event</param>
        public static void OnApplicationKeyDown(KeyboardEventArgs args)
        {
            ApplicationKeyDown?.Invoke(null, args);
        }

        /// <summary>
        /// Raise the <see cref="ApplicationKeyUp"/> event.
        /// </summary>
        /// <param name="args">Argument of event</param>
        public static void OnApplicationKeyUp(KeyboardEventArgs args)
        {
            ApplicationKeyUp?.Invoke(null, args);
        }

        /// <summary>
        /// Raise the <see cref="ApplicationPreviewKeyDown"/> event.
        /// </summary>
        /// <param name="args">Argument of event</param>
        public static void OnApplicationPreviewKeyDown(KeyboardEventArgs args)
        {
            ApplicationPreviewKeyDown?.Invoke(null, args);
        }

        /// <summary>
        /// Raise the <see cref="ApplicationPreviewKeyUp"/> event.
        /// </summary>
        /// <param name="args">Argument of event</param>
        public static void OnApplicationPreviewKeyUp(KeyboardEventArgs args)
        {
            ApplicationPreviewKeyUp?.Invoke(null, args);
        }

        /// <summary>
        /// Raise the <see cref="KeyDown"/> event.
        /// </summary>
        /// <param name="args">Argument of event</param>
        public void OnKeyDown(KeyboardEventArgs args)
        {
            KeyDown?.Invoke(this, args);
        }

        /// <summary>
        /// Raise the <see cref="KeyUp"/> event.
        /// </summary>
        /// <param name="args">Argument of event</param>
        public void OnKeyUp(KeyboardEventArgs args)
        {
            KeyUp?.Invoke(this, args);
        }

        /// <summary>
        /// Raise the <see cref="PreviewKeyDown"/> event.
        /// </summary>
        /// <param name="args">Argument of event</param>
        public void OnPreviewKeyDown(KeyboardEventArgs args)
        {
            PreviewKeyDown?.Invoke(this, args);
        }

        /// <summary>
        /// Raise the <see cref="PreviewKeyUp"/> event.
        /// </summary>
        /// <param name="args">Argument of event</param>
        public void OnPreviewKeyUp(KeyboardEventArgs args)
        {
            PreviewKeyUp?.Invoke(this, args);
        }
    }
}