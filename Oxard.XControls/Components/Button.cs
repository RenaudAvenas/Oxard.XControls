using Oxard.XControls.Events;
using System;
using System.Windows.Input;
using Xamarin.Forms;

namespace Oxard.XControls.Components
{
    public class Button : ContentControl
    {
        public static readonly BindableProperty CommandProperty = BindableProperty.Create(nameof(Command), typeof(ICommand), typeof(Button), propertyChanged: CommandPropertyChanged);
        public static readonly BindableProperty CommandParameterProperty = BindableProperty.Create(nameof(CommandParameter), typeof(object), typeof(Button), propertyChanged: CommandParameterPropertyChanged);
        public static readonly BindablePropertyKey IsPressedPropertyKey = BindableProperty.CreateReadOnly(nameof(IsPressed), typeof(bool), typeof(Button), false);
        public static BindableProperty IsPressedProperty = IsPressedPropertyKey.BindableProperty;

        public Button()
        {
            this.TouchManager = new TouchManager();
            this.TouchManager.TouchDown += this.TouchManagerOnTouchDown;
            this.TouchManager.TouchUp += this.TouchManagerOnTouchUp;
            this.TouchManager.TouchEnter += this.TouchManagerOnTouchEnter;
            this.TouchManager.TouchLeave += this.TouchManagerOnTouchLeave;
            this.TouchManager.TouchCanceled += this.TouchManagerOnTouchCanceled;
            this.TouchManager.Clicked += this.TouchManagerOnClicked;
        }

        /// <summary>
        /// Get event that is invoked when Button is clicked
        /// </summary>
        public event EventHandler Clicked;

        /// <summary>
        /// Get or set a command for this button. Command will be executed when button is clicked
        /// </summary>
        public ICommand Command
        {
            get { return (ICommand)this.GetValue(CommandProperty); }
            set { this.SetValue(CommandProperty, value); }
        }

        /// <summary>
        /// Get or set the command parameter to use with the <see cref="Command"/>
        /// </summary>
        public object CommandParameter
        {
            get { return (object)this.GetValue(CommandParameterProperty); }
            set { this.SetValue(CommandParameterProperty, value); }
        }

        /// <summary>
        /// Get the press state of the button
        /// </summary>
        public bool IsPressed
        {
            get { return (bool)this.GetValue(IsPressedProperty); }
            private set { this.SetValue(IsPressedPropertyKey, value); }
        }

        /// <summary>
        /// Get the <see cref="TouchManager"/> used by the button to detect events
        /// </summary>
        public TouchManager TouchManager { get; }

        /// <summary>
        /// Invoke <see cref="Clicked"/> event and call Execute method of <see cref="Command"/> property
        /// </summary>
        protected virtual void OnClicked()
        {
            this.Clicked?.Invoke(this, EventArgs.Empty);
            if (this.Command?.CanExecute(this.CommandParameter) == true)
                this.Command.Execute(this.CommandParameter);
        }

        private static void CommandPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            (bindable as Button)?.CommandChanged(oldValue as ICommand);
        }

        private static void CommandParameterPropertyChanged(BindableObject bindable, object oldvalue, object newvalue)
        {
            (bindable as Button)?.CommandParameterChanged();
        }

        private void CommandChanged(ICommand oldValue)
        {
            if (oldValue != null)
            {
                oldValue.CanExecuteChanged -= this.CommandOnCanExecuteChanged;
                this.IsEnabled = true;
            }

            if (this.Command != null)
            {
                this.IsEnabled = this.Command.CanExecute(this.CommandParameter);
                this.Command.CanExecuteChanged += this.CommandOnCanExecuteChanged;
            }
        }

        private void CommandParameterChanged()
        {
            this.IsEnabled = (this.Command?.CanExecute(this.CommandParameter)).GetValueOrDefault(true);
        }

        private void CommandOnCanExecuteChanged(object sender, EventArgs eventArgs)
        {
            this.IsEnabled = this.Command.CanExecute(this.CommandParameter);
        }

        private void TouchManagerOnTouchDown(object sender, TouchEventArgs args)
        {
            if (!this.IsEnabled)
            {
                this.TouchManager.DisableCurrentTouch();
                return;
            }

            this.IsPressed = true;
        }

        private void TouchManagerOnTouchUp(object sender, TouchEventArgs args)
        {
            this.IsPressed = false;
        }

        private void TouchManagerOnTouchLeave(object sender, TouchEventArgs args)
        {
            this.IsPressed = false;
        }

        private void TouchManagerOnTouchEnter(object sender, TouchEventArgs args)
        {
            if (this.TouchManager.IsClicking)
                this.IsPressed = true;
        }

        private void TouchManagerOnTouchCanceled(object sender, TouchEventArgs args)
        {
            this.IsPressed = false;
        }

        private void TouchManagerOnClicked(object sender, EventArgs e)
        {
            this.OnClicked();
        }
    }
}
