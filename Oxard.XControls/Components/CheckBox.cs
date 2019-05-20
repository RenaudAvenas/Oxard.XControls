using Xamarin.Forms;

namespace Oxard.XControls.Components
{
    public class CheckBox : Button
    {
        public static readonly BindableProperty IsCheckedProperty = BindableProperty.Create(nameof(IsChecked), typeof(bool), typeof(CheckBox), false, defaultBindingMode: BindingMode.TwoWay);

        /// <summary>
        /// Get or set if the CheckBox is checked or not
        /// </summary>
        public bool IsChecked
        {
            get => (bool)this.GetValue(IsCheckedProperty);
            set => this.SetValue(IsCheckedProperty, value);
        }

        /// <summary>
        /// Invoke <see cref="Clicked"/> event and call Execute method of <see cref="Command"/> property. This method change the state of the CheckBox (see <see cref="IsChecked"/> property)
        /// </summary>
        protected override void OnClicked()
        {
            base.OnClicked();

            this.IsChecked = !this.IsChecked;
            this.OnIsCheckedChanged();
        }

        /// <summary>
        /// Called when <see cref="IsChecked"/> property has changed
        /// </summary>
        protected virtual void OnIsCheckedChanged()
        {
        }
    }
}
