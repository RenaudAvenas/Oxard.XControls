using Android.Content;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Graphics.Drawables.Shapes;
using Android.Views;
using Oxard.XControls.Droid.Extensions;
using Oxard.XControls.Droid.Interpretors;
using Oxard.XControls.Extensions;
using Oxard.XControls.Graphics;
using Oxard.XControls.Interpretors;
using System;
using Xamarin.Forms.Platform.Android;

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
            int strokeThickness = (int)Math.Ceiling(this.Context.ToPixels(this.Source.StrokeThickness));

            var width = this.Context.ToPixels(this.Source.Width.TranslateIfNegative());
            var height = this.Context.ToPixels(this.Source.Height.TranslateIfNegative());

            var path = this.GetPath(width, height);

            if (this.Source.Fill != null && !this.Source.Fill.Equals(Brushes.Transparent))
            {
                ShapeDrawable drawable = new ShapeDrawable(new PathShape(path, width, height));
                this.Source.Fill.ToBackground(drawable.Paint, width, height);
                drawable.Paint.SetStyle(Paint.Style.Fill);
                drawable.Paint.Flags = PaintFlags.AntiAlias;

                drawable.SetBounds(0, 0, (int)width, (int)height);
                drawable.Draw(canvas);
            }

            if (strokeThickness > 0)
            {
                ShapeDrawable strokeDrawable = new ShapeDrawable(new PathShape(path, width, height));
                strokeDrawable.Paint.Flags = PaintFlags.AntiAlias;
                strokeDrawable.Paint.Color = this.Source.Stroke.ToAndroid();
                strokeDrawable.Paint.SetStyle(Paint.Style.Stroke);
                strokeDrawable.Paint.StrokeWidth = strokeThickness;
                if (!this.Source.StrokeDashArray.Y.DoubleIsEquals(0d))
                {
                    var x = (float)Math.Ceiling(this.Context.ToPixels(this.Source.StrokeDashArray.X));
                    var y = (float)Math.Ceiling(this.Context.ToPixels(this.Source.StrokeDashArray.Y));
                    strokeDrawable.Paint.SetPathEffect(new DashPathEffect(new[] { x, y }, 0f));
                }

                strokeDrawable.SetBounds(0, 0, (int)width, (int)height);
                strokeDrawable.Draw(canvas);
            }
        }

        private Path GetPath(float width, float height)
        {
            var path = new Path();

            var reader = this.Source.Geometry.GetReader();
            var segment = reader.GetNext();
            path.MoveTo(this.Context.ToPixels(reader.FromPoint.X), this.Context.ToPixels(reader.FromPoint.Y));
            while (segment != null)
            {
                var interpretor = InterpretorManager.GetForType(segment.GetType());

                if (interpretor == null)
                    throw new InvalidOperationException($"Interpretor for type {segment.GetType()} was not found. Use InterpretorManager.RegisterForType method to register your interpretor.");

                if (!(interpretor is ISegmentInterpretor segmentInterpretor))
                    throw new InvalidOperationException($"Interpretor for type {segment.GetType()} was found but it is not an ISegmentInterpretor.");

                segmentInterpretor.AddToPath(segment, reader.FromPoint, path, this.Context);

                segment = reader.GetNext();
            }

            return path;
        }
    }
}