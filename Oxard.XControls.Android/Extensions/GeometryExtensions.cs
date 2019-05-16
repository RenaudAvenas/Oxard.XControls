using Android.Content;
using Android.Graphics;
using Android.Graphics.Drawables;
using Oxard.XControls.Graphics;
using Oxard.XControls.Extensions;
using System;
using Xamarin.Forms.Platform.Android;
using Android.Graphics.Drawables.Shapes;
using System.Collections.Generic;
using Oxard.XControls.Droid.Extensions;
using Oxard.XControls.Interpretors;
using Oxard.XControls.Droid.Interpretors;

namespace Oxard.XControls.Droid.Extensions
{
    public static class GeometryExtensions
    {
        public static Drawable ToDrawable(this IDrawable drawable, Context androidContext)
        {
            var drawables = new List<Drawable>(2);

            int strokeThickness = (int)Math.Ceiling(androidContext.ToPixels(drawable.StrokeThickness));

            var width = androidContext.ToPixels(drawable.Width.TranslateIfNegative());
            var height = androidContext.ToPixels(drawable.Height.TranslateIfNegative());

            var path = GetPath(drawable, width, height, androidContext);

            if (drawable.Fill != null && !drawable.Fill.Equals(Brushes.Transparent))
            {
                ShapeDrawable fillDrawable = new ShapeDrawable(new PathShape(path, width, height));
                drawable.Fill.ToBackground(fillDrawable.Paint, width, height);
                fillDrawable.Paint.SetStyle(Paint.Style.Fill);
                fillDrawable.Paint.Flags = PaintFlags.AntiAlias;

                fillDrawable.SetBounds(0, 0, (int)width, (int)height);
                drawables.Add(fillDrawable);
            }

            if (strokeThickness > 0)
            {
                ShapeDrawable strokeDrawable = new ShapeDrawable(new PathShape(path, width, height));
                strokeDrawable.Paint.Flags = PaintFlags.AntiAlias;
                strokeDrawable.Paint.Color = drawable.Stroke.ToAndroid();
                strokeDrawable.Paint.SetStyle(Paint.Style.Stroke);
                strokeDrawable.Paint.StrokeWidth = strokeThickness;
                if (!drawable.StrokeDashArray.Y.DoubleIsEquals(0d))
                {
                    var x = (float)Math.Ceiling(androidContext.ToPixels(drawable.StrokeDashArray.X));
                    var y = (float)Math.Ceiling(androidContext.ToPixels(drawable.StrokeDashArray.Y));
                    strokeDrawable.Paint.SetPathEffect(new DashPathEffect(new[] { x, y }, 0f));
                }

                strokeDrawable.SetBounds(0, 0, (int)width, (int)height);
                drawables.Add(strokeDrawable);
            }

            return new LayerDrawable(drawables.ToArray());
        }

        private static Path GetPath(IDrawable drawable, float width, float height, Context androidContext)
        {
            var path = new Path();

            var reader = drawable.Geometry.GetReader();
            var segment = reader.GetNext();
            path.MoveTo(androidContext.ToPixels(reader.FromPoint.X), androidContext.ToPixels(reader.FromPoint.Y));
            while (segment != null)
            {
                var interpretor = InterpretorManager.GetForType(segment.GetType());

                if (interpretor == null)
                    throw new InvalidOperationException($"Interpretor for type {segment.GetType()} was not found. Use InterpretorManager.RegisterForType method to register your interpretor.");

                if (!(interpretor is ISegmentInterpretor segmentInterpretor))
                    throw new InvalidOperationException($"Interpretor for type {segment.GetType()} was found but it is not an ISegmentInterpretor.");

                segmentInterpretor.AddToPath(segment, reader.FromPoint, path, androidContext);

                segment = reader.GetNext();
            }

            return path;
        }
    }
}