using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Oxard.TestApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class InteractivityView : ContentView
    {
        private bool brushChanged;

        public InteractivityView()
        {
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

        private void ButtonOnChangeBackgroundClicked(object sender, EventArgs e)
        {
            if (brushChanged)
            {
                var originalBrush = (Brush)Resources["ButtonBackgroundBrush"];
                this.Button1.Background = originalBrush;
                this.Button2.Background = originalBrush;
                this.Button3.Background = originalBrush;
                this.XfButton1.Background = originalBrush;
                this.XfButton2.Background = originalBrush;
                this.XfButton3.Background = originalBrush;
            }
            else
            {
                var brush = Brush.Blue;
                this.Button1.Background = brush;
                this.Button2.Background = brush;
                this.Button3.Background = brush;
                this.XfButton1.Background = brush;
                this.XfButton2.Background = brush;
                this.XfButton3.Background = brush;
            }

            this.brushChanged = !this.brushChanged;
        }
    }
}