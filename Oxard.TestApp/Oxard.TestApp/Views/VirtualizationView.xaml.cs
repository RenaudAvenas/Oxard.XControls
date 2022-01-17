using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Oxard.TestApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class VirtualizationView : ContentView
    {
        private readonly Random random = new Random();
        private List<ComplexeElement> source;
        private ScrollToPosition nextScrollToPosition = ScrollToPosition.Start;

        public VirtualizationView()
        {
            InitializeComponent();
        }

        private ComplexeElement GenerateRandomComplexeElement()
        {
            return new ComplexeElement
            {
                Text = GenerateString(),
                GlyphText = GenerateString(1),
                BackgroundColor = GenerateColor(),
                ForegroundColor = GenerateColor(),
            };
        }

        private Color GenerateColor()
        {
            return new Color(GenerateDouble(0, 1), GenerateDouble(0, 1), GenerateDouble(0, 1));
        }

        private string GenerateString(int? length = null)
        {
            var stringLength = length ?? random.Next(1, 51);
            StringBuilder builder = new StringBuilder();

            for (int i = 0; i < stringLength; i++)
                builder.Append((char)random.Next(97, 123));

            return builder.ToString();
        }

        private double GenerateDouble(double min, double max)
        {
            return random.Next((int)(min * 100), (int)(max * 100 + 1)) / 100d;
        }

        private async void OnGenerateDataButton_Clicked(object sender, EventArgs e)
        {
            if (ActivityIndicator.IsRunning)
                return;

            ActivityIndicator.IsRunning = true;
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            source = new List<ComplexeElement>();
            await Task.Run(() =>
            {
                for (int i = 0; i < 1000; i++)
                {
                    source.Add(GenerateRandomComplexeElement());
                }

                source[500] = new ComplexeElement { BackgroundColor = Color.Red, ForegroundColor = Color.White, GlyphText = "S", Text = "Scroll to element" };
            });

            stopwatch.Stop();

            SummaryLabel.Text = $"1000 elements generated in {stopwatch.Elapsed}";

            ActivityIndicator.IsRunning = false;
        }

        private void NotVirualizedButton_Clicked(object sender, EventArgs e)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            NotVirtualizedItemsControl.ItemsSource = source;

            stopwatch.Stop();

            SummaryLabel.Text = $"1000 elements displayed in {stopwatch.Elapsed} (not virtualized)";
        }

        private void VirualizedButton_Clicked(object sender, EventArgs e)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            VirtualizedItemsControl.ItemsSource = source;

            stopwatch.Stop();

            SummaryLabel.Text = $"1000 elements displayed in {stopwatch.Elapsed} (virtualized)";
        }

        private void ResetButton_Clicked(object sender, EventArgs e)
        {
            SummaryLabel.Text = string.Empty;
            VirtualizedItemsControl.ItemsSource = null;
            NotVirtualizedItemsControl.ItemsSource = null;
        }

        private void ScrollTo_Clicked(object sender, EventArgs e)
        {
            VirtualizedItemsControl.ScrollToAsync(source[500], this.nextScrollToPosition, true);

            var previousScrollToPosition = this.nextScrollToPosition;
            switch (this.nextScrollToPosition)
            {
                case ScrollToPosition.MakeVisible:
                    this.nextScrollToPosition = ScrollToPosition.Start;
                    break;
                case ScrollToPosition.Start:
                    this.nextScrollToPosition = ScrollToPosition.Center;
                    break;
                case ScrollToPosition.Center:
                    this.nextScrollToPosition = ScrollToPosition.End;
                    break;
                case ScrollToPosition.End:
                    this.nextScrollToPosition = ScrollToPosition.MakeVisible;
                    break;
                default:
                    break;
            }

            SummaryLabel.Text = $"Scroll to element at index 500 with scroll position : {previousScrollToPosition} (new scroll position wil be {this.nextScrollToPosition})";
        }
    }

    public class ComplexeElement
    {
        public string Text { get; set; }

        public Color BackgroundColor { get; set; }

        public Color ForegroundColor { get; set; }

        public string GlyphText { get; set; }
    }
}