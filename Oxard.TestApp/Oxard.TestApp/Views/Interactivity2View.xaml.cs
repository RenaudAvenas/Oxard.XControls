using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Oxard.TestApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Interactivity2View : ContentView
    {
        private bool isDataBindingActivated;

        public bool IsDataBindingActivated
        {
            get => this.isDataBindingActivated;
            set
            {
                if(this.isDataBindingActivated != value)
                {
                    this.isDataBindingActivated = value;
                    this.OnPropertyChanged();
                }
            }
        }

        public Interactivity2View()
        {
            this.BindingContext = this;
            InitializeComponent();
        }

        private void ButtonOnSwitchEnableClicked(object sender, EventArgs e)
        {
            this.GridContainer.IsEnabled = !this.GridContainer.IsEnabled;
        }

        private void ButtonOnCheckNextClicked(object sender, EventArgs e)
        {
            if (this.Button1.IsChecked)
                this.Button2.IsChecked = true;
            else if (this.Button2.IsChecked)
                this.Button3.IsChecked = true;
            else
                this.Button1.IsChecked = true;

            if (this.XfButton1.IsChecked)
                this.XfButton2.IsChecked = true;
            else if (this.XfButton2.IsChecked)
                this.XfButton3.IsChecked = true;
            else
                this.XfButton1.IsChecked = true;
        }

        private void ButtonOnTestDataTrigger(object sender, EventArgs e)
        {
            this.IsDataBindingActivated = !this.IsDataBindingActivated;
        }
    }
}