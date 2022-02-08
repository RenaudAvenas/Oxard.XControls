using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Views.InputMethods;
using Oxard.XControls.Events;
using System;
using System.Collections.Generic;
using static Android.Views.View;
using Keycode = Android.Views.Keycode;

namespace Oxard.XControls.Droid.Events
{
    /// <summary>
    /// Keyboard helper for Android
    /// </summary>
    public class KeyboardHelper
    {
        private static readonly Dictionary<Keycode, Key> keysMap = new Dictionary<Keycode, Key>
        {
            { Keycode.Back, Key.Backspace },
            { Keycode.Tab, Key.Tab },
            { Keycode.Enter, Key.Enter },
            { Keycode.NumpadEnter, Key.Enter },
            { Keycode.ShiftLeft, Key.LeftShift },
            { Keycode.ShiftRight, Key.RightShift },
            { Keycode.CtrlLeft, Key.LeftControl },
            { Keycode.CtrlRight, Key.RightControl },
            { Keycode.Menu, Key.Menu },
            { Keycode.AltLeft, Key.LeftAlt },
            { Keycode.AltRight, Key.RightAlt },
            { Keycode.CapsLock, Key.CapitalLock },
            { Keycode.Escape, Key.Escape },
            { Keycode.Space, Key.Space },
            { Keycode.PageUp, Key.PageUp },
            { Keycode.PageDown, Key.PageDown },
            { Keycode.DpadLeft, Key.Left },
            { Keycode.DpadRight, Key.Right },
            { Keycode.DpadUp, Key.Up },
            { Keycode.DpadDown, Key.Down },
            { Keycode.Insert, Key.Insert },
            { Keycode.Del, Key.Delete },
            { Keycode.Help, Key.Help },
            { Keycode.Num0, Key.Number0 },
            { Keycode.Num1, Key.Number1 },
            { Keycode.Num2, Key.Number2 },
            { Keycode.Num3, Key.Number3 },
            { Keycode.Num4, Key.Number4 },
            { Keycode.Num5, Key.Number5 },
            { Keycode.Num6, Key.Number6 },
            { Keycode.Num7, Key.Number7 },
            { Keycode.Num8, Key.Number8 },
            { Keycode.Num9, Key.Number9 },
            { Keycode.A, Key.A },
            { Keycode.B, Key.B },
            { Keycode.C, Key.C },
            { Keycode.D, Key.D },
            { Keycode.E, Key.E },
            { Keycode.F, Key.F },
            { Keycode.G, Key.G },
            { Keycode.H, Key.H },
            { Keycode.I, Key.I },
            { Keycode.J, Key.J },
            { Keycode.K, Key.K },
            { Keycode.L, Key.L },
            { Keycode.M, Key.M },
            { Keycode.N, Key.N },
            { Keycode.O, Key.O },
            { Keycode.P, Key.P },
            { Keycode.Q, Key.Q },
            { Keycode.R, Key.R },
            { Keycode.S, Key.S },
            { Keycode.T, Key.T },
            { Keycode.U, Key.U },
            { Keycode.V, Key.V },
            { Keycode.W, Key.W },
            { Keycode.X, Key.X },
            { Keycode.Y, Key.Y },
            { Keycode.Z, Key.Z },
            { Keycode.Numpad0, Key.NumberPad0 },
            { Keycode.Numpad1, Key.NumberPad1 },
            { Keycode.Numpad2, Key.NumberPad2 },
            { Keycode.Numpad3, Key.NumberPad3 },
            { Keycode.Numpad4, Key.NumberPad4 },
            { Keycode.Numpad5, Key.NumberPad5 },
            { Keycode.Numpad6, Key.NumberPad6 },
            { Keycode.Numpad7, Key.NumberPad7 },
            { Keycode.Numpad8, Key.NumberPad8 },
            { Keycode.Numpad9, Key.NumberPad9 },
            { Keycode.NumpadMultiply, Key.Multiply },
            { Keycode.NumpadAdd, Key.Add },
            { Keycode.NumpadSubtract, Key.Subtract },
            { Keycode.NumpadDot, Key.Decimal },
            { Keycode.NumpadComma, Key.Decimal },
            { Keycode.NumpadDivide, Key.Divide },
            { Keycode.F1, Key.F1 },
            { Keycode.F2, Key.F2 },
            { Keycode.F3, Key.F3 },
            { Keycode.F4, Key.F4 },
            { Keycode.F5, Key.F5 },
            { Keycode.F6, Key.F6 },
            { Keycode.F7, Key.F7 },
            { Keycode.F8, Key.F8 },
            { Keycode.F9, Key.F9 },
            { Keycode.F10, Key.F10 },
            { Keycode.F11, Key.F11 },
            { Keycode.F12, Key.F12 },
            { Keycode.NumLock, Key.NumberKeyLock },
            { Keycode.Comma, Key.Comma }
        };

        private static Activity activity;
        private readonly KeyboardManager keyboardManager;
        private readonly IKeyboardListener view;
        private readonly bool isStaticInstance;

        public KeyboardHelper(KeyboardManager keyboardManager, IKeyboardListener view)
        {
            this.keyboardManager = keyboardManager;
            this.view = view;
            this.AttachToKeyboardListener();
        }

        private KeyboardHelper(IKeyboardListener view)
        {
            isStaticInstance = true;
            this.view = view;
            this.AttachToKeyboardListener();
        }

        public static void Initialize(Activity mainActivity)
        {
            activity = mainActivity;
            KeyboardManager.VirtualKeyboardRequested += OnKeyboardManagerVirtualKeyboardRequested;
            KeyboardManager.HideVirtualKeyboardRequested += OnKeyboardManagerHideVirtualKeyboardRequested;

            if (!(mainActivity is IKeyboardListener keyboardListener))
                throw new NotSupportedException("Activity must implements IKeyboardListener interface.");

            // Initialize a keyboard helper on main window
            _ = new KeyboardHelper(keyboardListener);
        }

        private static void OnKeyboardManagerVirtualKeyboardRequested(object sender, EventArgs e)
        {
            InputMethodManager imm = (InputMethodManager)Application.Context.GetSystemService(Context.InputMethodService);

            imm.ShowSoftInput(activity.Window.DecorView, ShowFlags.Forced);
        }

        private static void OnKeyboardManagerHideVirtualKeyboardRequested(object sender, EventArgs e)
        {
            InputMethodManager imm = (InputMethodManager)Application.Context.GetSystemService(Context.InputMethodService);

            imm.HideSoftInputFromWindow(activity.Window.DecorView.WindowToken, HideSoftInputFlags.None);
        }

        private void AttachToKeyboardListener()
        {
            this.view.PreviewKeyEvent += OnViewPreviewKeyEvent;
            this.view.KeyEvent += OnViewKeyEvent;
        }

        private void OnViewPreviewKeyEvent(object sender, AndroidKeyboardEventArgs e)
        {
            if (e.KeyEvent.Action == KeyEventActions.Up)
                e.Handled = OnPreviewKeyUp(e.KeyEvent).Handled;
            else if (e.KeyEvent.Action == KeyEventActions.Down)
                e.Handled = OnPreviewKeyDown(e.KeyEvent).Handled;
        }

        private void OnViewKeyEvent(object sender, AndroidKeyboardEventArgs e)
        {
            if (e.KeyEvent.Action == KeyEventActions.Up)
                e.Handled = OnKeyUp(e.KeyEvent).Handled;
            else if (e.KeyEvent.Action == KeyEventActions.Down)
                e.Handled = OnKeyDown(e.KeyEvent).Handled;
        }

        private KeyboardEventArgs OnKeyDown(KeyEvent keyEvent)
        {
            var eventArgs = ToKeyboardEventArgs(keyEvent);
            if (isStaticInstance)
                KeyboardManager.OnApplicationKeyDown(eventArgs);
            else
                this.keyboardManager.OnKeyDown(eventArgs);

            return eventArgs;
        }

        private KeyboardEventArgs OnKeyUp(KeyEvent keyEvent)
        {
            var eventArgs = ToKeyboardEventArgs(keyEvent);
            if (isStaticInstance)
                KeyboardManager.OnApplicationKeyUp(eventArgs);
            else
                this.keyboardManager.OnKeyUp(eventArgs);

            return eventArgs;
        }

        private KeyboardEventArgs OnPreviewKeyDown(KeyEvent keyEvent)
        {
            var eventArgs = ToKeyboardEventArgs(keyEvent);
            if (isStaticInstance)
                KeyboardManager.OnApplicationPreviewKeyDown(eventArgs);
            else
                this.keyboardManager.OnPreviewKeyDown(eventArgs);

            return eventArgs;
        }

        private KeyboardEventArgs OnPreviewKeyUp(KeyEvent keyEvent)
        {
            var eventArgs = ToKeyboardEventArgs(keyEvent);
            if (isStaticInstance)
                KeyboardManager.OnApplicationPreviewKeyUp(eventArgs);
            else
                this.keyboardManager.OnPreviewKeyUp(eventArgs);

            return eventArgs;
        }

        private static KeyboardEventArgs ToKeyboardEventArgs(KeyEvent keyEvent)
        {
            return new KeyboardEventArgs(
                ToKey(keyEvent.KeyCode),
                keyEvent.KeyCode.ToString(),
                keyEvent.IsShiftPressed,
                keyEvent.IsCapsLockOn,
                keyEvent.IsCtrlPressed,
                keyEvent.IsAltPressed);
        }

        private static Key ToKey(Keycode keycode)
        {
            return keysMap.ContainsKey(keycode) ? keysMap[keycode] : Key.Other;
        }
    }
}