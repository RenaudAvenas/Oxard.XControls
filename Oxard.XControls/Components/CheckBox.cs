using Xamarin.Forms;

namespace Oxard.XControls.Components
{
    public class CheckBox : Button
    {
        public static readonly BindableProperty IsCheckedProperty = BindableProperty.Create(nameof(IsChecked), typeof(bool), typeof(CheckBox), false, defaultBindingMode: BindingMode.TwoWay);

        public bool IsChecked
        {
            get => (bool)this.GetValue(IsCheckedProperty);
            set => this.SetValue(IsCheckedProperty, value);
        }

        protected override void OnClicked()
        {
            base.OnClicked();

            this.IsChecked = !this.IsChecked;
        }
    }
}
