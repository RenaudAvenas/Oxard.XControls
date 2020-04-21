using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace Oxard.XControls.Layouts.LayoutAlgorithms
{
    /// <summary>
    /// Layout children on a grid define by rows and columns
    /// </summary>
    public class GridAlgorithm : LayoutAlgorithm
    {
        private ChildTable table;

        /// <summary>
        /// Identifies the ColumnSpacing property.
        /// </summary>
        public static readonly BindableProperty ColumnSpacingProperty = BindableProperty.Create(nameof(ColumnSpacing), typeof(double), typeof(GridAlgorithm), default(double), propertyChanged: OnMeasureLayoutRequested);
        /// <summary>
        /// Identifies the RowSpacing property.
        /// </summary>
        public static readonly BindableProperty RowSpacingProperty = BindableProperty.Create(nameof(RowSpacing), typeof(double), typeof(GridAlgorithm), default(double), propertyChanged: OnMeasureLayoutRequested);

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
        /// Get the columns definition.</summary>
        /// <value>The columns definition.</value>
        public List<ColumnDefinition> ColumnDefinitions { get; } = new List<ColumnDefinition>();

        /// <summary>
        /// Get the row definitions.</summary>
        /// <value>The row definitions.</value>
        public List<RowDefinition> RowDefinitions { get; } = new List<RowDefinition>();

        /// <summary>
        /// Method called when a measurement is asked.
        /// </summary>
        /// <param name="widthConstraint">Width constraint</param>
        /// <param name="heightConstraint">Height constraint</param>
        /// <returns>Requested size</returns>
        protected override SizeRequest OnMeasure(double widthConstraint, double heightConstraint)
        {
            this.GetChildTable(widthConstraint, heightConstraint);
            this.PrepareSizes(this.table);

            return new SizeRequest(this.table.ResultSize);
        }

        /// <summary>
        /// Layout the children of the current layout
        /// </summary>
        /// <param name="x">X delay</param>
        /// <param name="y">Y delay</param>
        /// <param name="width">Width constraint to layout</param>
        /// <param name="height">Height constraint to layout</param>
        protected override void OnLayoutChildren(double x, double y, double width, double height)
        {
            this.GetChildTable(width, height);
            this.PrepareSizes(table);

            var currentY = 0d;
            for (int rowIndex = 0; rowIndex < table.RowInfos.Count; rowIndex++)
            {
                var currentX = 0d;
                for (int columnIndex = 0; columnIndex < table.ColumnInfos.Count; columnIndex++)
                {
                    foreach (var childInfo in this.table.GetChildrenOn(columnIndex, rowIndex))
                    {
                        Layout.LayoutChildIntoBoundingRegion(childInfo.Child, new Rectangle(currentX, currentY, this.GetChildAvailableWidth(childInfo), this.GetChildAvailableHeight(childInfo)));
                    }

                    currentX += this.ColumnSpacing + this.table.ColumnInfos[columnIndex].Width;
                }

                currentY += this.RowSpacing + this.table.RowInfos[rowIndex].Height;
            }
        }

        private ChildTable GetChildTable(double width, double height)
        {
            this.table = new ChildTable(this.ColumnDefinitions, this.RowDefinitions, width, height);
            foreach (var child in this.ParentLayout.Children)
            {
                if (!child.IsVisible)
                    continue;

                table.RegisterChild(child);
            }

            return table;
        }

        /// <summary>
        /// Prepare columns sizes (for auto and star columns and rows).
        /// </summary>
        /// <param name="table">The child table</param>
        /// <returns>The size of Star value on width and height (without taking care of ColumnSpans and RowSpans)</returns>
        private void PrepareSizes(ChildTable table)
        {
            var width = table.MeasuredSize.Width;
            var height = table.MeasuredSize.Height;

            #region Prepare size on width for a star value to zero

            var certainWidth = 0d;
            var totalReservedWidth = 0d;

            var starWidthNumber = 0d;
            var starWidthImpactedAutoColumns = new List<ColumnInfo>();
            for (int column = 0; column < table.ColumnInfos.Count; column++)
            {
                var columnInfo = table.ColumnInfos[column];
                if (columnInfo.ColumnDefinition.Width.IsAbsolute)
                    certainWidth += columnInfo.ColumnDefinition.Width.Value;
                else if (columnInfo.ColumnDefinition.Width.IsStar)
                    starWidthNumber += columnInfo.ColumnDefinition.Width.Value;
                else
                {
                    var columnAutoWidth = 0d;
                    var columnReservedWidth = 0d;
                    bool isStarDependent = false;

                    foreach (var childInfo in columnInfo.ChildInfos)
                    {
                        if (childInfo.LastAutoColumn != columnInfo)
                            continue;

                        if (childInfo.CrossStarColumn)
                            isStarDependent = true;

                        var totalSize = childInfo.GetDesiredSize(width, height);

                        var otherColumnsWidth = 0d;
                        var otherReservedWidth = 0d;
                        if (childInfo.ColumnSpan > 1)
                        {
                            for (int i = 0; i < childInfo.ColumnSpan; i++)
                            {
                                if (i + childInfo.StartColumnIndex == columnInfo.ColumnIndex)
                                    continue;

                                var otherColumnInfo = table.ColumnInfos[i];
                                if (otherColumnInfo.ColumnDefinition.Width.IsStar)
                                    continue;

                                otherColumnsWidth += otherColumnInfo.Width;
                                otherReservedWidth += otherColumnInfo.ReservedWidth;
                            }

                            otherColumnsWidth += (childInfo.ColumnSpan - 1) * this.ColumnSpacing;
                        }

                        var columnWidthForChild = totalSize.Request.Width - otherColumnsWidth;

                        if (!childInfo.CrossStarColumn)
                            columnAutoWidth = Math.Max(columnWidthForChild, columnAutoWidth);
                        else
                            columnReservedWidth = Math.Max(columnReservedWidth, columnWidthForChild - otherReservedWidth);
                    }

                    if (isStarDependent && columnReservedWidth > columnAutoWidth)
                    {
                        starWidthImpactedAutoColumns.Add(columnInfo);
                        columnInfo.IsStarImpacted = true;
                        columnInfo.ReservedWidth = columnReservedWidth - columnAutoWidth;
                        totalReservedWidth += columnInfo.ReservedWidth;
                    }

                    columnInfo.GrewWidth(columnAutoWidth);
                    certainWidth += columnInfo.Width;
                }

                if (column < table.ColumnInfos.Count - 1)
                    certainWidth += this.ColumnSpacing;
            }

            #endregion

            #region Adapt auto columns for star real size

            if (starWidthNumber > 0d)
            {
                var starWidth = double.IsPositiveInfinity(width) ? 0d : width - certainWidth;

                if (starWidth < totalReservedWidth)
                    starWidth = starWidth - (totalReservedWidth - starWidth);

                if (starWidth < 0)
                {
                    // Not enough space to display child, star value is zero and all auto column must take there reserved space
                    foreach (var columnInfo in starWidthImpactedAutoColumns)
                    {
                        columnInfo.GrewWidth(columnInfo.Width + columnInfo.ReservedWidth);
                        certainWidth += columnInfo.ReservedWidth;
                    }

                    // If width is infinty we add space in star columns if necessary
                    if (double.IsPositiveInfinity(width))
                    {
                        var starredColumns = new List<ColumnInfo>();
                        var starValue = 0d;
                        foreach (var columnInfo in this.table.ColumnInfos)
                        {
                            if (!columnInfo.ColumnDefinition.Width.IsStar)
                                continue;

                            starredColumns.Add(columnInfo);
                            foreach (var childInfo in columnInfo.ChildInfos)
                            {
                                if (!childInfo.IsLastChildColumn(columnInfo))
                                    continue;

                                var actualAvailableWidth = this.GetChildAvailableWidth(childInfo);
                                var desiredSize = childInfo.GetDesiredSize(double.PositiveInfinity, double.PositiveInfinity);

                                if (actualAvailableWidth < desiredSize.Request.Width)
                                {
                                    var childStarNumber = 0d;
                                    for (int i = 0; i < childInfo.ColumnSpan; i++)
                                    {
                                        var currentChildColumn = this.table.ColumnInfos[childInfo.StartColumnIndex + i];
                                        if (currentChildColumn.ColumnDefinition.Width.IsStar)
                                            childStarNumber += currentChildColumn.ColumnDefinition.Width.Value;
                                    }

                                    var neededStarSize = (desiredSize.Request.Width - actualAvailableWidth) / childStarNumber;
                                    starValue = Math.Max(starValue, neededStarSize);
                                }
                            }
                        }

                        foreach (var columnInfo in starredColumns)
                        {
                            columnInfo.GrewWidth(starValue * columnInfo.ColumnDefinition.Width.Value);
                            certainWidth += columnInfo.Width;
                        }
                    }
                }
                else
                {
                    var starValue = starWidth / starWidthNumber;
                    foreach (var columnInfo in table.ColumnInfos)
                    {
                        if (columnInfo.ColumnDefinition.Width.IsAbsolute || (!columnInfo.IsStarImpacted && columnInfo.ColumnDefinition.Width.IsAuto))
                            continue;
                        else if (columnInfo.ColumnDefinition.Width.IsStar)
                        {
                            columnInfo.GrewWidth(starValue * columnInfo.ColumnDefinition.Width.Value);
                            certainWidth += columnInfo.Width;
                        }
                        else
                        {
                            foreach (var childInfo in columnInfo.ChildInfos)
                            {
                                if (!childInfo.CrossStarColumn || childInfo.LastAutoColumn != columnInfo)
                                    continue;

                                var actualWidthForChild = 0d;
                                for (int i = 0; i < childInfo.ColumnSpan; i++)
                                {
                                    var otherColumnInfo = table.ColumnInfos[i];
                                    if (otherColumnInfo.ColumnDefinition.Width.IsStar)
                                        actualWidthForChild += starValue * columnInfo.ColumnDefinition.Width.Value;
                                    else
                                        actualWidthForChild += otherColumnInfo.Width;
                                }

                                actualWidthForChild += (childInfo.ColumnSpan - 1) * this.ColumnSpacing;

                                var size = childInfo.GetDesiredSize(width, height);
                                var impact = size.Request.Width - actualWidthForChild;

                                if (impact > 0)
                                {
                                    columnInfo.GrewWidth(columnInfo.Width + impact);
                                    certainWidth += impact;
                                }
                            }
                        }
                    }
                }
            }

            #endregion

            #region Prepare size on height for a star value to zero

            var certainHeight = 0d;
            var totalReservedHeight = 0d;

            var starHeightNumber = 0d;
            var starImpactedAutoRows = new List<RowInfo>();
            for (int row = 0; row < table.RowInfos.Count; row++)
            {
                var rowInfo = table.RowInfos[row];
                if (rowInfo.RowDefinition.Height.IsAbsolute)
                    certainHeight += rowInfo.RowDefinition.Height.Value;
                else if (rowInfo.RowDefinition.Height.IsStar)
                    starHeightNumber += rowInfo.RowDefinition.Height.Value;
                else
                {
                    var rowAutoHeight = 0d;
                    var rowReservedHeight = 0d;
                    bool isStarDependent = false;

                    foreach (var childInfo in rowInfo.ChildInfos)
                    {
                        if (childInfo.LastAutoRow != rowInfo)
                            continue;

                        if (childInfo.CrossStarRow)
                            isStarDependent = true;

                        var childAvailableWidth = this.GetChildAvailableWidthIfNotMeasured(childInfo);
                        var totalSize = childInfo.GetDesiredSize(childAvailableWidth, height);

                        var otherRowsHeight = 0d;
                        var otherReservedHeight = 0d;
                        if (childInfo.RowSpan > 1)
                        {
                            for (int i = 0; i < childInfo.RowSpan; i++)
                            {
                                if (i + childInfo.StartRowIndex == rowInfo.RowIndex)
                                    continue;

                                var otherRowInfo = table.RowInfos[i];
                                if (otherRowInfo.RowDefinition.Height.IsStar)
                                    continue;

                                otherRowsHeight += otherRowInfo.Height;
                                otherReservedHeight += otherRowInfo.ReservedHeight;
                            }

                            otherRowsHeight += (childInfo.RowSpan - 1) * this.RowSpacing;
                        }

                        var rowHeightForChild = totalSize.Request.Height - otherRowsHeight;

                        if (!childInfo.CrossStarRow)
                            rowAutoHeight = Math.Max(rowHeightForChild, rowAutoHeight);
                        else
                            rowReservedHeight = Math.Max(rowReservedHeight, rowHeightForChild - otherReservedHeight);
                    }

                    if (isStarDependent && rowReservedHeight > rowAutoHeight)
                    {
                        starImpactedAutoRows.Add(rowInfo);
                        rowInfo.IsStarImpacted = true;
                        rowInfo.ReservedHeight = rowReservedHeight - rowAutoHeight;
                        totalReservedHeight += rowInfo.ReservedHeight;
                    }

                    rowInfo.GrewHeight(rowAutoHeight);
                    certainHeight += rowInfo.Height;
                }

                if (row < table.RowInfos.Count - 1)
                    certainHeight += this.RowSpacing;
            }

            #endregion

            #region Adapt auto rows for star real size

            if (starHeightNumber > 0d)
            {
                var starHeight = double.IsPositiveInfinity(height) ? 0d : height - certainHeight;

                if (starHeight < totalReservedHeight)
                    starHeight = starHeight - (totalReservedHeight - starHeight);

                if (starHeight <= 0)
                {
                    // Not enough space to display child, star value is zero and all auto row must take there reserved space
                    foreach (var rowInfo in starImpactedAutoRows)
                    {
                        rowInfo.GrewHeight(rowInfo.Height + rowInfo.ReservedHeight);
                        certainHeight += rowInfo.ReservedHeight;
                    }

                    // If height is infinty we add space in star rows if necessary
                    if (double.IsPositiveInfinity(height))
                    {
                        var starredRows = new List<RowInfo>();
                        var starValue = 0d;
                        foreach (var rowInfo in this.table.RowInfos)
                        {
                            if (!rowInfo.RowDefinition.Height.IsStar)
                                continue;

                            starredRows.Add(rowInfo);
                            foreach (var childInfo in rowInfo.ChildInfos)
                            {
                                if (!childInfo.IsLastChildRow(rowInfo))
                                    continue;

                                var actualAvailableHeight = this.GetChildAvailableHeight(childInfo);
                                var desiredSize = childInfo.GetDesiredSize(this.GetChildAvailableWidth(childInfo), double.PositiveInfinity);

                                if (actualAvailableHeight < desiredSize.Request.Height)
                                {
                                    var childStarNumber = 0d;
                                    for (int i = 0; i < childInfo.RowSpan; i++)
                                    {
                                        var currentChildRow = this.table.RowInfos[childInfo.StartRowIndex + i];
                                        if (currentChildRow.RowDefinition.Height.IsStar)
                                            childStarNumber += currentChildRow.RowDefinition.Height.Value;
                                    }

                                    var neededStarSize = (desiredSize.Request.Height - actualAvailableHeight) / childStarNumber;
                                    starValue = Math.Max(starValue, neededStarSize);
                                }
                            }
                        }

                        foreach (var rowInfo in starredRows)
                        {
                            rowInfo.GrewHeight(starValue * rowInfo.RowDefinition.Height.Value);
                            certainHeight += rowInfo.Height;
                        }
                    }
                }
                else
                {
                    var starValue = starHeight / starHeightNumber;
                    foreach (var rowInfo in table.RowInfos)
                    {
                        if (rowInfo.RowDefinition.Height.IsAbsolute || (!rowInfo.IsStarImpacted && rowInfo.RowDefinition.Height.IsAuto))
                            continue;
                        else if (rowInfo.RowDefinition.Height.IsStar)
                        {
                            rowInfo.GrewHeight(starValue * rowInfo.RowDefinition.Height.Value);
                            certainHeight += rowInfo.Height;
                        }
                        else
                        {
                            foreach (var childInfo in rowInfo.ChildInfos)
                            {
                                if (!childInfo.CrossStarRow || childInfo.LastAutoRow != rowInfo)
                                    continue;

                                var actualHeightForChild = 0d;
                                for (int i = 0; i < childInfo.RowSpan; i++)
                                {
                                    var otherRowInfo = table.RowInfos[i];
                                    if (otherRowInfo.RowDefinition.Height.IsStar)
                                        actualHeightForChild += starValue * rowInfo.RowDefinition.Height.Value;
                                    else
                                        actualHeightForChild += otherRowInfo.Height;
                                }

                                actualHeightForChild += (childInfo.RowSpan - 1) * this.RowSpacing;

                                var childAvailableWidth = this.GetChildAvailableWidthIfNotMeasured(childInfo);
                                var size = childInfo.GetDesiredSize(childAvailableWidth, height);
                                var impact = size.Request.Height - actualHeightForChild;

                                if (impact > 0)
                                {
                                    rowInfo.GrewHeight(rowInfo.Height + impact);
                                    certainHeight += impact;
                                }
                            }
                        }
                    }
                }
            }

            #endregion

            #region Fill table size

            table.ResultSize = new Size(certainWidth, certainHeight);

            #endregion
        }

        private double GetChildAvailableWidthIfNotMeasured(ChildInfo child)
        {
            if (child.IsMeasured)
                return double.PositiveInfinity;

            var width = 0d;
            for (int i = 0; i < child.ColumnSpan; i++)
            {
                var columnInfo = this.table.ColumnInfos[i + child.StartColumnIndex];
                width += columnInfo.Width;
            }

            width += (child.ColumnSpan - 1) * this.ColumnSpacing;
            return width;
        }

        private double GetChildAvailableWidth(ChildInfo child)
        {
            var width = 0d;
            for (int i = 0; i < child.ColumnSpan; i++)
            {
                var columnInfo = this.table.ColumnInfos[i + child.StartColumnIndex];
                width += columnInfo.Width;
            }

            width += (child.ColumnSpan - 1) * this.ColumnSpacing;
            return width;
        }

        private double GetChildAvailableHeight(ChildInfo child)
        {
            var height = 0d;
            for (int i = 0; i < child.RowSpan; i++)
            {
                var rowInfo = this.table.RowInfos[i + child.StartRowIndex];
                height += rowInfo.Height;
            }

            height += (child.RowSpan - 1) * this.RowSpacing;
            return height;
        }

        private class ChildTable
        {
            private Dictionary<Tuple<int, int>, List<ChildInfo>> flatChildTable = new Dictionary<Tuple<int, int>, List<ChildInfo>>();

            public ChildTable(List<ColumnDefinition> columnDefinitions, List<RowDefinition> rowDefinitions, double width, double height)
            {
                if (columnDefinitions.Count == 0)
                    this.ColumnInfos = new List<ColumnInfo> { new ColumnInfo(new ColumnDefinition { Width = GridLength.Star }, 0) };
                else
                {
                    this.ColumnInfos = new List<ColumnInfo>(columnDefinitions.Count);
                    for (int i = 0; i < columnDefinitions.Count; i++)
                        this.ColumnInfos.Add(new ColumnInfo(columnDefinitions[i], i));
                }

                if (rowDefinitions.Count == 0)
                    this.RowInfos = new List<RowInfo> { new RowInfo(new RowDefinition { Height = GridLength.Star }, 0) };
                else
                {
                    this.RowInfos = new List<RowInfo>(rowDefinitions.Count);
                    for (int i = 0; i < rowDefinitions.Count; i++)
                        this.RowInfos.Add(new RowInfo(rowDefinitions[i], i));
                }

                this.MeasuredSize = new Size(width, height);
            }

            public Size MeasuredSize { get; set; }

            public Size ResultSize { get; set; }

            public List<ColumnInfo> ColumnInfos { get; }

            public List<RowInfo> RowInfos { get; }

            public void RegisterChild(View child)
            {
                var childInfo = new ChildInfo(child, this.ColumnInfos, this.RowInfos);

                ColumnInfo lastAutoColumn = null;
                for (int i = 0; i < childInfo.ColumnSpan; i++)
                {
                    var columnInfo = this.ColumnInfos[childInfo.StartColumnIndex + i];

                    columnInfo.ChildInfos.Add(childInfo);

                    if (columnInfo.ColumnDefinition.Width.IsAuto)
                        lastAutoColumn = columnInfo;
                    else if (columnInfo.ColumnDefinition.Width.IsStar)
                        childInfo.CrossStarColumn = true;
                }

                RowInfo lastAutoRow = null;
                for (int i = 0; i < childInfo.RowSpan; i++)
                {
                    var rowInfo = this.RowInfos[childInfo.StartRowIndex + i];

                    rowInfo.ChildInfos.Add(childInfo);

                    if (rowInfo.RowDefinition.Height.IsAuto)
                        lastAutoRow = rowInfo;
                    else if (rowInfo.RowDefinition.Height.IsStar)
                        childInfo.CrossStarRow = true;
                }

                childInfo.SetLastAuto(lastAutoColumn, lastAutoRow);

                var key = new Tuple<int, int>(childInfo.StartColumnIndex, childInfo.StartRowIndex);
                if (this.flatChildTable.TryGetValue(key, out var list))
                    list.Add(childInfo);
                else
                    flatChildTable[key] = new List<ChildInfo> { childInfo };
            }

            public IEnumerable<ChildInfo> GetChildrenOn(int columnIndex, int rowIndex)
            {
                var key = new Tuple<int, int>(columnIndex, rowIndex);
                if (flatChildTable.TryGetValue(key, out var list))
                {
                    foreach (var item in list)
                    {
                        yield return item;
                    }
                }
            }
        }

        private class ColumnInfo
        {
            private double? width;

            public ColumnInfo(ColumnDefinition columnDefinition, int columnIndex)
            {
                this.ChildInfos = new List<ChildInfo>();
                this.ColumnDefinition = columnDefinition;
                this.ColumnIndex = columnIndex;
                if (columnDefinition.Width.IsAbsolute)
                    this.width = columnDefinition.Width.Value;
                if (columnDefinition.Width.IsStar)
                    this.width = 0d;
            }

            public ColumnInfo(ColumnDefinition columnDefinition, int columnIndex, ChildInfo firstChild) : this(columnDefinition, columnIndex)
            {
                this.ChildInfos.Add(firstChild);
            }

            public ColumnDefinition ColumnDefinition { get; }

            public int ColumnIndex { get; }

            public double Width
            {
                get
                {
                    if (!width.HasValue)
                        throw new InvalidOperationException("Width must to be setted");

                    return width.Value;
                }
            }

            public List<ChildInfo> ChildInfos { get; }

            public bool IsStarImpacted { get; set; }

            /// <summary>
            /// Get or set the width to reserve for this column if star width is zero in addition of Width value.
            /// </summary>
            public double ReservedWidth { get; set; }

            public void GrewWidth(double maxChildWidth)
            {
                this.width = maxChildWidth;
            }
        }

        private class RowInfo
        {
            private double? height;

            public RowInfo(RowDefinition rowDefinition, int rowIndex)
            {
                this.ChildInfos = new List<ChildInfo>();
                this.RowDefinition = rowDefinition;
                this.RowIndex = rowIndex;
                if (rowDefinition.Height.IsAbsolute)
                    this.height = rowDefinition.Height.Value;
                if (rowDefinition.Height.IsStar)
                    this.height = 0d;
            }

            public RowInfo(RowDefinition rowDefinition, int rowIndex, ChildInfo firstChild) : this(rowDefinition, rowIndex)
            {
                this.ChildInfos.Add(firstChild);
            }

            public RowDefinition RowDefinition { get; }

            public int RowIndex { get; }

            public double Height
            {
                get
                {
                    if (!height.HasValue)
                        throw new InvalidOperationException("Height must to be setted");

                    return height.Value;
                }
            }

            public List<ChildInfo> ChildInfos { get; }

            public bool IsStarImpacted { get; set; }

            /// <summary>
            /// Get or set the height to reserve for this row if star row is zero in addition of Height value.
            /// </summary>
            public double ReservedHeight { get; set; }

            internal void GrewHeight(double maxChildHeight)
            {
                this.height = maxChildHeight;
            }
        }

        private class ChildInfo
        {
            private SizeRequest? desiredSize;

            public ChildInfo(View child, List<ColumnInfo> columns, List<RowInfo> rows)
            {
                this.Child = child;
                this.StartColumnIndex = Grid.GetColumn(child);
                this.StartRowIndex = Grid.GetRow(child);
                this.ColumnSpan = Grid.GetColumnSpan(child);
                this.RowSpan = Grid.GetRowSpan(child);

                if (this.StartColumnIndex >= columns.Count)
                    this.StartColumnIndex = columns.Count - 1;

                if (this.StartRowIndex >= rows.Count)
                    this.StartRowIndex = rows.Count - 1;

                this.StartColumn = columns[this.StartColumnIndex];
                this.StartRow = rows[this.StartRowIndex];

                if (this.ColumnSpan + this.StartColumnIndex > columns.Count)
                    this.ColumnSpan = columns.Count - this.StartColumnIndex;

                if (this.RowSpan + this.StartRowIndex > rows.Count)
                    this.RowSpan = rows.Count - this.StartRowIndex;

                if (this.ColumnSpan < 1)
                    this.ColumnSpan = 1;
                if (this.RowSpan < 1)
                    this.RowSpan = 1;
            }

            public View Child { get; }

            public int StartColumnIndex { get; }

            public int ColumnSpan { get; }

            public int StartRowIndex { get; }

            public int RowSpan { get; }

            public bool CrossStarColumn { get; set; }

            public bool CrossStarRow { get; set; }

            public ColumnInfo LastAutoColumn { get; private set; }

            public RowInfo LastAutoRow { get; private set; }

            public ColumnInfo StartColumn { get; }

            public RowInfo StartRow { get; }

            public bool IsMeasured => this.desiredSize.HasValue;

            public SizeRequest GetDesiredSize(double width, double height)
            {
                if (desiredSize == null)
                    desiredSize = this.Child.Measure(width, height, MeasureFlags.IncludeMargins);

                return desiredSize.Value;
            }

            public bool IsLastChildColumn(ColumnInfo column) => column.ColumnIndex == this.StartColumnIndex + this.ColumnSpan - 1;

            public bool IsLastChildRow(RowInfo row) => row.RowIndex == this.StartRowIndex + this.RowSpan - 1;

            internal void SetLastAuto(ColumnInfo lastAutoColumn, RowInfo lastAutoRow)
            {
                this.LastAutoColumn = lastAutoColumn;
                this.LastAutoRow = lastAutoRow;
            }
        }
    }
}
