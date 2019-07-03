using System;
using System.Collections.Generic;
using System.Reflection;
using Xamarin.Forms;

namespace Oxard.TestApp
{
    public partial class MainPage
    {
        public MainPage()
        {
            this.BindingContext = this;

            var assembly = Assembly.GetExecutingAssembly();

            foreach (var type in assembly.GetTypes())
            {
                if (type.FullName.Contains(".Views.") && type.FullName.EndsWith("View"))
                    this.PageModels.Add(new PageModel(type));
            }

            InitializeComponent();
        }

        public List<PageModel> PageModels { get; } = new List<PageModel>();

        private void ButtonOnClicked(object sender, EventArgs e)
        {
            Application.Current.MainPage = new NavigationPage(new MainPage());
        }

        private void ListViewItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var pageModel = (PageModel)e.SelectedItem;
            this.CurrentView.Content = pageModel.CreateInstance();
        }

        public class PageModel
        {
            public PageModel(Type pageType)
            {
                this.PageType = pageType;
                this.Name = pageType.Name;
            }

            public Type PageType { get; }

            public string Name { get; }

            internal View CreateInstance()
            {
                return (View)Activator.CreateInstance(this.PageType);
            }
        }
    }
}
