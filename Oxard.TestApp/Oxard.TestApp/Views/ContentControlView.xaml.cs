using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Oxard.TestApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ContentControlView : ContentView
    {
        public ContentControlView()
        {
            this.InitializeComponent();
            this.BindingContext = this;
        }

        public bool Test { get; set; }
    }
}