namespace Oxard.Maui.TestApp.Views;

public partial class ContentControlView : ContentView
{
    private bool? test;

    public ContentControlView()
	{
		InitializeComponent();
        this.BindingContext = this;
    }

    public bool? Test
    {
        get => test;
        set
        {
            test = value;
            this.OnPropertyChanged();
        }
    }

    private void Button_Clicked(object sender, System.EventArgs e)
    {
        if (!this.Test.HasValue)
            this.Test = false;
        else
        {
            if (this.Test.Value)
                this.Test = null;
            else
                this.Test = true;
        }
    }
}