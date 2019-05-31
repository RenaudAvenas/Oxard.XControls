using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Oxard.TestApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ButtonView : ContentView
    {
        public ButtonView()
        {
            InitializeComponent();
        }

        //private void LongPressButton_Clicked(object sender, EventArgs e)
        //{
        //    this.ClickOrLongPressedInfoLabel.Text = "Clicked!";
        //    this.AnimateLongPressedInfoLabel();
        //}

        //private void LongPressButton_LongPressed(object sender, EventArgs e)
        //{

        //    this.ClickOrLongPressedInfoLabel.Text = "LongPressed!";
        //    this.AnimateLongPressedInfoLabel();
        //}

        //private void AnimateLongPressedInfoLabel()
        //{
        //    ViewExtensions.CancelAnimations(this.ClickOrLongPressedInfoLabel);
        //    this.ClickOrLongPressedInfoLabel.Opacity = 1;
        //    this.ClickOrLongPressedInfoLabel.FadeTo(0);
        //}
    }
}