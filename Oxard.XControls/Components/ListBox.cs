using System;
using Xamarin.Forms;

namespace Oxard.XControls.Components
{
    /// <summary>
    /// List of selectable items
    /// </summary>
    public class ListBox : ItemsControl
    {
        /// <summary>
        /// Identifies the SelectedItem dependency property.
        /// </summary>
        public static readonly BindableProperty SelectedItemProperty = BindableProperty.Create(nameof(SelectedItem), typeof(object), typeof(ListBox), defaultBindingMode: BindingMode.TwoWay, propertyChanged: OnSelectedItemPropertyChanged);

        /// <summary>
        /// Identifies the SelectedIndex dependency property.
        /// </summary>
        public static readonly BindableProperty SelectedIndexProperty = BindableProperty.Create(nameof(SelectedIndex), typeof(int), typeof(ListBox), -1, defaultBindingMode: BindingMode.TwoWay, propertyChanged: OnSelectedIndexPropertyChanged);

        /// <summary>
        /// Get or set the selected item
        /// </summary>
        public object SelectedItem
        {
            get => this.GetValue(SelectedItemProperty);
            set => this.SetValue(SelectedItemProperty, value);
        }

        /// <summary>
        /// Get or set the selected index
        /// </summary>
        public int SelectedIndex
        {
            get => (int)this.GetValue(SelectedIndexProperty);
            set => this.SetValue(SelectedIndexProperty, value);
        }

        private static void OnSelectedItemPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            (bindable as ListBox)?.OnSelectedItemChanged(oldValue);
        }

        private static void OnSelectedIndexPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            (bindable as ListBox)?.OnSelectedIndexChanged((int)oldValue);
        }

        /// <summary>
        /// Called when <see cref="SelectedItem"/> changed
        /// </summary>
        /// <param name="oldValue">The old selected item or null</param>
        protected virtual void OnSelectedItemChanged(object oldValue)
        {
            if (oldValue != null)
            {
                var oldListBoxItem = this.GetViewForDataItem<ListBoxItem>(oldValue);
                oldListBoxItem.IsSelected = false;
            }

            if (this.SelectedItem == null)
            {
                this.SelectedIndex = -1;
                return;
            }

            var listBoxItem = this.GetViewForDataItem<ListBoxItem>(this.SelectedItem);
            listBoxItem.IsSelected = true;

            this.SelectedIndex = this.ItemsPanel.Children.IndexOf(listBoxItem);
        }

        /// <summary>
        /// Called when <see cref="SelectedIndex"/> changed
        /// </summary>
        /// <param name="oldValue">The old selected index</param>
        protected virtual void OnSelectedIndexChanged(int oldValue)
        {
            var selectedIndex = this.SelectedIndex;

            if (selectedIndex > this.ItemsPanel.Children.Count)
                selectedIndex = this.ItemsPanel.Children.Count - 1;

            if (selectedIndex < 0)
            {
                this.SelectedItem = null;
                return;
            }

            this.SelectedItem = this.GetDataItemForView(this.ItemsPanel.Children[SelectedIndex]);
        }

        /// <summary>
        /// Return a view that represents an item of ItemsControl. This method is for inherits. If item is not a <see cref="View"/> use a DataTemplate, DataTemplateSelector or change ItemsSource to contains views.
        /// </summary>
        /// <returns>The view for the item</returns>
        protected override View GetContainerForItemOverride()
        {
            return new ListBoxItem() { ListBox = this };
        }

        /// <summary>
        /// Return true if the <paramref name="item"/> is its own container to display
        /// </summary>
        /// <param name="item">Item to check</param>
        /// <returns>True if item its own container otherwise false</returns>
        protected override bool IsItemItsOwnContainerOverride(object item)
        {
            return item is ListBoxItem;
        }
    }
}