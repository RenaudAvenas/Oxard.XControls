using System.Collections.ObjectModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Oxard.TestApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ItemsControlView : ContentView
    {
        public ItemsControlView()
        {
            this.BindingContext = this;
            this.Source = new ObservableCollection<string> { "Test1", "Test2" };
            this.InitializeComponent();
        }

        public ObservableCollection<string> Source { get; }
    }
}