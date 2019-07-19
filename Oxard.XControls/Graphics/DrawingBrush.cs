using Oxard.XControls.Components;
using Oxard.XControls.Effects;
using System;
using System.Linq;
using Xamarin.Forms;

namespace Oxard.XControls.Graphics
{
    /// <summary>
    /// Class that can be used to draw a shape as a background via BackgroundEffect or BackgroundProperty
    /// </summary>
    public abstract class DrawingBrush : Brush, IDrawable
    {
        /// <summary>
        /// Identifies the Fill dependency property.
        /// </summary>
        public static readonly BindableProperty FillProperty = BindableProperty.Create(nameof(Fill), typeof(Brush), typeof(DrawingBrush), Brushes.Transparent, propertyChanged: OnFillPropertyChanged);
        /// <summary>
        /// Identifies the Stroke dependency property.
        /// </summary>
        public static readonly BindableProperty StrokeProperty = BindableProperty.Create(nameof(Stroke), typeof(Color), typeof(DrawingBrush), Color.Transparent, propertyChanged: OnStrokePropertyChanged);
        /// <summary>
        /// Identifies the StrokeThickness dependency property.
        /// </summary>
        public static readonly BindableProperty StrokeThicknessProperty = BindableProperty.Create(nameof(StrokeThickness), typeof(double), typeof(DrawingBrush), 0d, propertyChanged: OnStrokeThicknessPropertyChanged);
        /// <summary>
        /// Identifies the StrokeDash dependency property.
        /// </summary>
        public static readonly BindableProperty StrokeDashArrayProperty = BindableProperty.Create(nameof(StrokeDashArray), typeof(Point), typeof(DrawingBrush), new Point(1, 0), propertyChanged: OnStrokeDashArrayPropertyChanged);
        /// <summary>
        /// Identifies the AttachedFill dependency property
        /// </summary>
        public static readonly BindableProperty AttachedFillProperty = BindableProperty.CreateAttached("AttachedFill", typeof(Brush), typeof(DrawingBrush), Brushes.Transparent, propertyChanged: OnAttachedFillPropertyChanged, defaultValueCreator: AttachedFillPropertyDefaultValueCreator);
        /// <summary>
        /// Identifies the  AttachedStroke dependency property
        /// </summary>
        public static readonly BindableProperty AttachedStrokeProperty = BindableProperty.CreateAttached("AttachedStroke", typeof(Color), typeof(DrawingBrush), Color.Transparent, propertyChanged: OnAttachedStrokePropertyChanged, defaultValueCreator: AttachedStrokePropertyDefaultValueCreator);
        /// <summary>
        /// Identifies the  AttachedStrokeThickness dependency property
        /// </summary>
        public static readonly BindableProperty AttachedStrokeThicknessProperty = BindableProperty.CreateAttached("AttachedStrokeThickness", typeof(double), typeof(DrawingBrush), 0d, propertyChanged: OnAttachedStrokeThicknessPropertyChanged, defaultValueCreator: AttachedStrokeThicknessPropertyDefaultValueCreator);
        /// <summary>
        /// Identifies the  AttachedStrokeDashArray dependency property
        /// </summary>
        public static readonly BindableProperty AttachedStrokeDashArrayProperty = BindableProperty.CreateAttached("AttachedStrokeDashArray", typeof(Point), typeof(DrawingBrush), new Point(1, 0), propertyChanged: OnAttachedStrokeDashArrayPropertyChanged, defaultValueCreator: AttachedStrokeDashArrayPropertyDefaultValueCreator);

        private readonly Lazy<Brush> originialFill;
        private readonly Lazy<Color> originalStroke;
        private readonly Lazy<double> originalStrokeThickness;
        private readonly Lazy<Point> originalStrokeDashArray;

        /// <summary>
        /// Default constructor
        /// </summary>
        public DrawingBrush()
        {
            this.originialFill = new Lazy<Brush>(() => this.Fill);
            this.originalStroke = new Lazy<Color>(() => this.Stroke);
            this.originalStrokeThickness = new Lazy<double>(() => this.StrokeThickness);
            this.originalStrokeDashArray = new Lazy<Point>(() => this.StrokeDashArray);
        }

        /// <summary>
        /// Event called when shape geometry changed
        /// </summary>
        public event EventHandler GeometryChanged;
        
        /// <summary>
        /// Get or set the <see cref="Brush"/> used to fill the shape
        /// </summary>
        public Brush Fill
        {
            get => (Brush)this.GetValue(FillProperty);
            set => this.SetValue(FillProperty, value);
        }

        /// <summary>
        /// Get or set the <see cref="Color"/> used to draw the outline of the shape
        /// </summary>
        public Color Stroke
        {
            get => (Color)this.GetValue(StrokeProperty);
            set => this.SetValue(StrokeProperty, value);
        }

        /// <summary>
        /// Get or set the thickness of the stroke
        /// </summary>
        public double StrokeThickness
        {
            get => (double)this.GetValue(StrokeThicknessProperty);
            set => this.SetValue(StrokeThicknessProperty, value);
        }

        /// <summary>
        /// Get or set the stroke dash
        /// </summary>
        public Point StrokeDashArray
        {
            get => (Point)this.GetValue(StrokeDashArrayProperty);
            set => this.SetValue(StrokeDashArrayProperty, value);
        }

        /// <summary>
        /// Get the geometry of the drawable
        /// </summary>
        public abstract Geometry Geometry { get; }

        /// <summary>
        /// Get the width of the drawable
        /// </summary>
        public double Width { get; private set; }

        /// <summary>
        /// Get the height of the drawable
        /// </summary>
        public double Height { get; private set; }

        /// <summary>
        /// Called to define the <see cref="Width"/> and <see cref="Height"/> of the drawable
        /// </summary>
        /// <param name="width">Width of the drawable</param>
        /// <param name="height">Height of the drawable</param>
        public void SetSize(double width, double height)
        {
            this.Width = width;
            this.Height = height;
            this.OnPropertyChanged(nameof(this.Width));
            this.OnPropertyChanged(nameof(this.Height));
            this.OnSizeAllocated(width, height);
        }

        /// <summary>
        /// Get the AttachedFill property value for the specified bindable object
        /// </summary>
        /// <param name="bindableObject">Object on which we want the value of the property</param>
        /// <returns>AttachFill property value for the bindable object</returns>
        public static Brush GetAttachedFill(BindableObject bindableObject) => (Brush)bindableObject.GetValue(AttachedFillProperty);

        /// <summary>
        /// Set the AttachedFill property value for the specified bindable object
        /// </summary>
        /// <param name="bindableObject">Object that takes the value</param>
        /// <param name="value">The value to affect</param>
        public static void SetAttachedFill(BindableObject bindableObject, Brush value) => bindableObject.SetValue(AttachedFillProperty, value);

        /// <summary>
        /// Get the AttachedStroke property value for the specified bindable object
        /// </summary>
        /// <param name="bindableObject">Object on which we want the value of the property</param>
        /// <returns>AttachedStroke property value for the bindable object</returns>
        public static Color GetAttachedStroke(BindableObject bindableObject) => (Color)bindableObject.GetValue(AttachedStrokeProperty);

        /// <summary>
        /// Set the AttachedStroke property value for the specified bindable object
        /// </summary>
        /// <param name="bindableObject">Object that takes the value</param>
        /// <param name="value">The value to affect</param>
        public static void SetAttachedStroke(BindableObject bindableObject, Color value) => bindableObject.SetValue(AttachedStrokeProperty, value);

        /// <summary>
        /// Get the AttachedStrokeDashArray property value for the specified bindable object
        /// </summary>
        /// <param name="bindableObject">Object on which we want the value of the property</param>
        /// <returns>AttachedStrokeDashArray property value for the bindable object</returns>
        public static Point GetAttachedStrokeDashArray(BindableObject bindableObject) => (Point)bindableObject.GetValue(AttachedStrokeDashArrayProperty);

        /// <summary>
        /// Set the AttachedStrokeDashArray property value for the specified bindable object
        /// </summary>
        /// <param name="bindableObject">Object that takes the value</param>
        /// <param name="value">The value to affect</param>
        public static void SetAttachedStrokeDashArray(BindableObject bindableObject, Point value) => bindableObject.SetValue(AttachedStrokeDashArrayProperty, value);

        /// <summary>
        /// Get the AttachedStrokeThickness property value for the specified bindable object
        /// </summary>
        /// <param name="bindableObject">Object on which we want the value of the property</param>
        /// <returns>AttachedStrokeThickness property value for the bindable object</returns>
        public static double GetAttachedStrokeThickness(BindableObject bindableObject) => (double)bindableObject.GetValue(AttachedStrokeThicknessProperty);

        /// <summary>
        /// Set the AttachedStrokeThickness property value for the specified bindable object
        /// </summary>
        /// <param name="bindableObject">Object that takes the value</param>
        /// <param name="value">The value to affect</param>
        public static void SetAttachedStrokeThickness(BindableObject bindableObject, double value) => bindableObject.SetValue(AttachedStrokeThicknessProperty, value);

        /// <summary>
        /// Try to found the DrawingBrush in the specified <paramref name="bindable"/> (in Background property for <see cref="ContentControl"/> otherwise in <see cref="BackgroundEffect"/>) and execute <paramref name="toDo"/> if found.
        /// </summary>
        /// <param name="bindable">Bindable object where try found drawing brush</param>
        /// <param name="toDo">Action to execute if drawing brush found</param>
        protected static void TryFoundAndDoOnDrawingBrush(BindableObject bindable, Action<DrawingBrush> toDo)
        {
            if (bindable is ContentControl contentControl && contentControl.Background is DrawingBrush drawingBrush)
                toDo(drawingBrush);
            else if (bindable is Element element)
            {
                var backgroundEffect = (BackgroundEffect)element.Effects.FirstOrDefault(e => e is BackgroundEffect);
                if (backgroundEffect?.Background is DrawingBrush effectDrawingBrush)
                    toDo(effectDrawingBrush);
            }
        }
        
        private static void OnFillPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
        }

        private static void OnStrokePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
        }

        private static void OnStrokeThicknessPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
        }

        private static void OnStrokeDashArrayPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
        }

        private static void OnAttachedFillPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            TryFoundAndDoOnDrawingBrush(bindable, d => d.Fill = (Brush)newValue);
        }

        private static void OnAttachedStrokePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            TryFoundAndDoOnDrawingBrush(bindable, d => d.Stroke = (Color)newValue);
        }

        private static void OnAttachedStrokeThicknessPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            TryFoundAndDoOnDrawingBrush(bindable, d => d.StrokeThickness = (double)newValue);
        }

        private static void OnAttachedStrokeDashArrayPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            TryFoundAndDoOnDrawingBrush(bindable, d => d.StrokeDashArray = (Point)newValue);
        }

        private static object AttachedFillPropertyDefaultValueCreator(BindableObject bindable)
        {
            Brush result = Brushes.Transparent;
            TryFoundAndDoOnDrawingBrush(bindable,  d => result = d.originialFill.Value);
            return result;
        }

        private static object AttachedStrokePropertyDefaultValueCreator(BindableObject bindable)
        {
            var result = Color.Transparent;
            TryFoundAndDoOnDrawingBrush(bindable, d => result = d.originalStroke.Value);
            return result;
        }

        private static object AttachedStrokeThicknessPropertyDefaultValueCreator(BindableObject bindable)
        {
            var result = 0d;
            TryFoundAndDoOnDrawingBrush(bindable, d => result = d.originalStrokeThickness.Value);
            return result;
        }

        private static object AttachedStrokeDashArrayPropertyDefaultValueCreator(BindableObject bindable)
        {
            var result = new Point(1, 0);
            TryFoundAndDoOnDrawingBrush(bindable, d => result = d.originalStrokeDashArray.Value);
            return result;
        }

        /// <summary>
        /// Called when instance size changed (Width and Height).
        /// </summary>
        /// <param name="width">New width</param>
        /// <param name="height">New height</param>
        protected virtual void OnSizeAllocated(double width, double height)
        {
        }

        /// <summary>
        /// Launch <see cref="GeometryChanged"/> event. 
        /// </summary>
        protected void InvalidateGeometry()
        {
            this.GeometryChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
