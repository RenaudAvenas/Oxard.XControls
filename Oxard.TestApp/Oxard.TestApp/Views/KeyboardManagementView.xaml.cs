using Oxard.XControls.Events;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Oxard.TestApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class KeyboardManagementView : ContentView
    {
        public KeyboardManagementView()
        {
            InitializeComponent();
            KeyboardManager.ApplicationKeyDown += OnKeyboardManager_ApplicationKeyDown;
            KeyboardManager.ApplicationKeyUp += OnKeyboardManager_ApplicationKeyUp;
            KeyboardManager.ApplicationPreviewKeyDown += OnKeyboardManager_ApplicationPreviewKeyDown;
            KeyboardManager.ApplicationPreviewKeyUp += OnKeyboardManager_ApplicationPreviewKeyUp;
        }

        private void OnKeyboardManager_ApplicationKeyDown(object sender, KeyboardEventArgs args)
        {
            if (args.Key == Key.Other)
                KeyLabel.Text = $"Other Key ({args.PlatformKey}) is pressed.";
            else
                KeyLabel.Text = $"Key {args.Key} is pressed. Original key was : {args.PlatformKey}.";
        }

        private void OnKeyboardManager_ApplicationKeyUp(object sender, KeyboardEventArgs args)
        {
            if (args.Key == Key.Other)
                KeyLabel.Text = $"Other Key ({args.PlatformKey}) is released.";
            else
                KeyLabel.Text = $"Key {args.Key} is released. Original key was : {args.PlatformKey}.";
        }

        private void OnKeyboardManager_ApplicationPreviewKeyDown(object sender, KeyboardEventArgs args)
        {
            if (args.Key == Key.Other)
                PreviewKeyLabel.Text = $"Preview : Other Key ({args.PlatformKey}) is pressed.";
            else
                PreviewKeyLabel.Text = $"Preview : Key {args.Key} is pressed. Original key was : {args.PlatformKey}.";

            args.Handled = HandleEventsCheckBox.IsChecked;

            ModifiersLabel.Text = $"(Shift : {args.IsShiftOn}, Ctrl : {args.IsControlOn}, CapsLock : {args.IsCapsLockOn}, Alt : {args.IsAltOn})";
        }

        private void OnKeyboardManager_ApplicationPreviewKeyUp(object sender, KeyboardEventArgs args)
        {
            if (args.Key == Key.Other)
                PreviewKeyLabel.Text = $"Preview : Other Key ({args.PlatformKey}) is released.";
            else
                PreviewKeyLabel.Text = $"Preview : Key {args.Key} is released. Original key was : {args.PlatformKey}.";

            args.Handled = HandleEventsCheckBox.IsChecked;

            ModifiersLabel.Text = $"(Shift : {args.IsShiftOn}, Ctrl : {args.IsControlOn}, CapsLock : {args.IsCapsLockOn}, Alt : {args.IsAltOn})";
        }

        private void OnButton_ShowKeyboardClicked(object sender, EventArgs e)
        {
            KeyboardManager.ShowVirtualKeyboard();
        }

        private void OnButton_HideKeyboardClicked(object sender, EventArgs e)
        {
            KeyboardManager.HideVirtualKeyboard();
        }
    }
}