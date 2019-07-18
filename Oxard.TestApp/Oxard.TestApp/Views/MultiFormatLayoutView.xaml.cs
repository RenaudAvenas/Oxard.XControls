using Oxard.XControls.Layouts.LayoutAlgorythms;
using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Oxard.TestApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MultiFormatLayoutView : ContentView
    {
        private List<LayoutAlgorythm> algorythms = new List<LayoutAlgorythm>();
        private int layoutIndex = 0;

        public MultiFormatLayoutView()
        {
            this.InitializeComponent();
            algorythms.Add(this.MultiFormatLayout.Algorythm);
            algorythms.Add(new StackAlgorythm { Spacing = 10 });
            algorythms.Add(new StackAlgorythm { Spacing = 15, Orientation = StackOrientation.Horizontal });
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            layoutIndex++;
            if (layoutIndex >= this.algorythms.Count)
                layoutIndex = 0;

            this.MultiFormatLayout.Algorythm = this.algorythms[layoutIndex];
        }
    }
}