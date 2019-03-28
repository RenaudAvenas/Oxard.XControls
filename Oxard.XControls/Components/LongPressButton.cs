using System;
using System.Windows.Input;
using Xamarin.Forms;

namespace Oxard.XControls.Components
{
    public class LongPressButton : Button
    {
        public static readonly BindableProperty LongPressedCommandProperty = BindableProperty.Create(nameof(LongPressedCommand), typeof(ICommand), typeof(LongPressButton));
        public static readonly BindableProperty LongPressedCommandParameterProperty = BindableProperty.Create(nameof(LongPressedCommandParameter), typeof(object), typeof(LongPressButton));
        public static readonly BindableProperty LongPressedTimeProperty = BindableProperty.Create(nameof(LongPressedTime), typeof(int), typeof(LongPressButton), 1000, propertyChanged: LongPressedDurationPropertyChanged);
        
        public LongPressButton()
        {
            this.TouchManager.LongPressCancelClick = true;
            this.TouchManager.LongPressed += this.TouchManagerOnLongPressed;
        }

        public event EventHandler LongPressed;

        public ICommand LongPressedCommand
        {
            get => (ICommand)this.GetValue(LongPressedCommandProperty);
            set => this.SetValue(LongPressedCommandProperty, value);
        }

        public object LongPressedCommandParameter
        {
            get => this.GetValue(LongPressedCommandParameterProperty);
            set => this.SetValue(LongPressedCommandParameterProperty, value);
        }

        public int LongPressedTime
        {
            get => (int)this.GetValue(LongPressedTimeProperty);
            set => this.SetValue(LongPressedTimeProperty, value);
        }

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
