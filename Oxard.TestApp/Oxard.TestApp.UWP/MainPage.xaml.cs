using Oxard.XControls.UWP.Events;

namespace Oxard.TestApp.UWP
{
    public sealed partial class MainPage
    {
        public MainPage()
        {
            this.InitializeComponent();

            KeyboardHelper.Initialize();

            LoadApplication(new Oxard.TestApp.App());
        }
    }
}