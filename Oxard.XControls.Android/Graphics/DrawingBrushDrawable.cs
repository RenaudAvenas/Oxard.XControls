using Android.Graphics.Drawables;
using Android.Graphics;
using Oxard.XControls.Graphics;
using Xamarin.Forms;
using AView = Android.Views.View;
using AColor = Android.Graphics.Color;
using AMatrix = Android.Graphics.Matrix;
using APath = Android.Graphics.Path;
using System;
using Android.Graphics.Drawables.Shapes;
using Xamarin.Forms.Platform.Android;
using Color = Xamarin.Forms.Color;
using Android.Util;
using System.ComponentModel;

namespace Oxard.XControls.Droid.Graphics
{
    /// <summary>
    /// Drawable that draws a shape describe by a DrawingBrush
    /// </summary>
    /// <seealso cref="Android.Graphics.Drawables.ShapeDrawable" />
    public class DrawingBrushDrawable : ShapeDrawable
    {
        private readonly AView view;
        private readonly VisualElement element;
        private readonly DrawingBrush drawingBrush;
        private double lastWidth;
        private double lastHeight;

        protected float _density;

        APath _path;
        readonly RectF _pathFillBounds;
        readonly RectF _pathStrokeBounds;
        Brush _stroke;
        Brush _fill;

        Shader _strokeShader;
        Shader _fillShader;

        float _strokeWidth;
        float[] _strokeDash;
        float _strokeDashOffset;

        Stretch _aspect;

        AMatrix _transform;

        public DrawingBrushDrawable(AView view, VisualElement element, DrawingBrush drawingBrush)
        {
            if (element.Width >= 0 || element.Height >= 0)
                drawingBrush.SetSize(element.Width, element.Height);

            this.Paint.AntiAlias = true;

            using (DisplayMetrics metrics = view.Context.Resources.DisplayMetrics)
                _density = metrics.Density;

            _pathFillBounds = new RectF();
            _pathStrokeBounds = new RectF();

            _aspect = Stretch.None;
            this.view = view;
            this.element = element;
            this.drawingBrush = drawingBrush;
            this.drawingBrush.PropertyChanged += this.DrawingBrushOnPropertyChanged;
            this.drawingBrush.GeometryChanged += this.DrawingBrushOnGeometryChanged;

            this.UpdateFill(this.drawingBrush.Fill);
            this.UpdateStroke(this.drawingBrush.Stroke);
            this.UpdateStrokeThickness((float)this.drawingBrush.StrokeThickness);
            this.UpdateStrokeDashOffset((float)this.drawingBrush.StrokeDashOffset);
            this.UpdateAspect(this.drawingBrush.Aspect);
            this.UpdateStrokeDashArray(this.drawingBrush.StrokeDashArray.ToArray());
            this.UpdateStrokeLineCap(this.drawingBrush.StrokeLineCap.ToAndroid());
            this.UpdateStrokeLineJoin(this.drawingBrush.StrokeLineJoin.ToAndroid());
            this.UpdateStrokeMiterLimit((float)this.drawingBrush.StrokeMiterLimit);
        }

        protected override void OnBoundsChange(Android.Graphics.Rect bounds)
        {
            base.OnBoundsChange(bounds);

            if (this.element.Width >= 0 && this.element.Height >= 0 && (this.element.Width != this.lastWidth || this.element.Height != this.lastHeight))
            {
                this.lastWidth = this.element.Width;
                this.lastHeight = this.element.Height;
                this.drawingBrush.SetSize(this.element.Width, this.element.Height);
                this.UpdateSize(element.Width, element.Height);
                this.UpdateShape(this.drawingBrush.Geometry.ToAPath(this.view.Context));
            }
        }

        public override void Draw(Canvas canvas)
        {
            if (_path == null)
                return;

            AMatrix transformMatrix = CreateMatrix();

            _path.Transform(transformMatrix);
            transformMatrix.MapRect(_pathFillBounds);
            transformMatrix.MapRect(_pathStrokeBounds);

            if (_fill != null)
            {
                this.Paint.SetStyle(_strokeWidth > 0 ? Paint.Style.Fill : Paint.Style.FillAndStroke);

                if (_fill is GradientBrush fillGradientBrush)
                {
                    if (fillGradientBrush is LinearGradientBrush linearGradientBrush)
                        _fillShader = CreateLinearGradient(linearGradientBrush, _pathFillBounds);

                    if (fillGradientBrush is RadialGradientBrush radialGradientBrush)
                        _fillShader = CreateRadialGradient(radialGradientBrush, _pathFillBounds);

                    this.Paint.SetShader(_fillShader);
                }
                else
                {
                    AColor fillColor = Color.Default.ToAndroid();

                    if (_fill is SolidColorBrush solidColorBrush && solidColorBrush.Color != Color.Default)
                        fillColor = solidColorBrush.Color.ToAndroid();

                    this.Paint.Color = fillColor;
                }

                base.Draw(canvas);
                this.Paint.SetShader(null);
            }

            if (_stroke != null && _strokeWidth > 0)
            {
                this.Paint.SetStyle(Paint.Style.Stroke);

                if (_stroke is GradientBrush strokeGradientBrush)
                {
                    if (strokeGradientBrush is LinearGradientBrush linearGradientBrush)
                        _strokeShader = CreateLinearGradient(linearGradientBrush, _pathStrokeBounds);

                    if (strokeGradientBrush is RadialGradientBrush radialGradientBrush)
                        _strokeShader = CreateRadialGradient(radialGradientBrush, _pathStrokeBounds);

                    this.Paint.SetShader(_strokeShader);
                }
                else
                {
                    AColor strokeColor = Color.Default.ToAndroid();

                    if (_stroke is SolidColorBrush solidColorBrush && solidColorBrush.Color != Color.Default)
                        strokeColor = solidColorBrush.Color.ToAndroid();

                    this.Paint.Color = strokeColor;
                }

                base.Draw(canvas);
                this.Paint.SetShader(null);
            }

            AMatrix inverseTransformMatrix = new AMatrix();
            transformMatrix.Invert(inverseTransformMatrix);
            _path.Transform(inverseTransformMatrix);
            inverseTransformMatrix.MapRect(_pathFillBounds);
            inverseTransformMatrix.MapRect(_pathStrokeBounds);
        }

        public void UpdateShape(APath path)
        {
            _path = path;
            UpdatePathShape();
        }

        public void UpdateShapeTransform(AMatrix matrix)
        {
            _transform = matrix;
            _path.Transform(_transform);
            //Invalidate();
        }

        public void UpdateAspect(Stretch aspect)
        {
            _aspect = aspect;
            _fillShader = null;
            _strokeShader = null;
            //Invalidate();
        }

        public void UpdateFill(Brush fill)
        {
            _fill = fill;
            _fillShader = null;
            //Invalidate();
        }

        public void UpdateStroke(Brush stroke)
        {
            _stroke = stroke;
            _strokeShader = null;
            //Invalidate();
        }

        public void UpdateStrokeThickness(float strokeWidth)
        {
            _strokeWidth = _density * strokeWidth;
            this.Paint.StrokeWidth = _strokeWidth;
            UpdateStrokeDash();
        }

        public void UpdateStrokeDashArray(float[] dash)
        {
            _strokeDash = dash;
            UpdateStrokeDash();
        }

        public void UpdateStrokeDashOffset(float strokeDashOffset)
        {
            _strokeDashOffset = strokeDashOffset;
            UpdateStrokeDash();
        }

        public void UpdateStrokeLineCap(Paint.Cap strokeCap)
        {
            this.Paint.StrokeCap = strokeCap;
            UpdatePathStrokeBounds();
        }

        public void UpdateStrokeLineJoin(Paint.Join strokeJoin)
        {
            this.Paint.StrokeJoin = strokeJoin;
            //Invalidate();
        }

        public void UpdateStrokeMiterLimit(float strokeMiterLimit)
        {
            this.Paint.StrokeMiter = strokeMiterLimit * 2;
            UpdatePathStrokeBounds();
        }

        public void UpdateSize(double width, double height)
        {
            this.SetBounds(0, 0, (int)(width * _density), (int)(height * _density));
            UpdatePathShape();
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            if (disposing)
                this.drawingBrush.PropertyChanged -= this.DrawingBrushOnPropertyChanged;

        }

        protected void DrawingBrushOnPropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == DrawingBrush.AspectProperty.PropertyName)
                UpdateAspect(this.drawingBrush.Aspect);
            else if (args.PropertyName == DrawingBrush.FillProperty.PropertyName)
                UpdateFill(this.drawingBrush.Fill);
            else if (args.PropertyName == DrawingBrush.StrokeProperty.PropertyName)
                UpdateStroke(this.drawingBrush.Stroke);
            else if (args.PropertyName == DrawingBrush.StrokeThicknessProperty.PropertyName)
                UpdateStrokeThickness((float)this.drawingBrush.StrokeThickness);
            else if (args.PropertyName == DrawingBrush.StrokeDashArrayProperty.PropertyName)
                UpdateStrokeDashArray(this.drawingBrush.StrokeDashArray.ToArray());
            else if (args.PropertyName == DrawingBrush.StrokeDashOffsetProperty.PropertyName)
                UpdateStrokeDashOffset((float)this.drawingBrush.StrokeDashOffset);
            else if (args.PropertyName == DrawingBrush.StrokeLineCapProperty.PropertyName)
                UpdateStrokeLineCap(this.drawingBrush.StrokeLineCap.ToAndroid());
            else if (args.PropertyName == DrawingBrush.StrokeLineJoinProperty.PropertyName)
                UpdateStrokeLineJoin(this.drawingBrush.StrokeLineJoin.ToAndroid());
            else if (args.PropertyName == DrawingBrush.StrokeMiterLimitProperty.PropertyName)
                UpdateStrokeMiterLimit((float)this.drawingBrush.StrokeMiterLimit);
        }

        protected void UpdatePathShape()
        {
            if (_path != null && !this.Bounds.IsEmpty)
                this.Shape = new PathShape(_path, this.Bounds.Width(), this.Bounds.Height());
            else
                this.Shape = null;

            if (_path != null)
            {
                using (APath fillPath = new APath())
                {
                    this.Paint.StrokeWidth = 0.01f;
                    this.Paint.SetStyle(Paint.Style.Stroke);
                    this.Paint.GetFillPath(_path, fillPath);
                    fillPath.ComputeBounds(_pathFillBounds, false);
                    this.Paint.StrokeWidth = _strokeWidth;
                }
            }
            else
            {
                _pathFillBounds.SetEmpty();
            }

            _fillShader = null;
            UpdatePathStrokeBounds();
        }

        private void UpdateStrokeDash()
        {
            if (_strokeDash != null && _strokeDash.Length > 1)
            {
                float[] strokeDash = new float[_strokeDash.Length];

                for (int i = 0; i < _strokeDash.Length; i++)
                    strokeDash[i] = _strokeDash[i] * _strokeWidth;

                this.Paint.SetPathEffect(new DashPathEffect(strokeDash, _strokeDashOffset * _strokeWidth));
            }
            else
                this.Paint.SetPathEffect(null);

            UpdatePathStrokeBounds();
        }

        AMatrix CreateMatrix()
        {
            AMatrix matrix = new AMatrix();

            RectF drawableBounds = new RectF(this.Bounds);
            float halfStrokeWidth = this.Paint.StrokeWidth / 2;

            drawableBounds.Left += halfStrokeWidth;
            drawableBounds.Top += halfStrokeWidth;
            drawableBounds.Right -= halfStrokeWidth;
            drawableBounds.Bottom -= halfStrokeWidth;

            switch (_aspect)
            {
                case Stretch.None:
                    break;
                case Stretch.Fill:
                    matrix.SetRectToRect(_pathFillBounds, drawableBounds, AMatrix.ScaleToFit.Fill);
                    break;
                case Stretch.Uniform:
                    matrix.SetRectToRect(_pathFillBounds, drawableBounds, AMatrix.ScaleToFit.Center);
                    break;
                case Stretch.UniformToFill:
                    float widthScale = drawableBounds.Width() / _pathFillBounds.Width();
                    float heightScale = drawableBounds.Height() / _pathFillBounds.Height();
                    float maxScale = Math.Max(widthScale, heightScale);
                    matrix.SetScale(maxScale, maxScale);
                    matrix.PostTranslate(
                        drawableBounds.Left - maxScale * _pathFillBounds.Left,
                        drawableBounds.Top - maxScale * _pathFillBounds.Top);
                    break;
            }

            return matrix;
        }

        void UpdatePathStrokeBounds()
        {
            if (_path != null)
            {
                using (APath strokePath = new APath())
                {
                    this.Paint.SetStyle(Paint.Style.Stroke);
                    this.Paint.GetFillPath(_path, strokePath);
                    strokePath.ComputeBounds(_pathStrokeBounds, false);
                }
            }
            else
            {
                _pathStrokeBounds.SetEmpty();
            }

            _strokeShader = null;
            //Invalidate();
        }

        LinearGradient CreateLinearGradient(LinearGradientBrush linearGradientBrush, RectF pathBounds)
        {
            if (_path == null)
                return null;

            int[] colors = new int[linearGradientBrush.GradientStops.Count];
            float[] offsets = new float[linearGradientBrush.GradientStops.Count];

            for (int index = 0; index < linearGradientBrush.GradientStops.Count; index++)
            {
                colors[index] = linearGradientBrush.GradientStops[index].Color.ToAndroid();
                offsets[index] = linearGradientBrush.GradientStops[index].Offset;
            }

            Shader.TileMode tilemode = Shader.TileMode.Clamp;

            using (RectF gradientBounds = new RectF(pathBounds))
            {
                return new
                    LinearGradient(
                    (float)linearGradientBrush.StartPoint.X * gradientBounds.Width() + gradientBounds.Left,
                    (float)linearGradientBrush.StartPoint.Y * gradientBounds.Height() + gradientBounds.Top,
                    (float)linearGradientBrush.EndPoint.X * gradientBounds.Width() + gradientBounds.Left,
                    (float)linearGradientBrush.EndPoint.Y * gradientBounds.Height() + gradientBounds.Top,
                    colors,
                    offsets,
                    tilemode);
            }
        }

        RadialGradient CreateRadialGradient(RadialGradientBrush radialGradientBrush, RectF pathBounds)
        {
            if (_path == null)
                return null;

            int gradientStopsCount = radialGradientBrush.GradientStops.Count;
            AColor centerColor = gradientStopsCount > 0 ? radialGradientBrush.GradientStops[0].Color.ToAndroid() : Color.Default.ToAndroid();
            AColor edgeColor = gradientStopsCount > 0 ? radialGradientBrush.GradientStops[gradientStopsCount - 1].Color.ToAndroid() : Color.Default.ToAndroid();

            float[] offsets = new float[radialGradientBrush.GradientStops.Count];

            for (int index = 0; index < radialGradientBrush.GradientStops.Count; index++)
                offsets[index] = radialGradientBrush.GradientStops[index].Offset;

            Shader.TileMode tilemode = Shader.TileMode.Clamp;

            using (RectF gradientBounds = new RectF(pathBounds))
            {
                return new RadialGradient(
                    (float)radialGradientBrush.Center.X * gradientBounds.Width() + gradientBounds.Left,
                    (float)radialGradientBrush.Center.Y * gradientBounds.Height() + gradientBounds.Top,
                    (float)radialGradientBrush.Radius * Math.Max(gradientBounds.Height(), gradientBounds.Width()),
                    centerColor,
                    edgeColor,
                    tilemode);
            }
        }
        
        private void DrawingBrushOnGeometryChanged(object sender, EventArgs e)
        {
            this.UpdateShape(this.drawingBrush.Geometry.ToAPath(this.view.Context));
        }
    }
}