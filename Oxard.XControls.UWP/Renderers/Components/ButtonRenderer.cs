using Oxard.XControls.Components;
using Oxard.XControls.UWP.Events;
using System.ComponentModel;
using Xamarin.Forms.Platform.UWP;

[assembly: ExportRenderer(typeof(Button), typeof(Oxard.XControls.UWP.Renderers.Components.ButtonRenderer))]

namespace Oxard.XControls.UWP.Renderers.Components
{
    public class ButtonRenderer : VisualElementRenderer<Button, Windows.UI.Xaml.Controls.ContentControl>
    {
        private TouchHelper touchHelper;

        protected override void OnElementChanged(ElementChangedEventArgs<Button> e)
        {
            if (this.Control != null)
            {
                this.touchHelper?.Dispose();
                this.SetNativeControl(null);
            }

            base.OnElementChanged(e);
            
            if (e.NewElement != null)
            {
                if (this.Control == null)
                {
                    this.SetNativeControl(new Windows.UI.Xaml.Controls.ContentControl());
                    this.touchHelper = new TouchHelper(this.Element.TouchManager, this);
                }
            }
        }
    }
}
