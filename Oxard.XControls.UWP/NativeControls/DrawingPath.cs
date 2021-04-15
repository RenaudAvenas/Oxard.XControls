using Oxard.XControls.UWP.Extensions;
using System;
using System.ComponentModel;
using Oxard.XControls.Graphics;
using Xamarin.Forms.Platform.UWP;
using Windows.Foundation;
using WDoubleCollection = Windows.UI.Xaml.Media.DoubleCollection;
using WPenLineCap = Windows.UI.Xaml.Media.PenLineCap;
using Windows.UI.Xaml.Shapes;
using Windows.UI.Xaml;

namespace Oxard.XControls.UWP.NativeControls
{
    public class DrawingPath : Path
    {
        public static readonly DependencyProperty DrawableProperty = DependencyProperty.Register(nameof(Drawable), typeof(DrawingBrush), typeof(DrawingPath), new PropertyMetadata(null, OnDrawablePropertyChanged));

        public DrawingPath()
        {
        }

        public DrawingBrush Drawable
        {
            get { return (DrawingBrush)GetValue(DrawableProperty); }
            set { SetValue(DrawableProperty, value); }
        }

        private static void OnDrawablePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as DrawingPath)?.OnDrawableChanged(e.OldValue as DrawingBrush);
        }

        private void OnDrawableChanged(DrawingBrush oldDrawable)
        {
            if (oldDrawable != null)
            {
                oldDrawable.GeometryChanged -= this.DrawableOnGeometryChanged;
                oldDrawable.PropertyChanged -= DrawablePropertyChanged;
            }

            if (this.Drawable == null)
                return;

            this.UpdatePath();
            this.UpdateAspect();
            this.UpdateFill();
            this.UpdateStroke();
            this.UpdateStrokeThickness();
            this.UpdateStrokeDashArray();
            this.UpdateStrokeDashOffset();
            this.UpdateStrokeLineCap();
            this.UpdateStrokeLineJoin();
            this.UpdateStrokeMiterLimit();

            this.Drawable.GeometryChanged += this.DrawableOnGeometryChanged;
            this.Drawable.PropertyChanged += this.DrawablePropertyChanged;
        }

        private void DrawablePropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(DrawingBrush.Stroke))
                this.UpdateStroke();
            else if (e.PropertyName == nameof(DrawingBrush.Fill))
                this.UpdateFill();
            else if (e.PropertyName == nameof(DrawingBrush.StrokeThickness))
                this.UpdateStrokeThickness();
            else if (e.PropertyName == nameof(DrawingBrush.StrokeDashArray))
                this.UpdateStrokeDashArray();
            else if (e.PropertyName == nameof(DrawingBrush.Aspect))
                this.UpdateAspect();
            else if (e.PropertyName == nameof(DrawingBrush.StrokeDashOffset))
                this.UpdateStrokeDashOffset();
            else if (e.PropertyName == nameof(DrawingBrush.StrokeLineCap))
                this.UpdateStrokeLineCap();
            else if (e.PropertyName == nameof(DrawingBrush.StrokeLineJoin))
                this.UpdateStrokeLineJoin();
            else if (e.PropertyName == nameof(DrawingBrush.StrokeMiterLimit))
                this.UpdateStrokeMiterLimit();
        }

        private void DrawableOnGeometryChanged(object sender, EventArgs e)
        {
            this.UpdatePath();
        }

        private void UpdatePath()
        {
            if (this.Drawable?.Geometry == null)
                return;

            this.Data = this.Drawable.Geometry.ToWindows();
        }

		private void UpdateAspect()
		{
			this.Stretch = this.Drawable.Aspect.ToWindows();

            if (this.Stretch == Windows.UI.Xaml.Media.Stretch.Uniform)
            {
                this.HorizontalAlignment = HorizontalAlignment.Center;
                this.VerticalAlignment = VerticalAlignment.Center;
            }
            else
            {
                this.HorizontalAlignment = HorizontalAlignment.Left;
                this.VerticalAlignment = VerticalAlignment.Top;
            }
        }

		private void UpdateFill()
		{
			this.Fill = this.Drawable.Fill.ToBrush();
		}

		private void UpdateStroke()
		{
			this.Stroke = this.Drawable.Stroke.ToBrush();
		}

		private void UpdateStrokeThickness()
		{
			this.StrokeThickness = this.Drawable.StrokeThickness;
		}

		private void UpdateStrokeDashArray()
		{
			if (this.StrokeDashArray != null)
				this.StrokeDashArray.Clear();

			if (this.Drawable.StrokeDashArray != null && this.Drawable.StrokeDashArray.Count > 0)
			{
				if (this.StrokeDashArray == null)
					this.StrokeDashArray = new WDoubleCollection();

				double[] array = new double[this.Drawable.StrokeDashArray.Count];
				this.Drawable.StrokeDashArray.CopyTo(array, 0);

				foreach (double value in array)
				{
					this.StrokeDashArray.Add(value);
				}
			}
		}

		private void UpdateStrokeDashOffset()
		{
			this.StrokeDashOffset = this.Drawable.StrokeDashOffset;
		}

		private void UpdateStrokeLineCap()
		{
			WPenLineCap wLineCap = this.Drawable.StrokeLineCap.ToWindows();

			this.StrokeStartLineCap = wLineCap;
			this.StrokeEndLineCap = wLineCap;
		}

		private void UpdateStrokeLineJoin()
		{
			this.StrokeLineJoin = this.Drawable.StrokeLineJoin.ToWindows();
		}

		private void UpdateStrokeMiterLimit()
		{
			this.StrokeMiterLimit = this.Drawable.StrokeMiterLimit;
		}
	}
}
