namespace Oxard.XControls.Events
{
    /// <summary>
    /// Store keyboard event data
    /// </summary>
    public class KeyboardEventArgs
    {
        /// <summary>
        /// Construtor
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="platformKey">The string representation of native key.</param>
        /// <param name="isShiftOn">True if shift is pressed.</param>
        /// <param name="isCapsLockOn">True if caps lock is active.</param>
        /// <param name="isControlOn">True if control is pressed.</param>
        /// <param name="isAltOn">True if alt is pressed.</param>
        public KeyboardEventArgs(Key key, string platformKey, bool isShiftOn, bool isCapsLockOn, bool isControlOn, bool isAltOn)
        {
            Key = key;
            PlatformKey = platformKey;
            IsShiftOn = isShiftOn;
            IsCapsLockOn = isCapsLockOn;
            IsControlOn = isControlOn;
            IsAltOn = isAltOn;
        }

        /// <summary>
        /// Get the key.
        /// </summary>
        public Key Key { get; }

        /// <summary>
        /// Get a string representation of native key.
        /// </summary>
        public string PlatformKey { get; }

        /// <summary>
        /// Get or set a value that indicates if key is handled.
        /// </summary>
        public bool Handled { get; set; }

        /// <summary>
        /// Get a value that indicates if shift key is pressed.
        /// </summary>
        public bool IsShiftOn { get; }

        /// <summary>
        /// Get a value that indicates if caps lock key is active.
        /// </summary>
        public bool IsCapsLockOn { get; }

        /// <summary>
        /// Get a value that indicates if control key is pressed.
        /// </summary>
        public bool IsControlOn { get; }

        /// <summary>
        /// Get a value that indicates if alt key is pressed.
        /// </summary>
        public bool IsAltOn { get; }
    }
}