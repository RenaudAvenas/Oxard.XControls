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

            var gridAlgorithm = new GridAlgorithm(); // { ColumnSpacing = 10, RowSpacing = 20 };

            gridAlgorithm.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Auto) });
            gridAlgorithm.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            gridAlgorithm.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(55, GridUnitType.Absolute) });

            gridAlgorithm.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            gridAlgorithm.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });
            gridAlgorithm.RowDefinitions.Add(new RowDefinition { Height = new GridLength(55, GridUnitType.Absolute) });

            this.algorithms.Add(gridAlgorithm);
            this.algorithms.Add(new UniformGridAlgorithm { ColumnSpacing = 15, RowSpacing = 5, Columns = 3 });

            this.AlgoLabel.Text = this.MultiFormatLayout.Algorithm.GetType().Name;
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            layoutIndex++;
            if (layoutIndex >= this.algorithms.Count)
                layoutIndex = 0;

            this.MultiFormatLayout.Algorithm = this.algorithms[layoutIndex];

            this.AlgoLabel.Text = this.MultiFormatLayout.Algorithm.GetType().Name;
        }
    }
}