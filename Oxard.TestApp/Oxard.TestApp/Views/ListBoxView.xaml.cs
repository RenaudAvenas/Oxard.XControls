using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Oxard.TestApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ListBoxView : ContentView
    {
        public List<ListItem> Items { get; } = new List<ListItem>();

        public ListBoxView()
        {
            Items.Add(new ListItem("List item 1"));
            Items.Add(new ListItem("List item 2"));
            Items.Add(new ListItem("List item 3"));
            Items.Add(new ListItem("List item 4"));
            Items.Add(new ListItem("List item 5"));
            Items.Add(new ListItem("List item 6"));

            InitializeComponent();

            this.BindingContext = this;
        }

        private void SelectByIndexClicked(object sender, System.EventArgs e)
        {
            ListBox.SelectedIndex = 3;
        }

        private void SelectByItemClicked(object sender, System.EventArgs e)
        {
            ListBox.SelectedItem = Items[4];
        }
    }

    public class ListItem
    {
        public string Text { get; }

        public ListItem(string text)
        {
            this.Text = text;
        }
    }
}