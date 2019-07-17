using System;
using System.Windows.Input;
using Xamarin.Forms;

namespace Oxard.XControls.Components
{
    /// <summary>
    /// Inherits from <see cref="Button"/>. Manage a long press as a new command
    /// </summary>
    public class LongPressButton : Button
    {
        /// <summary>
        /// Identifies the LongPressedCommand dependency property.
        /// </summary>
        public static readonly BindableProperty LongPressedCommandProperty = BindableProperty.Create(nameof(LongPressedCommand), typeof(ICommand), typeof(LongPressButton));
        /// <summary>
        /// Identifies the LongPressedCommandParameter dependency property.
        /// </summary>
        public static readonly BindableProperty LongPressedCommandParameterProperty = BindableProperty.Create(nameof(LongPressedCommandParameter), typeof(object), typeof(LongPressButton));
        /// <summary>
        /// Identifies the LongPressedTime dependency property.
        /// </summary>
        public static readonly BindableProperty LongPressedTimeProperty = BindableProperty.Create(nameof(LongPressedTime), typeof(int), typeof(LongPressButton), 1000, propertyChanged: LongPressedDurationPropertyChanged);
        
        /// <summary>
        /// Default constructor
        /// </summary>
        public LongPressButton()
        {
            this.TouchManager.LongPressCancelClick = true;
            this.TouchManager.LongPressed += this.TouchManagerOnLongPressed;
        }

        /// <summary>
        /// Event launched when a long press is detected.
        /// </summary>
        public event EventHandler LongPressed;

        /// <summary>
        /// Get or set the command to execute on a long press
        /// </summary>
        public ICommand LongPressedCommand
        {
            get => (ICommand)this.GetValue(LongPressedCommandProperty);
            set => this.SetValue(LongPressedCommandProperty, value);
        }

        /// <summary>
        /// Get or set the parameter of <see cref="LongPressedCommand"/>
        /// </summary>
        public object LongPressedCommandParameter
        {
            get => this.GetValue(LongPressedCommandParameterProperty);
            set => this.SetValue(LongPressedCommandParameterProperty, value);
        }

        /// <summary>
        /// Get or set the time in millisecond of a long press
        /// </summary>
        public int LongPressedTime
        {
            get => (int)this.GetValue(LongPressedTimeProperty);
            set => this.SetValue(LongPressedTimeProperty, value);
        }

        /// <summary>
        /// Invoke the <see cref="LongPressed"/> event.
        /// </summary>
        protected virtual void OnLongPressed()
        {
            this.LongPressed?.Invoke(this, EventArgs.Empty);
        }

        private static void LongPressedDurationPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            (bindable as LongPressButton)?.LongPressedDurationChanged();
        }

        private void LongPressedDurationChanged()
        {
            this.TouchManager.LongPressTime = this.LongPressedTime;
        }

        private void TouchManagerOnLongPressed(object sender, Events.TouchEventArgs args)
        {
            if (!this.IsEnabled)
                return;

            if(this.LongPressedCommand != null)
            {
                if (this.LongPressedCommand.CanExecute(this.LongPressedCommandParameter))
                {
                    this.LongPressedCommand.Execute(this.LongPressedCommandParameter);
                    this.OnLongPressed();
                }
            }
            else
                this.OnLongPressed();
        }
    }
}
