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
        static ShapeView()
        {
            InterpretorManager.RegisterForTypeIfNotExists(typeof(LineSegment), new LineSegmentInterpretor());
            InterpretorManager.RegisterForTypeIfNotExists(typeof(CornerSegment), new CornerSegmentInterpretor());
        }

        public ShapeView(Context context)
            : base(context)
        {
        }

        internal Shapes.Shape Source { get; set; }

        public override void Draw(Canvas canvas)
        {
            if (this.Source == null)
                return;

            this.Source.ToDrawable(this.Context).Draw(canvas);
        }
    }
}