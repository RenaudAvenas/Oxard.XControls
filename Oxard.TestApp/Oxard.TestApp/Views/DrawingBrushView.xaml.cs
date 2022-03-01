using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Oxard.TestApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DrawingBrushView : ContentView
    {
        private int colorIndex = 0;

        public DrawingBrushView()
        {
            InitializeComponent();
        }

        private void ChangeColorButtonClicked(object sender, System.EventArgs e)
        {
            if (colorIndex == 0)
            {
                colorIndex = 1;
                Resources["EllipseBackground"] = Resources["RedColor"];
            }
            else if (colorIndex == 1)
            {
                colorIndex = 2;
                Resources["EllipseBackground"] = Resources["BlueColor"];
            }
            else
            {
                colorIndex = 0;
                Resources["EllipseBackground"] = Resources["AquaColor"];
            }
        }
    }
}