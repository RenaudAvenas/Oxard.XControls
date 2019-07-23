using Oxard.XControls.Layouts.LayoutAlgorithms;
using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Oxard.TestApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MultiFormatLayoutView : ContentView
    {
        private List<LayoutAlgorithm> algorithms = new List<LayoutAlgorithm>();
        private int layoutIndex = 0;

        public MultiFormatLayoutView()
        {
            this.InitializeComponent();
            this.algorithms.Add(this.MultiFormatLayout.Algorithm);
            this.algorithms.Add(new StackAlgorithm { Spacing = 10 });
            this.algorithms.Add(new StackAlgorithm { Spacing = 15, Orientation = StackOrientation.Horizontal });
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            layoutIndex++;
            if (layoutIndex >= this.algorithms.Count)
                layoutIndex = 0;

            this.MultiFormatLayout.Algorithm = this.algorithms[layoutIndex];
        }
    }
}