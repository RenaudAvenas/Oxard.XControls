using Oxard.XControls.Components;
using Oxard.XControls.UWP.Events;
using Xamarin.Forms.Platform.UWP;

[assembly: ExportRenderer(typeof(Button), typeof(Oxard.XControls.UWP.Renderers.Components.ButtonRenderer))]
[assembly: ExportRenderer(typeof(CheckBox), typeof(Oxard.XControls.UWP.Renderers.Components.ButtonRenderer))]

namespace Oxard.XControls.UWP.Renderers.Components
{
    public class ButtonRenderer : ContentControlRenderer<Button>//VisualElementRenderer<Button, Windows.UI.Xaml.Controls.ContentControl>
    {
        private TouchHelper touchHelper;

        protected override void OnElementChanged(ElementChangedEventArgs<Button> e)
        {
            if (this.Control != null)
                this.touchHelper?.Dispose();

            base.OnElementChanged(e);

            if (e.NewElement != null)
                this.touchHelper = new TouchHelper(this.Element.TouchManager, this);
        }
    }
}
