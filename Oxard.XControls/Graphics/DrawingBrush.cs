using System;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Shapes;

namespace Oxard.XControls.Graphics
{
    /// <summary>
    /// Class that can be used to draw a shape as a background via BackgroundEffect or BackgroundProperty
    /// </summary>
    public abstract class DrawingBrush : Brush
    {
        /// <summary>
        /// Identifies the Fill dependency property.
        /// </summary>
        public static readonly BindableProperty FillProperty = BindableProperty.Create(nameof(Fill), typeof(Brush), typeof(DrawingBrush), Brush.Transparent, propertyChanged: OnFillPropertyChanged);

        /// <summary>
        /// Identifies the Stroke dependency property.
        /// </summary>
        public static readonly BindableProperty StrokeProperty = BindableProperty.Create(nameof(Stroke), typeof(Brush), typeof(DrawingBrush), Brush.Transparent, propertyChanged: OnStrokePropertyChanged);

        /// <summary>
        /// Identifies the StrokeThickness dependency property.
        /// </summary>
        public static readonly BindableProperty StrokeThicknessProperty = BindableProperty.Create(nameof(StrokeThickness), typeof(double), typeof(DrawingBrush), 0d, propertyChanged: OnStrokeThicknessPropertyChanged);

        /// <summary>
        /// Identifies the StrokeDash dependency property.
        /// </summary>
        public static readonly BindableProperty StrokeDashArrayProperty = BindableProperty.Create(nameof(StrokeDashArray), typeof(DoubleCollection), typeof(DrawingBrush), new DoubleCollection(), propertyChanged: OnStrokeDashArrayPropertyChanged);

        /// <summary>
        /// Identifies the Fill dependency property.
        /// </summary>
        public static readonly BindableProperty StrokeDashOffsetProperty = BindableProperty.Create(nameof(StrokeDashOffset), typeof(double), typeof(DrawingBrush), 0d, propertyChanged: OnStrokeDashOffsetPropertyChanged);

        /// <summary>
        /// Identifies the Fill dependency property.
        /// </summary>
        public static readonly BindableProperty StrokeLineCapProperty = BindableProperty.Create(nameof(StrokeLineCap), typeof(PenLineCap), typeof(DrawingBrush), PenLineCap.Flat, propertyChanged: OnStrokeLineCapPropertyChanged);

        /// <summary>
        /// Identifies the Fill dependency property.
        /// </summary>
        public static readonly BindableProperty StrokeLineJoinProperty = BindableProperty.Create(nameof(StrokeLineJoin), typeof(PenLineJoin), typeof(DrawingBrush), PenLineJoin.Bevel, propertyChanged: OnStrokeLineJoinPropertyChanged);

        /// <summary>
        /// Identifies the Fill dependency property.
        /// </summary>
        public static readonly BindableProperty StrokeMiterLimitProperty = BindableProperty.Create(nameof(StrokeMiterLimit), typeof(double), typeof(DrawingBrush), 0d, propertyChanged: OnStrokeMiterLimitPropertyChanged);

        /// <summary>
        /// Identifies the Fill dependency property.
        /// </summary>
        public static readonly BindableProperty AspectProperty = BindableProperty.Create(nameof(Aspect), typeof(Stretch), typeof(DrawingBrush), Stretch.None, propertyChanged: OnAspectPropertyChanged);

        /// <summary>
        /// Identifies the AttachedBrush dependency property
        /// </summary>
        public static readonly BindableProperty AttachedBrushProperty = BindableProperty.CreateAttached("AttachedBrush", typeof(DrawingBrush), typeof(DrawingBrush), null, propertyChanged: OnAttachedBrushPropertyChanged);

        /// <summary>
        /// Identifies the AttachedFill dependency property
        /// </summary>
        public static readonly BindableProperty AttachedFillProperty = BindableProperty.CreateAttached("AttachedFill", typeof(Brush), typeof(DrawingBrush), Brush.Transparent, propertyChanged: OnAttachedFillPropertyChanged);

        /// <summary>
        /// Identifies the  AttachedStroke dependency property
        /// </summary>
        public static readonly BindableProperty AttachedStrokeProperty = BindableProperty.CreateAttached("AttachedStroke", typeof(Brush), typeof(DrawingBrush), Brush.Transparent, propertyChanged: OnAttachedStrokePropertyChanged);

        /// <summary>
        /// Identifies the  AttachedStrokeThickness dependency property
        /// </summary>
        public static readonly BindableProperty AttachedStrokeThicknessProperty = BindableProperty.CreateAttached("AttachedStrokeThickness", typeof(double), typeof(DrawingBrush), 0d, propertyChanged: OnAttachedStrokeThicknessPropertyChanged);

        /// <summary>
        /// Identifies the  AttachedStrokeDashArray dependency property
        /// </summary>
        public static readonly BindableProperty AttachedStrokeDashArrayProperty = BindableProperty.CreateAttached("AttachedStrokeDashArray", typeof(DoubleCollection), typeof(DrawingBrush), new DoubleCollection(), propertyChanged: OnAttachedStrokeDashArrayPropertyChanged);

        /// <summary>
        /// Identifies the  AttachedStrokeDashOffset dependency property.
        /// </summary>
        public static readonly BindableProperty AttachedStrokeDashOffsetProperty = BindableProperty.CreateAttached("AttachedStrokeDashOffset", typeof(double), typeof(DrawingBrush), 0d, propertyChanged: OnAttachedStrokeDashOffsetPropertyChanged);

        /// <summary>
        /// Identifies the AttachedStrokeLineCap dependency property.
        /// </summary>
        public static readonly BindableProperty AttachedStrokeLineCapProperty = BindableProperty.CreateAttached("AttachedStrokeLineCap", typeof(PenLineCap), typeof(DrawingBrush), PenLineCap.Flat, propertyChanged: OnAttachedStrokeLineCapPropertyChanged);

        /// <summary>
        /// Identifies the AttachedStrokeLineJoin dependency property.
        /// </summary>
        public static readonly BindableProperty AttachedStrokeLineJoinProperty = BindableProperty.CreateAttached("AttachedStrokeLineJoin", typeof(PenLineJoin), typeof(DrawingBrush), PenLineJoin.Bevel, propertyChanged: OnAttachedStrokeLineJoinPropertyChanged);

        /// <summary>
        /// Identifies the AttachedStrokeMiterLimit dependency property.
        /// </summary>
        public static readonly BindableProperty AttachedStrokeMiterLimitProperty = BindableProperty.CreateAttached("AttachedStrokeMiterLimit", typeof(double), typeof(DrawingBrush), 0d, propertyChanged: OnAttachedStrokeMiterLimitPropertyChanged);

        /// <summary>
        /// Identifies the AttachedAspect dependency property.
        /// </summary>
        public static readonly BindableProperty AttachedAspectProperty = BindableProperty.CreateAttached("AttachedAspect", typeof(Stretch), typeof(DrawingBrush), Stretch.Fill, propertyChanged: OnAttachedAspectPropertyChanged);

        /// <summary>
        /// Event called when shape geometry changed
        /// </summary>
        public event EventHandler GeometryChanged;

        /// <summary>
        /// Event called when a sub property of current object impact native draws.
        /// </summary>
        public event PropertyChangedEventHandler SubPropertyChanged;

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
        public Brush Stroke
        {
            get => (Brush)this.GetValue(StrokeProperty);
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
        /// Get or set the stroke dash array
        /// </summary>
        public DoubleCollection StrokeDashArray
        {
            get => (DoubleCollection)this.GetValue(StrokeDashArrayProperty);
            set => this.SetValue(StrokeDashArrayProperty, value);
        }

        /// <summary>
        /// Gets or sets the stroke dash offset.
        /// </summary>
        /// <value>
        /// The stroke dash offset.
        /// </value>
        public double StrokeDashOffset
        {
            get => (double)this.GetValue(StrokeDashOffsetProperty);
            set => this.SetValue(StrokeDashOffsetProperty, value);
        }

        /// <summary>
        /// Gets or sets the stroke line cap.
        /// </summary>
        /// <value>
        /// The stroke line cap.
        /// </value>
        public PenLineCap StrokeLineCap
        {
            get => (PenLineCap)this.GetValue(StrokeLineCapProperty);
            set => this.SetValue(StrokeLineCapProperty, value);
        }

        /// <summary>
        /// Gets or sets the stroke line join.
        /// </summary>
        /// <value>
        /// The stroke line join.
        /// </value>
        public PenLineJoin StrokeLineJoin
        {
            get => (PenLineJoin)this.GetValue(StrokeLineJoinProperty);
            set => this.SetValue(StrokeLineJoinProperty, value);
        }

        /// <summary>
        /// Gets or sets the stroke miter limit.
        /// </summary>
        /// <value>
        /// The stroke miter limit.
        /// </value>
        public double StrokeMiterLimit
        {
            get => (double)this.GetValue(StrokeMiterLimitProperty);
            set => this.SetValue(StrokeMiterLimitProperty, value);
        }

        /// <summary>
        /// Gets or sets the aspect.
        /// </summary>
        /// <value>
        /// The aspect.
        /// </value>
        public Stretch Aspect
        {
            get => (Stretch)this.GetValue(AspectProperty);
            set => this.SetValue(AspectProperty, value);
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
        /// Creates a new object that is a copy of the current instance.
        /// </summary>
        /// <returns>A new object that is a copy of this instance.</returns>
        public object Clone()
        {
            var copy = this.CloneDrawingBrush();

            copy.Fill = this.Fill;
            copy.Stroke = this.Stroke;
            copy.StrokeThickness = this.StrokeThickness;
            copy.StrokeDashArray = this.StrokeDashArray;
            copy.StrokeDashOffset = this.StrokeDashOffset;
            copy.StrokeMiterLimit = this.StrokeMiterLimit;
            copy.StrokeLineCap = this.StrokeLineCap;
            copy.StrokeLineJoin = this.StrokeLineJoin;
            copy.Aspect = this.Aspect;

            return copy;
        }

        /// <summary>
        /// Called to define the <see cref="Width"/> and <see cref="Height"/> of the drawable
        /// </summary>
        /// <param name="width">Width of the drawable</param>
        /// <param name="height">Height of the drawable</param>
        public void SetSize(double width, double height)
        {
            if (width < 0 || height < 0)
                return;

            this.Width = width;
            this.Height = height;
            this.OnSizeAllocated(width, height);
        }

        /// <summary>
        /// Get the AttachedBrush property value for the specified bindable object.
        /// </summary>
        /// <param name="bindableObject">Object on which we want the value of the property</param>
        /// <returns>AttachedBrush property value for the bindable object</returns>
        public static DrawingBrush GetAttachedBrush(BindableObject bindableObject) => (DrawingBrush)bindableObject?.GetValue(AttachedBrushProperty);

        /// <summary>
        /// Set the AttachedBrush property value for the specified bindable object. That set a clone of this brush on attached object (in <see cref="VisualElement.Background"/> property).
        /// </summary>
        /// <param name="bindableObject">Object that takes the value</param>
        /// <param name="value">The value to set</param>
        public static void SetAttachedBrush(BindableObject bindableObject, DrawingBrush value) => bindableObject?.SetValue(AttachedBrushProperty, value);

        /// <summary>
        /// Get the AttachedFill property value for the specified bindable object
        /// </summary>
        /// <param name="bindableObject">Object on which we want the value of the property</param>
        /// <returns>AttachFill property value for the bindable object</returns>
        public static Brush GetAttachedFill(BindableObject bindableObject) => (Brush)bindableObject?.GetValue(AttachedFillProperty);

        /// <summary>
        /// Set the AttachedFill property value for the specified bindable object
        /// </summary>
        /// <param name="bindableObject">Object that takes the value</param>
        /// <param name="value">The value to affect</param>
        public static void SetAttachedFill(BindableObject bindableObject, Brush value) => bindableObject?.SetValue(AttachedFillProperty, value);

        /// <summary>
        /// Get the AttachedStroke property value for the specified bindable object
        /// </summary>
        /// <param name="bindableObject">Object on which we want the value of the property</param>
        /// <returns>AttachedStroke property value for the bindable object</returns>
        public static Brush GetAttachedStroke(BindableObject bindableObject) => (Brush)bindableObject?.GetValue(AttachedStrokeProperty);

        /// <summary>
        /// Set the AttachedStroke property value for the specified bindable object
        /// </summary>
        /// <param name="bindableObject">Object that takes the value</param>
        /// <param name="value">The value to affect</param>
        public static void SetAttachedStroke(BindableObject bindableObject, Brush value) => bindableObject?.SetValue(AttachedStrokeProperty, value);

        /// <summary>
        /// Get the AttachedStrokeDashArray property value for the specified bindable object
        /// </summary>
        /// <param name="bindableObject">Object on which we want the value of the property</param>
        /// <returns>AttachedStrokeDashArray property value for the bindable object</returns>
        public static DoubleCollection GetAttachedStrokeDashArray(BindableObject bindableObject) => (DoubleCollection)bindableObject?.GetValue(AttachedStrokeDashArrayProperty);

        /// <summary>
        /// Set the AttachedStrokeDashArray property value for the specified bindable object
        /// </summary>
        /// <param name="bindableObject">Object that takes the value</param>
        /// <param name="value">The value to affect</param>
        public static void SetAttachedStrokeDashArray(BindableObject bindableObject, DoubleCollection value) => bindableObject?.SetValue(AttachedStrokeDashArrayProperty, value);

        /// <summary>
        /// Get the AttachedStrokeThickness property value for the specified bindable object
        /// </summary>
        /// <param name="bindableObject">Object on which we want the value of the property</param>
        /// <returns>AttachedStrokeThickness property value for the bindable object</returns>
        public static double GetAttachedStrokeThickness(BindableObject bindableObject) => (double)bindableObject?.GetValue(AttachedStrokeThicknessProperty);

        /// <summary>
        /// Set the AttachedStrokeThickness property value for the specified bindable object
        /// </summary>
        /// <param name="bindableObject">Object that takes the value</param>
        /// <param name="value">The value to affect</param>
        public static void SetAttachedStrokeThickness(BindableObject bindableObject, double value) => bindableObject?.SetValue(AttachedStrokeThicknessProperty, value);

        /// <summary>
        /// Get the AttachedStrokeDashOffset property value for the specified bindable object
        /// </summary>
        /// <param name="bindableObject">Object on which we want the value of the property</param>
        /// <returns>AttachedStrokeDashOffset property value for the bindable object</returns>
        public static double GetAttachedStrokeDashOffset(BindableObject bindableObject) => (double)bindableObject?.GetValue(AttachedStrokeDashOffsetProperty);

        /// <summary>
        /// Set the AttachedStrokeDashOffset property value for the specified bindable object
        /// </summary>
        /// <param name="bindableObject">Object that takes the value</param>
        /// <param name="value">The value to affect</param>
        public static void SetAttachedStrokeDashOffset(BindableObject bindableObject, double value) => bindableObject?.SetValue(AttachedStrokeDashOffsetProperty, value);

        /// <summary>
        /// Get the AttachedStrokeLineCapProperty property value for the specified bindable object
        /// </summary>
        /// <param name="bindableObject">Object on which we want the value of the property</param>
        /// <returns>AttachedStrokeLineCapProperty property value for the bindable object</returns>
        public static PenLineCap GetAttachedStrokeLineCap(BindableObject bindableObject) => (PenLineCap)bindableObject?.GetValue(AttachedStrokeLineCapProperty);

        /// <summary>
        /// Set the AttachedStrokeLineCapProperty property value for the specified bindable object
        /// </summary>
        /// <param name="bindableObject">Object that takes the value</param>
        /// <param name="value">The value to affect</param>
        public static void SetAttachedStrokeLineCapProperty(BindableObject bindableObject, PenLineCap value) => bindableObject?.SetValue(AttachedStrokeLineCapProperty, value);

        /// <summary>
        /// Get the AttachedStrokeLineCapProperty property value for the specified bindable object
        /// </summary>
        /// <param name="bindableObject">Object on which we want the value of the property</param>
        /// <returns>AttachedStrokeLineCapProperty property value for the bindable object</returns>
        public static PenLineJoin GetAttachedStrokeLineJoinProperty(BindableObject bindableObject) => (PenLineJoin)bindableObject?.GetValue(AttachedStrokeLineJoinProperty);

        /// <summary>
        /// Set the AttachedStrokeLineCapProperty property value for the specified bindable object
        /// </summary>
        /// <param name="bindableObject">Object that takes the value</param>
        /// <param name="value">The value to affect</param>
        public static void SetAttachedStrokeLineJoinProperty(BindableObject bindableObject, PenLineJoin value) => bindableObject?.SetValue(AttachedStrokeLineJoinProperty, value);

        /// <summary>
        /// Get the AttachedStrokeMiterLimitProperty property value for the specified bindable object
        /// </summary>
        /// <param name="bindableObject">Object on which we want the value of the property</param>
        /// <returns>AttachedStrokeMiterLimitProperty property value for the bindable object</returns>
        public static double GetAttachedStrokeMiterLimitProperty(BindableObject bindableObject) => (double)bindableObject?.GetValue(AttachedStrokeMiterLimitProperty);

        /// <summary>
        /// Set the AttachedStrokeMiterLimitProperty property value for the specified bindable object
        /// </summary>
        /// <param name="bindableObject">Object that takes the value</param>
        /// <param name="value">The value to affect</param>
        public static void SetAttachedStrokeMiterLimitProperty(BindableObject bindableObject, double value) => bindableObject?.SetValue(AttachedStrokeMiterLimitProperty, value);

        /// <summary>
        /// Get the AttachedAspectProperty property value for the specified bindable object
        /// </summary>
        /// <param name="bindableObject">Object on which we want the value of the property</param>
        /// <returns>AttachedAspectProperty property value for the bindable object</returns>
        public static Stretch GetAttachedAspectProperty(BindableObject bindableObject) => (Stretch)bindableObject?.GetValue(AttachedAspectProperty);

        /// <summary>
        /// Set the AttachedAspectProperty property value for the specified bindable object
        /// </summary>
        /// <param name="bindableObject">Object that takes the value</param>
        /// <param name="value">The value to affect</param>
        public static void SetAttachedAspectProperty(BindableObject bindableObject, Stretch value) => bindableObject?.SetValue(AttachedAspectProperty, value);

        /// <summary>
        /// Try to found the DrawingBrush in the specified <paramref name="bindable"/> (in <see cref="VisualElement.Background"/> property) and execute <paramref name="toDo"/> if found.
        /// </summary>
        /// <param name="bindable">Bindable object where try found drawing brush</param>
        /// <param name="toDo">Action to execute if drawing brush found</param>
        protected static void TryFoundAndDoOnDrawingBrush(BindableObject bindable, Action<DrawingBrush> toDo)
        {
            if (toDo == null)
                return;

            if (bindable is VisualElement visualElement && visualElement.Background is DrawingBrush drawingBrush)
                toDo(drawingBrush);
        }

        private static void OnFillPropertyChanged(BindableObject bindable, object oldValue, object newValue) => (bindable as DrawingBrush).OnFillChanged((Brush)oldValue);

        private static void OnStrokePropertyChanged(BindableObject bindable, object oldValue, object newValue) => (bindable as DrawingBrush).OnStrokeChanged((Brush)oldValue);

        private static void OnStrokeThicknessPropertyChanged(BindableObject bindable, object oldValue, object newValue) => (bindable as DrawingBrush).OnStrokeThicknessChanged();

        private static void OnStrokeDashArrayPropertyChanged(BindableObject bindable, object oldValue, object newValue) => (bindable as DrawingBrush).OnStrokeDashArrayChanged();

        private static void OnStrokeDashOffsetPropertyChanged(BindableObject bindable, object oldValue, object newValue) => (bindable as DrawingBrush).OnStrokeDashOffsetChanged();

        private static void OnStrokeLineCapPropertyChanged(BindableObject bindable, object oldValue, object newValue) => (bindable as DrawingBrush).OnStrokeLineCapChanged();

        private static void OnStrokeLineJoinPropertyChanged(BindableObject bindable, object oldValue, object newValue) => (bindable as DrawingBrush).OnStrokeLineJoinChanged();

        private static void OnStrokeMiterLimitPropertyChanged(BindableObject bindable, object oldValue, object newValue) => (bindable as DrawingBrush).OnStrokeMiterLimitChanged();

        private static void OnAspectPropertyChanged(BindableObject bindable, object oldValue, object newValue) => (bindable as DrawingBrush).OnAspectChanged();

        private static void OnAttachedBrushPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (newValue == null)
                return;

            DrawingBrush drawingBrush;
            if (bindable is VisualElement visualElement)
            {
                drawingBrush = (DrawingBrush)((DrawingBrush)newValue).Clone();
                visualElement.Background = drawingBrush;
            }
            else
                return;

            SetAttachedFill(bindable, drawingBrush.Fill);
            SetAttachedStroke(bindable, drawingBrush.Stroke);
            SetAttachedStrokeThickness(bindable, drawingBrush.StrokeThickness);
            SetAttachedStrokeDashArray(bindable, drawingBrush.StrokeDashArray);
            SetAttachedStrokeDashOffset(bindable, drawingBrush.StrokeDashOffset);
            SetAttachedStrokeLineCapProperty(bindable, drawingBrush.StrokeLineCap);
            SetAttachedStrokeLineJoinProperty(bindable, drawingBrush.StrokeLineJoin);
            SetAttachedStrokeMiterLimitProperty(bindable, drawingBrush.StrokeMiterLimit);
            SetAttachedAspectProperty(bindable, drawingBrush.Aspect);
        }

        private static void OnAttachedFillPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            TryFoundAndDoOnDrawingBrush(bindable, d => d.Fill = (Brush)newValue);
        }

        private static void OnAttachedStrokePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            TryFoundAndDoOnDrawingBrush(bindable, d => d.Stroke = (Brush)newValue);
        }

        private static void OnAttachedStrokeThicknessPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            TryFoundAndDoOnDrawingBrush(bindable, d => d.StrokeThickness = (double)newValue);
        }

        private static void OnAttachedStrokeDashArrayPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            TryFoundAndDoOnDrawingBrush(bindable, d => d.StrokeDashArray = (DoubleCollection)newValue);
        }

        private static void OnAttachedStrokeDashOffsetPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            TryFoundAndDoOnDrawingBrush(bindable, d => d.StrokeDashOffset = (double)newValue);
        }

        private static void OnAttachedStrokeLineCapPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            TryFoundAndDoOnDrawingBrush(bindable, d => d.StrokeLineCap = (PenLineCap)newValue);
        }

        private static void OnAttachedStrokeLineJoinPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            TryFoundAndDoOnDrawingBrush(bindable, d => d.StrokeLineJoin = (PenLineJoin)newValue);
        }

        private static void OnAttachedStrokeMiterLimitPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            TryFoundAndDoOnDrawingBrush(bindable, d => d.StrokeMiterLimit = (double)newValue);
        }

        private static void OnAttachedAspectPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            TryFoundAndDoOnDrawingBrush(bindable, d => d.Aspect = (Stretch)newValue);
        }

        /// <summary>
        /// Called when <see cref="FillProperty"/> changed for this instance of <see cref="DrawingBrush"/>.
        /// </summary>
        protected virtual void OnFillChanged(Brush oldValue)
        {
            if (oldValue != null)
                oldValue.PropertyChanged -= OnFillPropertyChanged;

            if (this.Fill != null)
                this.Fill.PropertyChanged += OnFillPropertyChanged;
        }

        /// <summary>
        /// Called when <see cref="StrokeProperty"/> changed for this instance of <see cref="DrawingBrush"/>.
        /// </summary>
        protected virtual void OnStrokeChanged(Brush oldValue)
        {
            if (oldValue != null)
                oldValue.PropertyChanged -= OnStrokePropertyChanged;

            if (this.Stroke != null)
                this.Stroke.PropertyChanged += OnStrokePropertyChanged;
        }

        /// <summary>
        /// Called when <see cref="StrokeThicknessProperty"/> changed for this instance of <see cref="DrawingBrush"/>.
        /// </summary>
        protected virtual void OnStrokeThicknessChanged()
        { }

        /// <summary>
        /// Called when <see cref="StrokeDashArrayProperty"/> changed for this instance of <see cref="DrawingBrush"/>.
        /// </summary>
        protected virtual void OnStrokeDashArrayChanged()
        { }

        /// <summary>
        /// Called when <see cref="StrokeDashOffsetProperty"/> changed for this instance of <see cref="DrawingBrush"/>.
        /// </summary>
        protected virtual void OnStrokeDashOffsetChanged()
        { }

        /// <summary>
        /// Called when <see cref="StrokeLineCapProperty"/> changed for this instance of <see cref="DrawingBrush"/>.
        /// </summary>
        protected virtual void OnStrokeLineCapChanged()
        { }

        /// <summary>
        /// Called when <see cref="StrokeLineJoinProperty"/> changed for this instance of <see cref="DrawingBrush"/>.
        /// </summary>
        protected virtual void OnStrokeLineJoinChanged()
        { }

        /// <summary>
        /// Called when <see cref="StrokeMiterLimitProperty"/> changed for this instance of <see cref="DrawingBrush"/>.
        /// </summary>
        protected virtual void OnStrokeMiterLimitChanged()
        { }

        /// <summary>
        /// Called when <see cref="AspectProperty"/> changed for this instance of <see cref="DrawingBrush"/>.
        /// </summary>
        protected virtual void OnAspectChanged()
        { }

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

        /// <summary>
        /// Creates a new <see cref="DrawingBrush"/> that is a copy of the current instance.
        /// Just clone custom properties of inherited classes. The clone method of DrawingBrush already copies its own properties.
        /// </summary>
        /// <returns>A new <see cref="DrawingBrush"/> that is a copy of this instance.</returns>
        protected abstract DrawingBrush CloneDrawingBrush();

        /// <summary>
        /// Called when wub property of current brush changed
        /// </summary>
        /// <param name="propertyName">Name of the property that has changed.</param>
        protected void OnSubPropertyChanged(string propertyName)
        {
            SubPropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void OnFillPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            OnSubPropertyChanged(nameof(this.Fill));
        }

        private void OnStrokePropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            OnSubPropertyChanged(nameof(this.Stroke));
        }
    }
}