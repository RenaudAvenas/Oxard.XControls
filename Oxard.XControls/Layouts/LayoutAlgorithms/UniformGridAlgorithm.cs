using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;

namespace Oxard.XControls.Layouts.LayoutAlgorithms
{
    /// <summary>
    /// Algorithm that display children in uniform column and row size grid.
    /// By default, children are read from left to right. If you set only <see cref="Rows"/> property and let <see cref="Columns"/> to zero, children will be read from top to bottom.
    /// UniformGridAlgorithm manage Grid.ColumnSpan and Grid.RowSpan properties.
    /// </summary>
    /// <seealso cref="Oxard.XControls.Layouts.LayoutAlgorithms.LayoutAlgorithm" />
    public class UniformGridAlgorithm : LayoutAlgorithm
    {
        private bool isLeftToRight = true;

        /// <summary>
        /// Identifies the Columns property.
        /// </summary>
        public static readonly BindableProperty ColumnsProperty = BindableProperty.Create(nameof(Columns), typeof(int), typeof(UniformGridAlgorithm), default(int), propertyChanged: OnMeasureLayoutRequested);
        /// <summary>
        /// Identifies the Rows property.
        /// </summary>
        public static readonly BindableProperty RowsProperty = BindableProperty.Create(nameof(Rows), typeof(int), typeof(UniformGridAlgorithm), default(int), propertyChanged: OnMeasureLayoutRequested);
        /// <summary>
        /// Identifies the ColumnSpacing property.
        /// </summary>
        public static readonly BindableProperty ColumnSpacingProperty = BindableProperty.Create(nameof(ColumnSpacing), typeof(double), typeof(UniformGridAlgorithm), default(double), propertyChanged: OnMeasureLayoutRequested);
        /// <summary>
        /// Identifies the RowSpacing property.
        /// </summary>
        public static readonly BindableProperty RowSpacingProperty = BindableProperty.Create(nameof(RowSpacing), typeof(double), typeof(UniformGridAlgorithm), default(double), propertyChanged: OnMeasureLayoutRequested);

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
               
        /// <summary>
        /// Method called when a measurement is asked.
        /// </summary>
        /// <param name="widthConstraint">Width constraint</param>
        /// <param name="heightConstraint">Height constraint</param>
        /// <returns>
        /// Requested size
        /// </returns>
        /// <exception cref="System.NotImplementedException"></exception>
        protected override SizeRequest OnMeasure(double widthConstraint, double heightConstraint)
        {
            var columnsAndRows = this.GetColumnsAndRowsNumberAndSetMode();
            double width = 0d, height = 0d;
            bool isWidthMeasured = false, isHeightMeasured = false;
            if (this.ParentLayout.HorizontalOptions.Expands && !double.IsInfinity(widthConstraint))
            {
                width = widthConstraint;
                isWidthMeasured = true;
            }

            if (this.ParentLayout.VerticalOptions.Expands && !double.IsInfinity(heightConstraint))
            {
                height = heightConstraint;
                isHeightMeasured = true;
            }

            if (isWidthMeasured && isHeightMeasured)
                return new SizeRequest(new Size(width, height));

            var widthIsNumber = !double.IsInfinity(widthConstraint);
            var heightIsNumber = !double.IsInfinity(heightConstraint);

            var childWidthConstraint = widthIsNumber ? (widthConstraint - this.ColumnSpacing * (columnsAndRows.columns - 1)) / columnsAndRows.columns : double.PositiveInfinity;
            var childHeightConstraint = heightIsNumber ? (heightConstraint - this.RowSpacing * (columnsAndRows.rows - 1)) / columnsAndRows.rows : double.PositiveInfinity;

            Size minColumnAndRowSize = new Size();
            int column = 0;
            int row = 0;
            foreach (var child in this.ParentLayout.Children.Where(c => c.IsVisible))
            {
                var columnSpan = Math.Max(1, Grid.GetColumnSpan(child));
                if (column + columnSpan > columnsAndRows.columns)
                    columnSpan = columnsAndRows.columns;

                var rowSpan = Math.Max(1, Grid.GetRowSpan(child));
                if (row + rowSpan > columnsAndRows.rows)
                    rowSpan = columnsAndRows.rows;

                var specificChildWidthConstraint = childWidthConstraint;
                if (!isWidthMeasured && columnSpan > 1)
                    specificChildWidthConstraint = childWidthConstraint * columnSpan + this.ColumnSpacing * (columnSpan - 1);

                var specificChildHeightConstraint = childHeightConstraint;
                if (!isHeightMeasured && rowSpan > 1)
                    specificChildHeightConstraint = childHeightConstraint * rowSpan + this.RowSpacing * (rowSpan - 1);

                var childSizeRequest = child.Measure(specificChildWidthConstraint, specificChildHeightConstraint, MeasureFlags.IncludeMargins).Request;

                if (!isWidthMeasured && columnSpan > 1)
                    childSizeRequest.Width = (childSizeRequest.Width - (this.ColumnSpacing * (columnSpan - 1))) / columnSpan;

                if (!isHeightMeasured && rowSpan > 1)
                    childSizeRequest.Height = (childSizeRequest.Height - (this.RowSpacing * (rowSpan - 1))) / rowSpan;

                if (!isWidthMeasured)
                    minColumnAndRowSize.Width = Math.Max(childSizeRequest.Width, minColumnAndRowSize.Width);
                if (!isHeightMeasured)
                    minColumnAndRowSize.Height = Math.Max(childSizeRequest.Height, minColumnAndRowSize.Height);

                column++;
                if (column == columnsAndRows.columns)
                {
                    column = 0;
                    row++;
                }
            }

            var requestWidth = isWidthMeasured ? widthConstraint : minColumnAndRowSize.Width * columnsAndRows.columns + this.ColumnSpacing * (columnsAndRows.columns - 1);
            var requestHeight = isHeightMeasured ? heightConstraint : minColumnAndRowSize.Height * columnsAndRows.rows + this.RowSpacing * (columnsAndRows.rows - 1);

            return new SizeRequest(new Size(requestWidth, requestHeight));
        }

        /// <summary>
        /// Layout the children of the current layout
        /// </summary>
        /// <param name="x">X delay</param>
        /// <param name="y">Y delay</param>
        /// <param name="width">Width constraint to layout</param>
        /// <param name="height">Height constraint to layout</param>
        /// <exception cref="System.NotImplementedException"></exception>
        protected override void OnLayoutChildren(double x, double y, double width, double height)
        {
            var columnsAndRows = this.GetColumnsAndRowsNumberAndSetMode();
            var columnWidth = Math.Max(0, (width - ((columnsAndRows.columns - 1) * this.ColumnSpacing)) / columnsAndRows.columns);
            var rowHeight = Math.Max(0, (height - ((columnsAndRows.rows - 1) * this.RowSpacing)) / columnsAndRows.rows);

            if (this.isLeftToRight)
                this.LayoutChildrenLeftToRight(columnWidth, rowHeight, columnsAndRows.columns, columnsAndRows.rows);
            else
                this.LayoutChildrenTopToBottom(columnWidth, rowHeight, columnsAndRows.columns, columnsAndRows.rows);
        }

        private void LayoutChildrenLeftToRight(double columnWidth, double rowHeight, int columns, int rows)
        {
            int column = 0;
            int row = 0;
            double currentX = 0d;
            double currentY = 0d;

            var currentRemainingChildren = new List<ChildRemaining>();
            foreach (var child in this.ParentLayout.Children.Where(c => c.IsVisible))
            {
                while (true)
                {
                    var otherChildOnColumn = currentRemainingChildren.FirstOrDefault(c => c.Index == column);
                    if (otherChildOnColumn == null)
                        break;

                    column += otherChildOnColumn.Span;
                    otherChildOnColumn.Remaining--;

                    if (otherChildOnColumn.Remaining == 0)
                        currentRemainingChildren.Remove(otherChildOnColumn);

                    if (column == columns)
                    {
                        currentX = 0;
                        currentY += rowHeight + this.RowSpacing;
                        column = 0;
                        row++;
                    }
                    else
                        currentX += otherChildOnColumn.Span * columnWidth + this.ColumnSpacing * otherChildOnColumn.Span;
                }

                var columnSpan = Math.Max(1, Grid.GetColumnSpan(child));
                var rowSpan = Math.Max(1, Grid.GetRowSpan(child));

                if (columnSpan + column > columns)
                    columnSpan = columns - column;

                if (rowSpan + row > rows)
                    rowSpan = rows - row;

                if (rowSpan > 1)
                    currentRemainingChildren.Add(new ChildRemaining(column, rowSpan - 1, columnSpan));

                var childAvailableWidth = columnWidth * columnSpan + (this.ColumnSpacing * (columnSpan - 1));
                var childAvailableHeight = rowHeight * rowSpan + (this.RowSpacing * (rowSpan - 1));

                Layout.LayoutChildIntoBoundingRegion(child, new Rectangle(currentX, currentY, childAvailableWidth, childAvailableHeight));

                column += columnSpan;
                if (column == columns)
                {
                    currentX = 0;
                    currentY += rowHeight + this.RowSpacing;
                    column = 0;
                    row++;
                }
                else
                    currentX += childAvailableWidth + this.ColumnSpacing;

                if (row >= rows)
                    break;
            }
        }

        private void LayoutChildrenTopToBottom(double columnWidth, double rowHeight, int columns, int rows)
        {
            int column = 0;
            int row = 0;
            double currentX = 0d;
            double currentY = 0d;

            var currentRemainingChildren = new List<ChildRemaining>();
            foreach (var child in this.ParentLayout.Children.Where(c => c.IsVisible))
            {
                while (true)
                {
                    var otherChildOnRow = currentRemainingChildren.FirstOrDefault(c => c.Index == row);
                    if (otherChildOnRow == null)
                        break;

                    row += otherChildOnRow.Span;
                    otherChildOnRow.Remaining--;

                    if (otherChildOnRow.Remaining == 0)
                        currentRemainingChildren.Remove(otherChildOnRow);

                    if (row == rows)
                    {
                        currentY = 0;
                        currentX += columnWidth + this.ColumnSpacing;
                        row = 0;
                        column++;
                    }
                    else
                        currentY += otherChildOnRow.Span * rowHeight + this.RowSpacing * otherChildOnRow.Span;
                }

                var columnSpan = Math.Max(1, Grid.GetColumnSpan(child));
                var rowSpan = Math.Max(1, Grid.GetRowSpan(child));

                if (columnSpan + column > columns)
                    columnSpan = columns - column;

                if (rowSpan + row > rows)
                    rowSpan = rows - row;

                if (columnSpan > 1)
                    currentRemainingChildren.Add(new ChildRemaining(row, columnSpan - 1, rowSpan));

                var childAvailableWidth = columnWidth * columnSpan + (this.ColumnSpacing * (columnSpan - 1));
                var childAvailableHeight = rowHeight * rowSpan + (this.RowSpacing * (rowSpan - 1));

                Layout.LayoutChildIntoBoundingRegion(child, new Rectangle(currentX, currentY, childAvailableWidth, childAvailableHeight));

                row += rowSpan;
                if (row == rows)
                {
                    currentY = 0;
                    currentX += columnWidth + this.ColumnSpacing;
                    row = 0;
                    column++;
                }
                else
                    currentY += childAvailableHeight + this.RowSpacing;

                if (column >= columns)
                    break;
            }
        }

        private (int columns, int rows) GetColumnsAndRowsNumberAndSetMode()
        {
            this.isLeftToRight = true;

            var columns = this.Columns < 0 ? 0 : this.Columns;
            var rows = this.Rows < 0 ? 0 : this.Rows;

            if (columns > 0 && rows == 0)
                rows = this.ComputeSpansAndGetRowNumber();
            else if (rows > 0 && columns == 0)
            {
                columns = this.ComputeSpansAndGetColumnNumber();
                isLeftToRight = false;
            }

            if (rows == 0)
                rows = 1;
            if (columns == 0)
                columns = 1;

            return (columns, rows);
        }

        private int ComputeSpansAndGetRowNumber()
        {
            int column = 0;
            int row = 0;

            var lastChildOnLastColumn = false;
            var currentRemainingChildren = new List<ChildRemaining>();
            foreach (var child in this.ParentLayout.Children.Where(c => c.IsVisible))
            {
                lastChildOnLastColumn = false;

                while (true)
                {
                    var otherChildOnColumn = currentRemainingChildren.FirstOrDefault(c => c.Index == column);
                    if (otherChildOnColumn == null)
                        break;

                    column += otherChildOnColumn.Span;
                    otherChildOnColumn.Remaining--;

                    if (otherChildOnColumn.Remaining == 0)
                        currentRemainingChildren.Remove(otherChildOnColumn);

                    if (column >= this.Columns)
                    {
                        column = 0;
                        row++;
                    }
                }

                var columnSpan = Math.Max(1, Grid.GetColumnSpan(child));
                var rowSpan = Math.Max(1, Grid.GetRowSpan(child));

                if (rowSpan > 1)
                    currentRemainingChildren.Add(new ChildRemaining(column, rowSpan - 1, columnSpan));

                column += columnSpan;
                if (column >= this.Columns)
                {
                    // End of columns
                    column = 0;
                    row++;
                    lastChildOnLastColumn = true;
                }
            }

            // row represents the index of row. So number of row is this index plus one even if index is up from end of columns detection (cf End of columns commentary above)
            if (!lastChildOnLastColumn)
                row++;

            if (currentRemainingChildren.Count > 0)
                row += currentRemainingChildren.Max(c => c.Remaining);

            return row;
        }

        private int ComputeSpansAndGetColumnNumber()
        {
            int column = 0;
            int row = 0;

            var lastChildOnLastRow = false;
            var currentRemainingChildren = new List<ChildRemaining>();
            foreach (var child in this.ParentLayout.Children.Where(c => c.IsVisible))
            {
                lastChildOnLastRow = false;

                while (true)
                {
                    var otherChildOnRow = currentRemainingChildren.FirstOrDefault(c => c.Index == row);
                    if (otherChildOnRow == null)
                        break;

                    row += otherChildOnRow.Span;
                    otherChildOnRow.Remaining--;

                    if (otherChildOnRow.Remaining == 0)
                        currentRemainingChildren.Remove(otherChildOnRow);

                    if (row >= this.Rows)
                    {
                        column++;
                        row = 0;
                    }
                }

                var columnSpan = Math.Max(1, Grid.GetColumnSpan(child));
                var rowSpan = Math.Max(1, Grid.GetRowSpan(child));

                if (columnSpan > 1)
                    currentRemainingChildren.Add(new ChildRemaining(row, columnSpan - 1, rowSpan));

                row += rowSpan;
                if (row >= this.Rows)
                {
                    // End of rows
                    column++;
                    row = 0;
                    lastChildOnLastRow = true;
                }
            }

            // column represents the index of column. So number of column is this index plus one even if index is up from end of rows detection (cf End of rows commentary above)
            if (!lastChildOnLastRow)
                column++;

            if (currentRemainingChildren.Count > 0)
                column += currentRemainingChildren.Max(c => c.Remaining);

            return column;
        }

        private class ChildRemaining
        {
            public ChildRemaining(int index, int remaining, int span)
            {
                this.Index = index;
                this.Remaining = remaining;
                this.Span = span;
            }

            public int Index { get; }

            public int Remaining { get; set; }

            public int Span { get; }
        }
    }
}
