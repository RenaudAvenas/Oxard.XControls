using Microsoft.Maui.Layouts;
using Oxard.Maui.XControls.Layouts.LayoutAlgorithms;

namespace Oxard.Maui.XControls.Layouts
{
    /// <summary>
    /// Layout that display children in uniform column and row size grid.
    /// By default, children are read from left to right. If you set only <see cref="Rows"/> property and let <see cref="Columns"/> to zero, children will be read from top to bottom.
    /// </summary>
    public class UniformGrid : Layout
    {
        private UniformGridAlgorithm algorithm;

        /// <summary>
        /// Identifies the Columns property.
        /// </summary>
        public static readonly BindableProperty ColumnsProperty = BindableProperty.Create(nameof(Columns), typeof(int), typeof(UniformGrid), default(int), propertyChanged: OnColumnsPropertyChanged);
        /// <summary>
        /// Identifies the Rows property.
        /// </summary>
        public static readonly BindableProperty RowsProperty = BindableProperty.Create(nameof(Rows), typeof(int), typeof(UniformGrid), default(int), propertyChanged: OnRowsPropertyChanged);
        /// <summary>
        /// Identifies the ColumnSpacing property.
        /// </summary>
        public static readonly BindableProperty ColumnSpacingProperty = BindableProperty.Create(nameof(ColumnSpacing), typeof(double), typeof(UniformGrid), default(double), propertyChanged: OnColumnSpacingPropertyChanged);
        /// <summary>
        /// Identifies the RowSpacing property.
        /// </summary>
        public static readonly BindableProperty RowSpacingProperty = BindableProperty.Create(nameof(RowSpacing), typeof(double), typeof(UniformGrid), default(double), propertyChanged: OnRowSpacingPropertyChanged);

        /// <summary>
        /// Get or set the number of columns
        /// </summary>
        public int Columns
        {
            get => (int)this.GetValue(ColumnsProperty);
            set => this.SetValue(ColumnsProperty, value);
        }

        /// <summary>
        /// Get or set the number of rows
        /// </summary>
        public int Rows
        {
            get => (int)this.GetValue(RowsProperty);
            set => this.SetValue(RowsProperty, value);
        }

        /// <summary>
        /// Get or set the space between each columns
        /// </summary>
        public double ColumnSpacing
        {
            get => (double)this.GetValue(ColumnSpacingProperty);
            set => this.SetValue(ColumnSpacingProperty, value);
        }

        /// <summary>
        /// Get or set space between each rows
        /// </summary>
        public double RowSpacing
        {
            get => (double)this.GetValue(RowSpacingProperty);
            set => this.SetValue(RowSpacingProperty, value);
        }

        private static void OnColumnsPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            (bindable as UniformGrid)?.OnColumnsChanged();
        }

        private static void OnRowsPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            (bindable as UniformGrid)?.OnRowsChanged();
        }

        private static void OnColumnSpacingPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            (bindable as UniformGrid)?.OnColumnSpacingChanged();
        }

        private static void OnRowSpacingPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            (bindable as UniformGrid)?.OnRowSpacingChanged();
        }

        /// <summary>
        /// Called when <see cref="Columns"/> changed. Change the Columns property of <seealso cref="UniformGridAlgorithm"/> and invalidate current layout.
        /// </summary>
        protected virtual void OnColumnsChanged()
        {
            if (this.algorithm == null)
                return;

            this.algorithm.Columns = this.Columns;
        }

        /// <summary>
        /// Called when <see cref="Rows"/> changed. Change the Rows property of <seealso cref="UniformGridAlgorithm"/> and invalidate current layout.
        /// </summary>
        protected virtual void OnRowsChanged()
        {
            if (this.algorithm == null)
                return;

            this.algorithm.Rows = this.Rows;
        }

        /// <summary>
        /// Called when <see cref="ColumnSpacing"/> changed. Change the ColumnSpacing property of <seealso cref="UniformGridAlgorithm"/> and invalidate current layout.
        /// </summary>
        protected virtual void OnColumnSpacingChanged()
        {
            if (this.algorithm == null)
                return;

            this.algorithm.ColumnSpacing = this.ColumnSpacing;
        }

        /// <summary>
        /// Called when <see cref="RowSpacing"/> changed. Change the RowSpacing property of <seealso cref="UniformGridAlgorithm"/> and invalidate current layout.
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        protected virtual void OnRowSpacingChanged()
        {
            if (this.algorithm == null)
                return;

            this.algorithm.RowSpacing = this.RowSpacing;
        }

        /// <summary>
        /// Create the layout manager used by the current layout
        /// </summary>
        /// <returns>The layout manager</returns>
        protected override sealed ILayoutManager CreateLayoutManager()
        {
            this.algorithm = new UniformGridAlgorithm(this)
            {
                Columns = this.Columns,
                ColumnSpacing = this.ColumnSpacing,
                Rows = this.Rows,
                RowSpacing = this.RowSpacing,
            };

            return this.algorithm;
        }
    }
}
