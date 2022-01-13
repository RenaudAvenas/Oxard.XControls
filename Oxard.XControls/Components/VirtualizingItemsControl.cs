using Oxard.XControls.Layouts;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Runtime.CompilerServices;
using Xamarin.Forms;

namespace Oxard.XControls.Components
{
    /// <summary>
    /// Base class for items list with templating and virutalization.
    /// This class need to have a template part named PART_ScrollOwner of type <see cref="ScrollView"/> in its ControlTemplate.
    /// </summary>
    public class VirtualizingItemsControl : ContentView
    {
        private readonly Dictionary<object, GeneratedItem> generatedItems = new Dictionary<object, GeneratedItem>();
        private readonly Queue<View> recyclableViews = new Queue<View>();
        private bool isLoaded;
        private VirtualizingStackLayout itemsPanel;

        /// <summary>
        /// Identifies the ItemsSource dependency property.
        /// </summary>
        public static readonly BindableProperty ItemsSourceProperty = BindableProperty.Create(nameof(ItemsSource), typeof(IList), typeof(VirtualizingItemsControl), null, propertyChanged: ItemsSourcePropertyChanged);

        /// <summary>
        /// Identifies the ItemTemplate dependency property.
        /// </summary>
        public static readonly BindableProperty ItemTemplateProperty = BindableProperty.Create(nameof(ItemTemplate), typeof(DataTemplate), typeof(VirtualizingItemsControl), null, propertyChanged: ItemTemplatePropertyChanged);

        /// <summary>
        /// Identifies the ItemTemplateSelector dependency property.
        /// </summary>
        public static readonly BindableProperty ItemTemplateSelectorProperty = BindableProperty.Create(nameof(ItemTemplateSelector), typeof(DataTemplateSelector), typeof(VirtualizingItemsControl), propertyChanged: ItemTemplateSelectorPropertyChanged);

        /// <summary>
        /// Identifies the AlternationCount property.
        /// </summary>
        public static readonly BindableProperty AlternationCountProperty = BindableProperty.Create(nameof(AlternationCount), typeof(int), typeof(VirtualizingItemsControl), default(int), propertyChanged: OnAlternationCountPropertyChanged);

        /// <summary>
        /// Identifies the key of AlternationIndex dependency property.
        /// </summary>
        public static readonly BindablePropertyKey AlternationIndexPropertyKey = BindableProperty.CreateAttachedReadOnly("AlternationIndex", typeof(int), typeof(VirtualizingItemsControl), default(int));

        /// <summary>
        /// Identifies the AlternationIndex dependency property.
        /// </summary>
        public static readonly BindableProperty AlternationIndexProperty = AlternationIndexPropertyKey.BindableProperty;

        /// <summary>
        /// Identifies the ItemContainerStyle dependency property.
        /// </summary>
        public static readonly BindableProperty ItemContainerStyleProperty = BindableProperty.Create(nameof(ItemContainerStyle), typeof(Style), typeof(VirtualizingItemsControl), propertyChanged: OnItemContainerStylePropertyChanged);

        /// <summary>
        /// Get or set the layout used to display items
        /// </summary>
        public VirtualizingStackLayout ItemsPanel
        {
            get => this.itemsPanel;
            set => this.ChangeLayout(value);
        }

        /// <summary>
        /// Get or set the items source
        /// </summary>
        public IList ItemsSource
        {
            get => (IList)this.GetValue(ItemsSourceProperty);
            set => this.SetValue(ItemsSourceProperty, value);
        }

        /// <summary>
        /// Get or set the data template to use to generate views from items
        /// </summary>
        public DataTemplate ItemTemplate
        {
            get => (DataTemplate)this.GetValue(ItemTemplateProperty);
            set => this.SetValue(ItemTemplateProperty, value);
        }

        /// <summary>
        /// Get or set the data template selector to use to generate views from items
        /// </summary>
        public DataTemplateSelector ItemTemplateSelector
        {
            get => (DataTemplateSelector)this.GetValue(ItemTemplateSelectorProperty);
            set => this.SetValue(ItemTemplateSelectorProperty, value);
        }

        /// <summary>
        /// Get or set the number of alternating item containers in the <see cref="ItemsControl"/>, which enables alternating containers to have a unique appearance.
        /// </summary>
        public int AlternationCount
        {
            get => (int)this.GetValue(AlternationCountProperty);
            set => this.SetValue(AlternationCountProperty, value);
        }

        /// <summary>
        /// Get or set the style of each UI item in the ItemsControl
        /// </summary>
        public Style ItemContainerStyle
        {
            get => (Style)this.GetValue(ItemContainerStyleProperty);
            set => this.SetValue(ItemContainerStyleProperty, value);
        }

        internal ScrollView ScrollOwner { get; private set; }

        /// <summary>
        /// Generate items between <paramref name="minIndex"/> and <paramref name="maxIndex"/> in ItemsSource
        /// </summary>
        /// <param name="minIndex">Start index range</param>
        /// <param name="maxIndex">End index range</param>
        public void GenerateRange(int minIndex, int maxIndex)
        {
            for (int i = minIndex; i <= maxIndex; i++)
                GenerateAt(i);
        }

        /// <summary>
        /// Generate item at <paramref name="index"/> in ItemsSource
        /// </summary>
        /// <param name="index">Index where item will be generated</param>
        /// <returns></returns>
        public View GenerateAt(int index)
        {
            if (this.ItemsSource == null || this.ItemsSource.Count == 0 || index >= ItemsSource.Count)
                return null;

            var item = ItemsSource[index];
            if (this.generatedItems.TryGetValue(item, out var generatedItem))
                return generatedItem.View;

            View view;
            if (recyclableViews.Count > 0)
            {
                view = recyclableViews.Dequeue();
                view.IsVisible = true;
            }
            else
            {
                view = GetContainerForItem(item);
                this.itemsPanel?.Children.Add(view);
            }

            view.BindingContext = item;
            ComputeAlternationIndexForItem(view, index);
            this.ApplyItemToView(item, view, index);

            this.generatedItems[item] = new GeneratedItem { Item = item, Index = index, View = view };

            return view;
        }

        /// <summary>
        /// Recycle item at <paramref name="index"/>. Item will be not visible but can be used by other data.
        /// </summary>
        /// <param name="index">The index</param>
        public void RecycleAt(int index)
        {
            if (index >= ItemsSource.Count)
                return;

            var item = ItemsSource[index];
            if (this.generatedItems.TryGetValue(item, out var generatedItem))
            {
                generatedItem.View.IsVisible = false;
                recyclableViews.Enqueue(generatedItem.View);
                this.generatedItems.Remove(item);
            }
        }

        /// <summary>
        /// Recyle all displayed items
        /// </summary>
        public void RecycleAll()
        {
            foreach (var generatedItem in this.generatedItems.Values)
            {
                generatedItem.View.IsVisible = false;
                recyclableViews.Enqueue(generatedItem.View);
            }

            this.generatedItems.Clear();
        }

        /// <summary>
        /// Returns the items that can be displayed
        /// </summary>
        /// <returns>All items to display</returns>
        public IEnumerable<GeneratedItem> GetDisplayedChildren()
        {
            return this.generatedItems.Values;
        }

        /// <summary>
        /// Get generated view for item. It will returns null if item is not in ItemsSource or if the view to displayed this item is not generated or not of type <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">Type of view</typeparam>
        /// <param name="item">The item</param>
        /// <returns>The generated view for item</returns>
        public T GetViewForDataItem<T>(object item) where T : View
        {
            if (this.generatedItems.TryGetValue(item, out var generatedItem))
                return generatedItem.View as T;

            return null;
        }

        /// <summary>
        /// Calls when applying control template
        /// </summary>
        protected override void OnApplyTemplate()
        {
            this.ScrollOwner = this.GetTemplateChild("PART_ScrollOwner") as ScrollView;
            base.OnApplyTemplate();
        }

        /// <summary>
        /// Return a view that represents an item of ItemsControl. This method is for inherits. If item is not a <see cref="View"/> use a DataTemplate, DataTemplateSelector or change ItemsSource to contains views.
        /// </summary>
        /// <returns>The view for the item</returns>
        protected virtual View GetContainerForItemOverride()
        {
            return null;
        }

        /// <summary>
        /// Called when property changed
        /// </summary>
        /// <param name="propertyName">Name of the property</param>
        protected override void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            if (propertyName == "Renderer")
            {
                if (!this.isLoaded)
                {
                    this.isLoaded = true;
                    if (this.ItemsPanel == null)
                        this.ItemsPanel = new VirtualizingStackLayout();
                    else
                        this.RecreateAllItems(true);
                    this.Loaded();
                }
                else
                {
                    this.isLoaded = false;
                    this.Unload();
                }
            }

            base.OnPropertyChanged(propertyName);
        }

        /// <summary>
        /// Called when items control is loaded
        /// </summary>
        protected virtual void Loaded()
        {
        }

        /// <summary>
        /// Called when ItemsControl is unload
        /// </summary>
        protected virtual void UnloadOverride()
        {
        }

        /// <summary>
        /// Called when <see cref="ItemContainerStyle"/> changed.
        /// </summary>
        protected virtual void OnItemContainerStyleChanged()
        {
            if (ItemsPanel == null)
                return;

            foreach (var item in ItemsPanel.Children)
                item.Style = this.ItemContainerStyle;
        }

        /// <summary>
        /// When overrides, applies specific view container properties from item or item index
        /// </summary>
        /// <param name="item">The item</param>
        /// <param name="view">The view container</param>
        /// <param name="index">The index of item in ItemsSource</param>
        protected virtual void ApplyItemToView(object item, View view, int index)
        {
        }

        /// <summary>
        /// Get the AlternationIndex property value for the specified bindable object
        /// </summary>
        /// <param name="bindableObject">Object on which we want the value of the property</param>
        /// <returns>AlternationIndex property value for the bindable object</returns>
        public static int GetAlternationIndex(BindableObject bindableObject) => ((int?)bindableObject?.GetValue(AlternationIndexProperty)).GetValueOrDefault();

        /// <summary>
        /// Set the AlternationIndex property value for the specified bindable object
        /// </summary>
        /// <param name="bindableObject">Object that takes the value</param>
        /// <param name="value">The value to affect</param>
        private static void SetAlternationIndex(BindableObject bindableObject, int value) => bindableObject.SetValue(AlternationIndexPropertyKey, value);

        private static void OnAlternationCountPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            (bindable as VirtualizingItemsControl)?.OnAlternationCountChanged();
        }

        private static void ItemsSourcePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            (bindable as VirtualizingItemsControl)?.OnItemsSourceChanged(oldValue as IEnumerable);
        }

        private static void OnItemContainerStylePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            (bindable as VirtualizingItemsControl)?.OnItemContainerStyleChanged();
        }

        private void Unload()
        {
            if (this.ItemsSource != null && this.ItemsSource is INotifyCollectionChanged notifyCollectionChanged)
                notifyCollectionChanged.CollectionChanged -= this.ItemsSourceOnCollectionChanged;

            this.UnloadOverride();
        }

        private void OnItemsSourceChanged(IEnumerable oldItemsSource)
        {
            if (oldItemsSource != null && oldItemsSource is INotifyCollectionChanged oldNotifyCollectionChanged)
                oldNotifyCollectionChanged.CollectionChanged -= this.ItemsSourceOnCollectionChanged;

            if (this.ItemsSource != null && this.ItemsSource is INotifyCollectionChanged notifyCollectionChanged)
                notifyCollectionChanged.CollectionChanged += this.ItemsSourceOnCollectionChanged;

            if (this.ItemsPanel == null)
                return;

            this.RecreateAllItems(false);
        }

        private void ItemsSourceOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (!this.isLoaded)
                return;

            this.RecreateAllItems(false);

            //switch (e.Action)
            //{
            //    case NotifyCollectionChangedAction.Add:
            //        int index = e.NewStartingIndex;
            //        bool isToAdd = index < 0 || index >= this.ItemsPanel.Children.Count;
            //        foreach (object item in e.NewItems)
            //        {
            //            if (isToAdd)
            //                this.CreateAndAddItemFor(item);
            //            else
            //            {
            //                this.CreateAndInsertItemFor(item, index);
            //                index++;
            //            }
            //        }
            //        break;

            //    case NotifyCollectionChangedAction.Move:
            //        this.RecreateAllItems(false);
            //        break;

            //    case NotifyCollectionChangedAction.Remove:
            //        foreach (object item in e.OldItems)
            //            this.RemoveItemFor(item);
            //        break;

            //    case NotifyCollectionChangedAction.Replace:
            //        this.RecreateAllItems(false);
            //        break;

            //    case NotifyCollectionChangedAction.Reset:
            //        this.RecreateAllItems(false);
            //        break;
            //}
        }

        private static void ItemTemplatePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
        }

        private static void ItemTemplateSelectorPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
        }

        private void ChangeLayout(VirtualizingStackLayout newLayout)
        {
            if (newLayout == this.itemsPanel)
                return;

            this.itemsPanel = newLayout;
            this.itemsPanel.SetVirtualizingItemsControl(this);

            this.RecreateAllItems(true);
            this.Content = newLayout;
        }

        private View GetContainerForItem(object item)
        {
            var templatedView = item;

            if (this.ItemTemplateSelector != null)
                templatedView = this.ItemTemplateSelector.SelectTemplate(item, this).CreateContent();
            else if (this.ItemTemplate != null)
                templatedView = this.ItemTemplate.CreateContent();

            var view = templatedView as View;
            var itemView = this.GetContainerForItemOverride();
            if (itemView != null)
            {
                if (itemView is ContentView contentView)
                    contentView.Content = view;

                view = itemView;
            }

            if (view == null)
                throw new NotSupportedException($"Type of {item.GetType()} must be a valid item or you must override GetContainerForItemOverride in {this.GetType()}");

            view.BindingContext = item;

            if (this.ItemContainerStyle != null)
                view.Style = this.ItemContainerStyle;

            return view;
        }

        private void RecreateAllItems(bool invalidViews)
        {
            if (this.ItemsPanel == null || !this.isLoaded)
                return;

            if (invalidViews)
            {
                this.ItemsPanel.Children.Clear();
                this.recyclableViews.Clear();
            }
            else
            {
                foreach (var oldItem in this.generatedItems)
                {
                    oldItem.Value.View.IsVisible = false;
                    this.recyclableViews.Enqueue(oldItem.Value.View);
                }
            }

            this.generatedItems.Clear();
            this.ItemsPanel.InitializeItemsSource();
        }

        private void ComputeAlternationIndexForItem(View item, int index)
        {
            if (this.AlternationCount == 0)
                return;

            SetAlternationIndex(item, index % this.AlternationCount);
        }

        private void OnAlternationCountChanged()
        {
            if (this.ItemsPanel == null)
                return;

            foreach (var generatedItem in this.GetDisplayedChildren())
                this.ComputeAlternationIndexForItem(generatedItem.View, generatedItem.Index);
        }
    }

    /// <summary>
    /// Class represents an item of a VirtualizingItemsControl that is generated
    /// </summary>
    public class GeneratedItem
    {
        /// <summary>
        /// Get the data that generate this instance
        /// </summary>
        public object Item { get; internal set; }

        /// <summary>
        /// Get the view that displays data
        /// </summary>
        public View View { get; internal set; }

        /// <summary>
        /// Get the index in the collection of data for this instance
        /// </summary>
        public int Index { get; internal set; }
    }
}
