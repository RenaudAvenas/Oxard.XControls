using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Oxard.TestApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DynamicResourceView : ContentView
    {
        private int colorIndex;

        public DynamicResourceView()
        {
            InitializeComponent();
        }

        private void ChangeColorButton_Clicked(object sender, EventArgs e)
        {
            if (this.colorIndex == 0)
            {
                colorIndex = 1;
                Resources["BackgroundLabel"] = Color.Aqua;
                Resources["BackgroundPressed"] = Color.FromHex("99FFFF");
                Resources["Foreground"] = Color.Black;
            }
            else if (colorIndex == 1)
            {
                colorIndex = 2;
                Resources["BackgroundLabel"] = Color.LightGray;
                Resources["BackgroundPressed"] = Color.DarkGray;
                Resources["Foreground"] = Color.FromHex("33FFFF");
                Resources["Foreground"] = Color.Yellow;
            }
            else
            {
                colorIndex = 0;
                Resources["BackgroundLabel"] = Color.Blue;
                Resources["BackgroundPressed"] = Color.DarkBlue;
                Resources["Foreground"] = Color.White;
            }
        }
    }
}