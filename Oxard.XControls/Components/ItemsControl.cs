using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Runtime.CompilerServices;
using Xamarin.Forms;

namespace Oxard.XControls.Components
{
    [ContentProperty(nameof(Items))]
    public class ItemsControl : ContentView
    {
        public static readonly BindableProperty ItemsSourceProperty = BindableProperty.Create(nameof(ItemsSource), typeof(IEnumerable), typeof(ItemsControl), null, propertyChanged: ItemsSourcePropertyChanged);
        public static readonly BindableProperty ItemTemplateProperty = BindableProperty.Create(nameof(ItemTemplate), typeof(DataTemplate), typeof(ItemsControl), null, propertyChanged: ItemTemplatePropertyChanged);
        public static readonly BindableProperty ItemTemplateSelectorProperty = BindableProperty.Create(nameof(ItemTemplateSelector), typeof(DataTemplateSelector), typeof(ItemsControl), propertyChanged: ItemTemplateSelectorPropertyChanged);

        private bool isLoaded;
        private IList<View> items;
        private Layout<View> itemsPanel;
        private Dictionary<object, View> generatedItems = new Dictionary<object, View>();

        public Layout<View> ItemsPanel
        {
            get => this.itemsPanel;
            set => this.ChangeLayout(value);
        }

        public IList<View> Items
        {
            get
            {
                if (this.items != null)
                    return this.items;

                this.items = new ObservableCollection<View>();
                this.ItemsSource = this.items;
                return this.items;
            }
        }

        public IEnumerable ItemsSource
        {
            get => (IEnumerable)this.GetValue(ItemsSourceProperty);
            set => this.SetValue(ItemsSourceProperty, value);
        }

        public DataTemplate ItemTemplate
        {
            get => (DataTemplate)this.GetValue(ItemTemplateProperty);
            set => this.SetValue(ItemTemplateProperty, value);
        }

        public DataTemplateSelector ItemTemplateSelector
        {
            get => (DataTemplateSelector)this.GetValue(ItemTemplateSelectorProperty);
            set => this.SetValue(ItemTemplateSelectorProperty, value);
        }

        public bool IsItemItsOwnContainer(object item)
        {
            if (!(item is View))
                return false;

            return this.IsItemItsOwnContainerOverride(item);
        }

        protected View GetContainerForItemOverride(object item)
        {
            throw new NotSupportedException("item must be a view");
        }

        protected virtual bool IsItemItsOwnContainerOverride(object item) => true;

        protected override void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            if (propertyName == "Renderer")
            {
                if (!this.isLoaded)
                {
                    this.isLoaded = true;
                    if (this.ItemsPanel == null)
                        this.ItemsPanel = new StackLayout();
                    else
                        this.RecreateAllItems();
                }
                else
                {
                    this.isLoaded = false;
                    this.Unload();
                }
            }

            base.OnPropertyChanged(propertyName);
        }

        protected virtual void UnloadOverride()
        {
        }

        private static void ItemsSourcePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            (bindable as ItemsControl)?.OnItemsSourceChanged(oldValue as IEnumerable);
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

            this.RecreateAllItems();
        }

        private void ItemsSourceOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (!this.isLoaded)
                return;

            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    int index = e.NewStartingIndex;
                    bool isToAdd = index < 0 || index >= this.ItemsPanel.Children.Count;
                    foreach (object item in e.NewItems)
                    {
                        if (isToAdd)
                            this.CreateAndAddItemFor(item);
                        else
                        {
                            this.CreateAndInsertItemFor(item, index);
                            index++;
                        }
                    }
                    break;
                case NotifyCollectionChangedAction.Move:
                    this.RecreateAllItems();
                    break;
                case NotifyCollectionChangedAction.Remove:
                    foreach (object item in e.OldItems)
                        this.RemoveItemFor(item);
                    break;
                case NotifyCollectionChangedAction.Replace:
                    this.RecreateAllItems();
                    break;
                case NotifyCollectionChangedAction.Reset:
                    this.RecreateAllItems();
                    break;
            }
        }

        private static void ItemTemplatePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            //(bindable as ItemsControl)
        }

        private static void ItemTemplateSelectorPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
        }

        private void ChangeLayout(Layout<View> newLayout)
        {
            if (newLayout == this.itemsPanel)
                return;

            this.itemsPanel = newLayout;

            this.RecreateAllItems();
            this.Content = newLayout;
        }

        private View GetContainerForItem(object item)
        {
            if (this.IsItemItsOwnContainer(item))
            {
                this.generatedItems[item] = (View)item;
                return (View)item;
            }

            var templatedView = item;

            if (this.ItemTemplateSelector != null)
                templatedView = this.ItemTemplateSelector.SelectTemplate(item, this).CreateContent();
            else if (this.ItemTemplate != null)
                templatedView = this.ItemTemplate.CreateContent();

            var view = templatedView as View;
            if (!this.IsItemItsOwnContainer(templatedView))
                view = this.GetContainerForItemOverride(templatedView);

            view.BindingContext = item;

            this.generatedItems[item] = view;
            return view;
        }

        private void RecreateAllItems()
        {
            if (this.ItemsPanel == null || !this.isLoaded)
                return;

            this.ItemsPanel.Children.Clear();

            if (this.ItemsSource == null)
                return;

            foreach (var item in this.ItemsSource)
                this.CreateAndAddItemFor(item);
        }

        private void CreateAndAddItemFor(object item)
        {
            this.ItemsPanel.Children.Add(this.GetContainerForItem(item));
        }

        private void CreateAndInsertItemFor(object item, int index)
        {
            this.ItemsPanel.Children.Insert(index, this.GetContainerForItem(item));
        }

        private void RemoveItemFor(object item)
        {
            if (this.generatedItems.TryGetValue(item, out View generatedItem))
            {
                this.generatedItems.Remove(item);
                this.ItemsPanel.Children.Remove(generatedItem);
            }
        }
    }
}
