using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

        public Layout<View> ItemsPanel
        {
            get => itemsPanel;
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

        protected override void OnSizeAllocated(double width, double height)
        {
            if (!this.isLoaded)
            {
                this.isLoaded = true;
                if (this.ItemsPanel == null)
                    this.ItemsPanel = new StackLayout();
                else
                    RecreateAllItems();
            }

            base.OnSizeAllocated(width, height);
        }

        protected virtual bool IsItemItsOwnContainerOverride(object item) => true;

        private static void ItemsSourcePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
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
            if (!this.isLoaded || newLayout == this.itemsPanel)
                return;

            this.itemsPanel = newLayout;

            this.RecreateAllItems();
            this.Content = newLayout;
        }

        private View GetContainerForItem(object item)
        {
            var templatedView = item;

            if (this.ItemTemplateSelector != null)
                templatedView = this.ItemTemplateSelector.SelectTemplate(item, this).CreateContent();
            else if(this.ItemTemplate != null)
                templatedView = this.ItemTemplate.CreateContent();

            var view = templatedView as View;
            if(!this.IsItemItsOwnContainer(templatedView))
                 view = this.GetContainerForItemOverride(templatedView);
            
            view.BindingContext = item;
            return view;
        }

        private void RecreateAllItems()
        {
            if (this.ItemsPanel == null || !this.isLoaded)
                return;

            foreach (var item in this.ItemsSource)
            {
                if (this.IsItemItsOwnContainer(item))
                    this.ItemsPanel.Children.Add((View)item);
                else
                    this.ItemsPanel.Children.Add(GetContainerForItem(item));
            }
        }
    }
}
