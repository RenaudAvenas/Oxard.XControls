using Android.Content;
using Android.Graphics;
using Android.Views;
using Oxard.XControls.Droid.Extensions;
using Oxard.XControls.Droid.Interpretors;
using Oxard.XControls.Graphics;
using Oxard.XControls.Interpretors;

namespace Oxard.XControls.Droid.NativeControls
{
    public class ShapeView : View
    {
        private static bool defaultInterpretorsAreRegistred;

        public ShapeView(Context context)
            : base(context)
        {
            if (!defaultInterpretorsAreRegistred)
            {
                defaultInterpretorsAreRegistred = true;
                InterpretorManager.RegisterForType(typeof(LineSegment), new LineSegmentInterpretor());
                InterpretorManager.RegisterForType(typeof(CornerSegment), new CornerSegmentInterpretor());
            }
        }

        internal Shapes.Shape Source { get; set; }

        public override void Draw(Canvas canvas)
        {
            this.Source.ToDrawable(this.Context).Draw(canvas);
        }
    }
}