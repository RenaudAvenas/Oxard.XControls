﻿using Oxard.XControls.Graphics;
using Xamarin.Forms;

namespace Oxard.XControls.Shapes
{
    public class Rectangle : Shape
    {
        public static readonly BindableProperty TopLeftCornerRadiusProperty = BindableProperty.Create(nameof(TopLeftCornerRadius), typeof(CornerRadius), typeof(Rectangle), propertyChanged: SpecificCornerRadiusPropertyChanged);
        public static readonly BindableProperty TopRightCornerRadiusProperty = BindableProperty.Create(nameof(TopRightCornerRadius), typeof(CornerRadius), typeof(Rectangle), propertyChanged: SpecificCornerRadiusPropertyChanged);
        public static readonly BindableProperty BottomRightCornerRadiusProperty = BindableProperty.Create(nameof(BottomRightCornerRadius), typeof(CornerRadius), typeof(Rectangle), propertyChanged: SpecificCornerRadiusPropertyChanged);
        public static readonly BindableProperty BottomLeftCornerRadiusProperty = BindableProperty.Create(nameof(BottomLeftCornerRadius), typeof(CornerRadius), typeof(Rectangle), propertyChanged: SpecificCornerRadiusPropertyChanged);
        public static readonly BindableProperty CornerRadiusProperty = BindableProperty.Create(nameof(CornerRadius), typeof(string), typeof(Rectangle), propertyChanged: CornerRadiusPropertyChanged);

        private Geometry actualGeometry;
        // Used for opimization when using CornerRadiusProperty
        private bool calculationInProgress;
        private bool isLoaded;

        public CornerRadius TopLeftCornerRadius
        {
            get => (CornerRadius)this.GetValue(TopLeftCornerRadiusProperty);
            set => this.SetValue(TopLeftCornerRadiusProperty, value);
        }

        public CornerRadius TopRightCornerRadius
        {
            get => (CornerRadius)this.GetValue(TopRightCornerRadiusProperty);
            set => this.SetValue(TopRightCornerRadiusProperty, value);
        }

        public CornerRadius BottomRightCornerRadius
        {
            get => (CornerRadius)this.GetValue(BottomRightCornerRadiusProperty);
            set => this.SetValue(BottomRightCornerRadiusProperty, value);
        }

        public CornerRadius BottomLeftCornerRadius
        {
            get => (CornerRadius)this.GetValue(BottomLeftCornerRadiusProperty);
            set => this.SetValue(BottomLeftCornerRadiusProperty, value);
        }

        public string CornerRadius
        {
            get => (string)this.GetValue(CornerRadiusProperty);
            set => this.SetValue(CornerRadiusProperty, value);
        }

        public override Geometry Geometry => this.actualGeometry;

        protected override void OnSizeAllocated(double width, double height)
        {
            this.isLoaded = true;
            this.CalculateGeometry();
        }

        private void CalculateGeometry()
        {
            if (calculationInProgress || !this.isLoaded)
                return;

            this.actualGeometry = GeometryHelper.GetRectangle(this.Width, this.Height, this.StrokeThickness, this.TopLeftCornerRadius, this.TopRightCornerRadius, this.BottomRightCornerRadius, this.BottomLeftCornerRadius); ;
            this.InvalidateGeometry();
        }

        private static void SpecificCornerRadiusPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            Rectangle rect = bindable as Rectangle;
            rect?.SpecificCornerRadiusChanged();
        }

        private static void CornerRadiusPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            Rectangle rect = bindable as Rectangle;
            rect?.CornerRadiusChanged();
        }

        private void CornerRadiusChanged()
        {
            var expression = new CornerRadiusExpression(this.CornerRadius);
            calculationInProgress = true;

            this.TopLeftCornerRadius = expression.TopLeft;
            this.TopRightCornerRadius = expression.TopRight;
            this.BottomLeftCornerRadius = expression.BottomLeft;
            this.BottomRightCornerRadius = expression.BottomRight;

            calculationInProgress = false;

            this.CalculateGeometry();
        }

        private void SpecificCornerRadiusChanged()
        {
            this.CalculateGeometry();
        }
    }
}
