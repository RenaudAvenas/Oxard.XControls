using Android.Content;
using Android.Views;
using Oxard.XControls.Droid.Events;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Button = Oxard.XControls.Components.Button;

[assembly: ExportRenderer(typeof(Button), typeof(Oxard.XControls.Droid.Renderers.Components.ButtonRenderer))]

namespace Oxard.XControls.Droid.Renderers.Components
{
    public class ButtonRenderer : VisualElementRenderer<Button>
    {
        private TouchHelper touchHelper;

        public ButtonRenderer(Context context) : base(context)
        {
        }

        public override bool OnTouchEvent(MotionEvent e)
        {
            if (this.touchHelper != null)
                return this.touchHelper.OnTouchEvent(e);

            return base.OnTouchEvent(e);
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Button> e)
        {
            base.OnElementChanged(e);
            this.touchHelper = this.Element != null ? new TouchHelper(this.Element.TouchManager, this) : null;
        }
    }
}