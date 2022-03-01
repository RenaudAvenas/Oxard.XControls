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
            }
            else if (colorIndex == 1)
            {
                colorIndex = 2;
                Resources["BackgroundLabel"] = Color.LightGray;
            }
            else
            {
                colorIndex = 0;
                Resources["BackgroundLabel"] = Color.Blue;
            }
        }
    }
}