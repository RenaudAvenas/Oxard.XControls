namespace Oxard.XControls.Events
{
    /// <summary>
    /// Reference all keyboard keys
    /// </summary>
    public enum Key
    {
        /// <summary>
        /// Key specific to a platform and unknown on generic platform.
        /// </summary>
        Other,

        /// <summary>
        /// The backspace key.
        /// </summary>
        Backspace,

        /// <summary>
        /// The tabulation key.
        /// </summary>
        Tab,

        /// <summary>
        /// The enter key. Can be the validation button on virtual keyboard.
        /// </summary>
        Enter,

        /// <summary>
        /// The shift key without left or right precision.
        /// </summary>
        Shift,

        /// <summary>
        /// The left shift key.
        /// </summary>
        LeftShift,

        /// <summary>
        /// The right shift key.
        /// </summary>
        RightShift,

        /// <summary>
        /// The control key without left or right precision.
        /// </summary>
        Control,

        /// <summary>
        /// The left control key.
        /// </summary>
        LeftControl,

        /// <summary>
        /// The right control key.
        /// </summary>
        RightControl,

        /// <summary>
        /// The menu key.
        /// </summary>
        Menu,

        /// <summary>
        /// The alt key without left or right precision.
        /// </summary>
        Alt,

        /// <summary>
        /// The left alt key.
        /// </summary>
        LeftAlt,

        /// <summary>
        /// The right alt key.
        /// </summary>
        RightAlt,

        /// <summary>
        /// The capital lock key.
        /// </summary>
        CapitalLock,

        /// <summary>
        /// The escape key.
        /// </summary>
        Escape,

        /// <summary>
        /// The space key.
        /// </summary>
        Space,

        /// <summary>
        /// The page up key.
        /// </summary>
        PageUp,

        /// <summary>
        /// The page down key.
        /// </summary>
        PageDown,

        /// <summary>
        /// The end key.
        /// </summary>
        End,

        /// <summary>
        /// The start key.
        /// </summary>
        Home,

        /// <summary>
        /// The left arrow key.
        /// </summary>
        Left,

        /// <summary>
        /// The right arrow key.
        /// </summary>
        Right,

        /// <summary>
        /// The up arrow key.
        /// </summary>
        Up,

        /// <summary>
        /// The down arrow key.
        /// </summary>
        Down,

        /// <summary>
        /// The insert key.
        /// </summary>
        Insert,

        /// <summary>
        /// The delete key.
        /// </summary>
        Delete,

        /// <summary>
        /// The help key.
        /// </summary>
        Help,

        /// <summary>
        /// The 0 key.
        /// </summary>
        Number0,

        /// <summary>
        /// The 1 key.
        /// </summary>
        Number1,

        /// <summary>
        /// The 2 key.
        /// </summary>
        Number2,

        /// <summary>
        /// The 3 key.
        /// </summary>
        Number3,

        /// <summary>
        /// The 4 key.
        /// </summary>
        Number4,

        /// <summary>
        /// The 5 key.
        /// </summary>
        Number5,

        /// <summary>
        /// The 6 key.
        /// </summary>
        Number6,

        /// <summary>
        /// The 7 key.
        /// </summary>
        Number7,

        /// <summary>
        /// The 8 key.
        /// </summary>
        Number8,

        /// <summary>
        /// The 9 key.
        /// </summary>
        Number9,

        /// <summary>
        /// The A key.
        /// </summary>
        A,

        /// <summary>
        /// The B key.
        /// </summary>
        B,

        /// <summary>
        /// The C key.
        /// </summary>
        C,

        /// <summary>
        /// The D key.
        /// </summary>
        D,

        /// <summary>
        /// The E key.
        /// </summary>
        E,

        /// <summary>
        /// The F key.
        /// </summary>
        F,

        /// <summary>
        /// The G key.
        /// </summary>
        G,

        /// <summary>
        /// The H key.
        /// </summary>
        H,

        /// <summary>
        /// The I key.
        /// </summary>
        I,

        /// <summary>
        /// The J key.
        /// </summary>
        J,

        /// <summary>
        /// The K key.
        /// </summary>
        K,

        /// <summary>
        /// The L key.
        /// </summary>
        L,

        /// <summary>
        /// The M key.
        /// </summary>
        M,

        /// <summary>
        /// The N key.
        /// </summary>
        N,

        /// <summary>
        /// The O key.
        /// </summary>
        O,

        /// <summary>
        /// The P key.
        /// </summary>
        P,

        /// <summary>
        /// The Q key.
        /// </summary>
        Q,

        /// <summary>
        /// The R key.
        /// </summary>
        R,

        /// <summary>
        /// The S key.
        /// </summary>
        S,

        /// <summary>
        /// The T key.
        /// </summary>
        T,

        /// <summary>
        /// The U key.
        /// </summary>
        U,

        /// <summary>
        /// The V key.
        /// </summary>
        V,

        /// <summary>
        /// The W key.
        /// </summary>
        W,

        /// <summary>
        /// The X key.
        /// </summary>
        X,

        /// <summary>
        /// The Y key.
        /// </summary>
        Y,

        /// <summary>
        /// The Z key.
        /// </summary>
        Z,

        /// <summary>
        /// The numeric pad 0  key.
        /// </summary>
        NumberPad0,

        /// <summary>
        /// The numeric pad 1 key.
        /// </summary>
        NumberPad1,

        /// <summary>
        /// The numeric pad 2 key.
        /// </summary>
        NumberPad2,

        /// <summary>
        /// The numeric pad 3 key.
        /// </summary>
        NumberPad3,

        /// <summary>
        /// The numeric pad 4 key.
        /// </summary>
        NumberPad4,

        /// <summary>
        /// The numeric pad 5 key.
        /// </summary>
        NumberPad5,

        /// <summary>
        /// The numeric pad 6 key.
        /// </summary>
        NumberPad6,

        /// <summary>
        /// The numeric pad 7 key.
        /// </summary>
        NumberPad7,

        /// <summary>
        /// The numeric pad 8 key.
        /// </summary>
        NumberPad8,

        /// <summary>
        /// The numeric pad 9 key.
        /// </summary>
        NumberPad9,

        /// <summary>
        /// The * key.
        /// </summary>
        Multiply,

        /// <summary>
        /// The + key.
        /// </summary>
        Add,

        /// <summary>
        /// The - numeric pad key.
        /// </summary>
        Subtract,

        /// <summary>
        /// The decimal numeric pad key (. or ,).
        /// </summary>
#pragma warning disable CA1720 // L'identificateur contient le nom de type
        Decimal,
#pragma warning restore CA1720 // L'identificateur contient le nom de type

        /// <summary>
        /// The / key.
        /// </summary>
        Divide,

        /// <summary>
        /// The F1 function key.
        /// </summary>
        F1,

        /// <summary>
        /// The F2 function key.
        /// </summary>
        F2,

        /// <summary>
        /// The F3 function key.
        /// </summary>
        F3,

        /// <summary>
        /// The F4 function key.
        /// </summary>
        F4,

        /// <summary>
        /// The F5 function key.
        /// </summary>
        F5,

        /// <summary>
        /// The F6 function key.
        /// </summary>
        F6,

        /// <summary>
        /// The F7 function key.
        /// </summary>
        F7,

        /// <summary>
        /// The F8 function key.
        /// </summary>
        F8,

        /// <summary>
        /// The F9 function key.
        /// </summary>
        F9,

        /// <summary>
        /// The F10 function key.
        /// </summary>
        F10,

        /// <summary>
        /// The F11 function key.
        /// </summary>
        F11,

        /// <summary>
        /// The F12 function key.
        /// </summary>
        F12,

        /// <summary>
        /// The number lock key.
        /// </summary>
        NumberKeyLock,

        /// <summary>
        /// The , key
        /// </summary>
        Comma
    }
}