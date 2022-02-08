using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Oxard.XControls.Droid.Events;
using System;

namespace Oxard.TestApp.Droid
{
    [Activity(Label = "Oxard.TestApp", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity, IKeyboardListener
    {
        public event EventHandler<AndroidKeyboardEventArgs> PreviewKeyEvent;

        public event EventHandler<AndroidKeyboardEventArgs> KeyEvent;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);
            global::Xamarin.Forms.Forms.SetFlags("Shapes_Experimental");
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            KeyboardHelper.Initialize(this);

            LoadApplication(new App());
        }

        public override bool DispatchKeyEvent(KeyEvent e)
        {
            var androidKeyboardEventArgs = new AndroidKeyboardEventArgs(e);
            PreviewKeyEvent?.Invoke(this, androidKeyboardEventArgs);
            return androidKeyboardEventArgs.Handled == true ? true : base.DispatchKeyEvent(e);
        }

        public override bool OnKeyDown([GeneratedEnum] Keycode keyCode, KeyEvent e)
        {
            var androidKeyboardEventArgs = new AndroidKeyboardEventArgs(e);
            KeyEvent?.Invoke(this, androidKeyboardEventArgs);
            return androidKeyboardEventArgs.Handled == true ? true : base.OnKeyDown(keyCode, e);
        }

        public override bool OnKeyUp([GeneratedEnum] Keycode keyCode, KeyEvent e)
        {
            var androidKeyboardEventArgs = new AndroidKeyboardEventArgs(e);
            KeyEvent?.Invoke(this, androidKeyboardEventArgs);
            return androidKeyboardEventArgs.Handled == true ? true : base.OnKeyUp(keyCode, e);
        }
    }
}