using Oxard.XControls.Events;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using Windows.System;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Input;

namespace Oxard.XControls.UWP.Events
{
    /// <summary>
    /// Keyboard helper for UWP
    /// </summary>
    public class KeyboardHelper
    {
        private static Dictionary<VirtualKey, Key> keysMap = new Dictionary<VirtualKey, Key>
        {
            { VirtualKey.Back, Key.Backspace },
            { VirtualKey.Tab, Key.Tab },
            { VirtualKey.Enter, Key.Enter },
            { VirtualKey.Accept, Key.Enter },
            { VirtualKey.Shift, Key.Shift },
            { VirtualKey.LeftShift, Key.LeftShift },
            { VirtualKey.RightShift, Key.RightShift },
            { VirtualKey.Control, Key.Control },
            { VirtualKey.LeftControl, Key.LeftControl },
            { VirtualKey.RightControl, Key.RightControl },
            { VirtualKey.Application, Key.Menu },
            { VirtualKey.Menu, Key.Alt },
            { VirtualKey.LeftMenu, Key.LeftAlt },
            { VirtualKey.RightMenu, Key.RightAlt },
            { VirtualKey.CapitalLock, Key.CapitalLock },
            { VirtualKey.Escape, Key.Escape },
            { VirtualKey.Space, Key.Space },
            { VirtualKey.PageUp, Key.PageUp },
            { VirtualKey.PageDown, Key.PageDown },
            { VirtualKey.End, Key.End },
            { VirtualKey.Home, Key.Home },
            { VirtualKey.Left, Key.Left },
            { VirtualKey.Right, Key.Right },
            { VirtualKey.Up, Key.Up },
            { VirtualKey.Down, Key.Down },
            { VirtualKey.Insert, Key.Insert },
            { VirtualKey.Delete, Key.Delete },
            { VirtualKey.Help, Key.Help },
            { VirtualKey.Number0, Key.Number0 },
            { VirtualKey.Number1, Key.Number1 },
            { VirtualKey.Number2, Key.Number2 },
            { VirtualKey.Number3, Key.Number3 },
            { VirtualKey.Number4, Key.Number4 },
            { VirtualKey.Number5, Key.Number5 },
            { VirtualKey.Number6, Key.Number6 },
            { VirtualKey.Number7, Key.Number7 },
            { VirtualKey.Number8, Key.Number8 },
            { VirtualKey.Number9, Key.Number9 },
            { VirtualKey.A, Key.A },
            { VirtualKey.B, Key.B },
            { VirtualKey.C, Key.C },
            { VirtualKey.D, Key.D },
            { VirtualKey.E, Key.E },
            { VirtualKey.F, Key.F },
            { VirtualKey.G, Key.G },
            { VirtualKey.H, Key.H },
            { VirtualKey.I, Key.I },
            { VirtualKey.J, Key.J },
            { VirtualKey.K, Key.K },
            { VirtualKey.L, Key.L },
            { VirtualKey.M, Key.M },
            { VirtualKey.N, Key.N },
            { VirtualKey.O, Key.O },
            { VirtualKey.P, Key.P },
            { VirtualKey.Q, Key.Q },
            { VirtualKey.R, Key.R },
            { VirtualKey.S, Key.S },
            { VirtualKey.T, Key.T },
            { VirtualKey.U, Key.U },
            { VirtualKey.V, Key.V },
            { VirtualKey.W, Key.W },
            { VirtualKey.X, Key.X },
            { VirtualKey.Y, Key.Y },
            { VirtualKey.Z, Key.Z },
            { VirtualKey.NumberPad0, Key.NumberPad0 },
            { VirtualKey.NumberPad1, Key.NumberPad1 },
            { VirtualKey.NumberPad2, Key.NumberPad2 },
            { VirtualKey.NumberPad3, Key.NumberPad3 },
            { VirtualKey.NumberPad4, Key.NumberPad4 },
            { VirtualKey.NumberPad5, Key.NumberPad5 },
            { VirtualKey.NumberPad6, Key.NumberPad6 },
            { VirtualKey.NumberPad7, Key.NumberPad7 },
            { VirtualKey.NumberPad8, Key.NumberPad8 },
            { VirtualKey.NumberPad9, Key.NumberPad9 },
            { VirtualKey.Multiply, Key.Multiply },
            { VirtualKey.Add, Key.Add },
            { VirtualKey.Subtract, Key.Subtract },
            { VirtualKey.Decimal, Key.Decimal },
            { VirtualKey.Divide, Key.Divide },
            { VirtualKey.F1, Key.F1 },
            { VirtualKey.F2, Key.F2 },
            { VirtualKey.F3, Key.F3 },
            { VirtualKey.F4, Key.F4 },
            { VirtualKey.F5, Key.F5 },
            { VirtualKey.F6, Key.F6 },
            { VirtualKey.F7, Key.F7 },
            { VirtualKey.F8, Key.F8 },
            { VirtualKey.F9, Key.F9 },
            { VirtualKey.F10, Key.F10 },
            { VirtualKey.F11, Key.F11 },
            { VirtualKey.F12, Key.F12 },
            { VirtualKey.NumberKeyLock, Key.NumberKeyLock },
            { (VirtualKey)188, Key.Comma }
        };

        private readonly KeyboardManager keyboardManager;
        private bool lastAltState;

        public KeyboardHelper(KeyboardManager keyboardManager, UIElement control)
        {
            this.keyboardManager = keyboardManager;
            control.KeyDown += OnControlKeyDown;
            control.KeyUp += OnControlKeyUp;
            control.PreviewKeyDown += OnControlPreviewKeyDown;
            control.PreviewKeyUp += OnControlPreviewKeyUp;
        }

        private KeyboardHelper()
            : this(null, Window.Current.Content)
        {
        }

        public static void Initialize()
        {
            _ = new KeyboardHelper();
        }

        private void OnDispatcher_AcceleratorKeyActivated(CoreDispatcher sender, AcceleratorKeyEventArgs args)
        {
            if (args.VirtualKey == VirtualKey.Menu && (args.EventType & CoreAcceleratorKeyEventType.KeyUp) > 0)
            {
                if (lastAltState == false)
                    return;

                Window.Current.Dispatcher.AcceleratorKeyActivated -= OnDispatcher_AcceleratorKeyActivated;

                var keyboardEventArgs = ToKeyboardEventArgs(VirtualKey.Menu, false);
                if (keyboardManager != null)
                    this.keyboardManager.OnPreviewKeyUp(keyboardEventArgs);
                else
                    KeyboardManager.OnApplicationPreviewKeyUp(keyboardEventArgs);

                if (keyboardEventArgs.Handled)
                {
                    args.Handled = true;
                    return;
                }

                if (keyboardManager != null)
                    this.keyboardManager.OnKeyUp(keyboardEventArgs);
                else
                    KeyboardManager.OnApplicationKeyUp(keyboardEventArgs);

                args.Handled = keyboardEventArgs.Handled;
            }
        }

        private void OnControlKeyDown(object sender, KeyRoutedEventArgs e)
        {
            var keyboardArgs = ToKeyboardEventArgs(e.Key, true);
            if (keyboardManager != null)
                this.keyboardManager.OnKeyDown(keyboardArgs);
            else
                KeyboardManager.OnApplicationKeyDown(keyboardArgs);
            e.Handled = keyboardArgs.Handled;
        }

        private void OnControlKeyUp(object sender, KeyRoutedEventArgs e)
        {
            var keyboardArgs = ToKeyboardEventArgs(e.Key, false);
            if (keyboardManager != null)
                this.keyboardManager.OnKeyUp(keyboardArgs);
            else
                KeyboardManager.OnApplicationKeyUp(keyboardArgs);
            e.Handled = keyboardArgs.Handled;
        }

        private void OnControlPreviewKeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == VirtualKey.Menu)
                Window.Current.Dispatcher.AcceleratorKeyActivated += OnDispatcher_AcceleratorKeyActivated;

            var keyboardArgs = ToKeyboardEventArgs(e.Key, true);
            if (keyboardManager != null)
                this.keyboardManager.OnPreviewKeyDown(keyboardArgs);
            else
                KeyboardManager.OnApplicationPreviewKeyDown(keyboardArgs);
            e.Handled = keyboardArgs.Handled;
        }

        private void OnControlPreviewKeyUp(object sender, KeyRoutedEventArgs e)
        {
            var keyboardArgs = ToKeyboardEventArgs(e.Key, false);
            if (keyboardManager != null)
                this.keyboardManager.OnPreviewKeyUp(keyboardArgs);
            else
                KeyboardManager.OnApplicationPreviewKeyUp(keyboardArgs);
            e.Handled = keyboardArgs.Handled;
        }

        private KeyboardEventArgs ToKeyboardEventArgs(VirtualKey key, bool isDownKeyEvent)
        {
            var currentWindow = CoreWindow.GetForCurrentThread();

            if (key == VirtualKey.Menu)
                lastAltState = isDownKeyEvent;

            return new KeyboardEventArgs(
                ToKey(key),
                key.ToString(),
                (currentWindow.GetKeyState(VirtualKey.Shift) & CoreVirtualKeyStates.Down) > 0 || key == VirtualKey.Shift && isDownKeyEvent,
                (currentWindow.GetKeyState(VirtualKey.CapitalLock) & CoreVirtualKeyStates.Locked) > 0 || key == VirtualKey.CapitalLock && isDownKeyEvent,
                (currentWindow.GetKeyState(VirtualKey.Control) & CoreVirtualKeyStates.Down) > 0 || key == VirtualKey.Control && isDownKeyEvent,
                lastAltState);
        }

        private static Key ToKey(VirtualKey virtualKey)
        {
            return keysMap.ContainsKey(virtualKey) ? keysMap[virtualKey] : Key.Other;
        }
    }
}