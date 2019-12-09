using System;
using System.Collections.ObjectModel;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Oxard.TestApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ItemsControlView : ContentView
    {
        private Random random = new Random();

        public ItemsControlView()
        {
            this.BindingContext = this;
            this.Source = new ObservableCollection<string> { "Test1", "Test2" };
            this.InitializeComponent();

        }

        public ObservableCollection<string> Source { get; }

        private void AddButtonClicked(object sender, EventArgs e)
        {
            this.Source.Add(this.GetRandomString());
            var result = this.ItemsControl.GetViewForDataItem<View>("Test2");
        }

        private void InsertButtonClicked(object sender, EventArgs e)
        {
            if (this.Source.Count == 0)
                return;

            this.Source.Insert(this.random.Next(0, this.Source.Count - 1), this.GetRandomString());
        }

        private void RemoveButtonClicked(object sender, EventArgs e)
        {
            if (this.Source.Count == 0)
                return;

            this.Source.RemoveAt(this.random.Next(0, this.Source.Count - 1));
        }

        private void RemoveEndButtonClicked(object sender, EventArgs e)
        {
            if (this.Source.Count == 0)
                return;

            this.Source.RemoveAt(this.Source.Count - 1);
        }

        private string GetRandomString()
        {
            int length = this.random.Next(1, 11);
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < length; i++)
            {
                int range = random.Next(0, 3);

                // Range of number
                if (range == 0)
                    builder.Append((char)random.Next(48, 58));

                // Range of upper letter
                else if (range == 1)
                    builder.Append((char)random.Next(65, 91));

                // Range of lower letter
                else
                    builder.Append((char)random.Next(97, 123));
            }

            return builder.ToString();
        }
    }
}