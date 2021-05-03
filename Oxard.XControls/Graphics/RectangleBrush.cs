using Oxard.XControls.Shapes;
using Xamarin.Forms;
using Xamarin.Forms.Shapes;
using CornerRadius = Oxard.XControls.Shapes.CornerRadius;

namespace Oxard.XControls.Graphics
{
    /// <summary>
    /// Class that can be used to draw a rectangle as a background via BackgroundEffect or BackgroundProperty
    /// </summary>
    public class RectangleBrush : DrawingBrush
    {
        /// <summary>
        /// Identifies the TopLeftCornerRadius dependency property.
        /// </summary>
        public static readonly BindableProperty TopLeftCornerRadiusProperty = BindableProperty.Create(nameof(TopLeftCornerRadius), typeof(CornerRadius), typeof(RectangleBrush), propertyChanged: SpecificCornerRadiusPropertyChanged);
        /// <summary>
        /// Identifies the TopRightCornerRadius dependency property.
        /// </summary>
        public static readonly BindableProperty TopRightCornerRadiusProperty = BindableProperty.Create(nameof(TopRightCornerRadius), typeof(CornerRadius), typeof(RectangleBrush), propertyChanged: SpecificCornerRadiusPropertyChanged);
        /// <summary>
        /// Identifies the BottomRightCornerRadius dependency property.
        /// </summary>
        public static readonly BindableProperty BottomRightCornerRadiusProperty = BindableProperty.Create(nameof(BottomRightCornerRadius), typeof(CornerRadius), typeof(RectangleBrush), propertyChanged: SpecificCornerRadiusPropertyChanged);
        /// <summary>
        /// Identifies the BottomLeftCornerRadius dependency property.
        /// </summary>
        public static readonly BindableProperty BottomLeftCornerRadiusProperty = BindableProperty.Create(nameof(BottomLeftCornerRadius), typeof(CornerRadius), typeof(RectangleBrush), propertyChanged: SpecificCornerRadiusPropertyChanged);
        /// <summary>
        /// Identifies the CornerRadius dependency property.
        /// </summary>
        public static readonly BindableProperty CornerRadiusProperty = BindableProperty.Create(nameof(CornerRadius), typeof(string), typeof(RectangleBrush), propertyChanged: CornerRadiusPropertyChanged);

        private Geometry actualGeometry;
        // Used for opimization when using CornerRadiusProperty
        private bool calculationInProgress;
        private bool isLoaded;

        /// <summary>
        /// Get or set the top left corner radius
        /// </summary>
        public CornerRadius TopLeftCornerRadius
        {
            get => (CornerRadius)this.GetValue(TopLeftCornerRadiusProperty);
            set => this.SetValue(TopLeftCornerRadiusProperty, value);
        }

        /// <summary>
        /// Get or set the top right corner radius
        /// </summary>
        public CornerRadius TopRightCornerRadius
        {
            get => (CornerRadius)this.GetValue(TopRightCornerRadiusProperty);
            set => this.SetValue(TopRightCornerRadiusProperty, value);
        }

        /// <summary>
        /// Get or set the bottom right corner radius
        /// </summary>
        public CornerRadius BottomRightCornerRadius
        {
            get => (CornerRadius)this.GetValue(BottomRightCornerRadiusProperty);
            set => this.SetValue(BottomRightCornerRadiusProperty, value);
        }

        /// <summary>
        /// Get or set the bottom left corner radius
        /// </summary>
        public CornerRadius BottomLeftCornerRadius
        {
            get => (CornerRadius)this.GetValue(BottomLeftCornerRadiusProperty);
            set => this.SetValue(BottomLeftCornerRadiusProperty, value);
        }

        /// <summary>
        /// Get or set the corner radius definition for all corners by a <see cref="CornerRadiusExpression"/>.
        /// Values can be :
        /// "5" // Set all corners to x and y radius to 5
        /// "tl5" // Set tl (Top Left) corner to x and y to 5
        /// "tl5,6 br10,8" // Set tl (Top Left) corner to x 5 and y 6 and br (Bottom Right) to x 10 and y 8
        /// ...
        /// </summary>
        public string CornerRadius
        {
            get => (string)this.GetValue(CornerRadiusProperty);
            set => this.SetValue(CornerRadiusProperty, value);
        }

        /// <summary>
        /// Get the geometry of the drawable
        /// </summary>
        public override Geometry Geometry => this.actualGeometry;

        /// <summary>
        /// To be added.
        /// </summary>
        /// <value>
        /// To be added.
        /// </value>
        /// <exception cref="System.NotImplementedException"></exception>
        /// <remarks>
        /// To be added.
        /// </remarks>
        public override bool IsEmpty => false;

        /// <summary>
        /// Creates a new <see cref="DrawingBrush"/> that is a copy of the current instance.
        /// Just clone custom properties of inherited classes. The clone method of DrawingBrush already copies its own properties.
        /// </summary>
        /// <returns>A new <see cref="DrawingBrush"/> that is a copy of this instance.</returns>
        protected override DrawingBrush CloneDrawingBrush()
        {
            var rectangle = new RectangleBrush();

            if (this.CornerRadius != null)
                rectangle.CornerRadius = this.CornerRadius;
            else
            {
                if (this.TopLeftCornerRadius != null)
                    rectangle.TopLeftCornerRadius = new CornerRadius(this.TopLeftCornerRadius.RadiusX, this.TopLeftCornerRadius.RadiusY);
                if (this.TopRightCornerRadius != null)
                    rectangle.TopRightCornerRadius = new CornerRadius(this.TopRightCornerRadius.RadiusX, this.TopRightCornerRadius.RadiusY);
                if (this.BottomRightCornerRadius != null)
                    rectangle.BottomRightCornerRadius = new CornerRadius(this.BottomRightCornerRadius.RadiusX, this.BottomRightCornerRadius.RadiusY);
                if (this.BottomLeftCornerRadius != null)
                    rectangle.BottomLeftCornerRadius = new CornerRadius(this.BottomLeftCornerRadius.RadiusX, this.BottomLeftCornerRadius.RadiusY);
            }

            return rectangle;
        }

        private static void SpecificCornerRadiusPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var rect = bindable as RectangleBrush;
            rect?.SpecificCornerRadiusChanged();
        }

        private static void CornerRadiusPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var rect = bindable as RectangleBrush;
            rect?.CornerRadiusChanged();
        }

        /// <summary>
        /// Called when instance size changed (Width and Height).
        /// </summary>
        /// <param name="width">New width</param>
        /// <param name="height">New height</param>
        protected override void OnSizeAllocated(double width, double height)
        {
            this.isLoaded = true;
            this.CalculateGeometry();
        }

        /// <summary>
        /// Called when <see cref="DrawingBrush.StrokeThicknessProperty" /> changed for this instance of <see cref="DrawingBrush" />.
        /// </summary>
        protected override void OnStrokeThicknessChanged()
        {
            base.OnStrokeThicknessChanged();
            this.CalculateGeometry();

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

        private void CalculateGeometry()
        {
            if (calculationInProgress || !this.isLoaded)
                return;

            this.actualGeometry = GeometryHelper.GetRectangle(this.Width, this.Height, this.StrokeThickness, this.TopLeftCornerRadius, this.TopRightCornerRadius, this.BottomRightCornerRadius, this.BottomLeftCornerRadius);
            this.InvalidateGeometry();
        }
    }
}
