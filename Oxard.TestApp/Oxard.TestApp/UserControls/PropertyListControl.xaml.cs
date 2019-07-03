using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Oxard.TestApp.UserControls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PropertyListControl : TemplatedView
    {
        public static readonly BindableProperty InstanceProperty = BindableProperty.Create(nameof(Instance), typeof(object), typeof(PropertyListControl), propertyChanged: InstancePropertyChanged);
        public static readonly BindableProperty PropertiesProperty = BindableProperty.Create(nameof(Properties), typeof(ObservableCollection<PropertyModel>), typeof(PropertyListControl));
        
        public PropertyListControl()
        {
            this.Properties = new ObservableCollection<PropertyModel>();
            this.InitializeComponent();
        }

        public ObservableCollection<PropertyModel> Properties
        {
            get => (ObservableCollection<PropertyModel>)this.GetValue(PropertiesProperty);
            set => this.SetValue(PropertiesProperty, value);
        }

        public object Instance
        {
            get => this.GetValue(InstanceProperty);
            set => this.SetValue(InstanceProperty, value);
        }

        private static void InstancePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            (bindable as PropertyListControl)?.ReflectInstance();
        }
                
        private void ReflectInstance()
        {
            this.Properties.Clear();

            if (this.Instance == null)
                return;

            var controlType = this.Instance.GetType();

            var priorityProperties = controlType.GetProperties().Where(p => p.DeclaringType == controlType && p.SetMethod?.IsPublic == true && PropertyModel.IsManagedProperty(p)).OrderBy(p => p.Name).ToList();

            var inheritedProperties = new List<PropertyInfo>();
            var type = controlType.BaseType;
            while (type != null)
            {
                inheritedProperties.AddRange(type.GetProperties().Where(p => p.DeclaringType == type && p.SetMethod?.IsPublic == true && PropertyModel.IsManagedProperty(p)).OrderBy(p => p.Name));
                type = type.BaseType;
            }

            foreach (var mainProperty in priorityProperties)
                this.Properties.Add(new PropertyModel(mainProperty, this.Instance, true));
            foreach (var inheritedProperty in inheritedProperties)
                this.Properties.Add(new PropertyModel(inheritedProperty, this.Instance, false));
        }

        public class PropertyModel : INotifyPropertyChanged
        {
            private readonly object instance;
            private string value;

            public PropertyModel(PropertyInfo property, object instance, bool isMainProperty)
            {
                this.Property = property;
                this.instance = instance;
                this.IsMainProperty = isMainProperty;

                if (property.PropertyType == typeof(Color))
                {
                    var color = (Color)property.GetValue(instance);
                    var alpha = color.A != 1 ? ColorElementToHex(color.A) : string.Empty;
                    this.value = $"#{alpha}{ColorElementToHex(color.R)}{ColorElementToHex(color.G)}{ColorElementToHex(color.B)}";
                }
                else
                    this.value = property.GetValue(instance)?.ToString();
            }

            public PropertyInfo Property { get; }

            public bool IsMainProperty { get; }

            public string Value
            {
                get => value;
                set
                {
                    if (this.value != value)
                    {
                        this.value = value;
                        this.OnPropertyChanged();
                        this.SetPropertyOnInstance();
                    }
                }
            }

            public static bool IsManagedProperty(PropertyInfo property)
            {
                return property.PropertyType == typeof(bool)
                    || property.PropertyType == typeof(int)
                    || property.PropertyType == typeof(double)
                    || property.PropertyType == typeof(string)
                    || property.PropertyType.IsEnum
                    || property.PropertyType == typeof(Color);
            }

            private static string ColorElementToHex(double element)
            {
                if (element < 0)
                    return "00";

                return ((int)element * 255).ToString(@"X2");
            }

            private void SetPropertyOnInstance()
            {
                if(this.Property.PropertyType == typeof(bool))
                {
                    if (bool.TryParse(this.value, out bool result))
                        this.Property.SetValue(this.instance, result);
                }
                else if (this.Property.PropertyType == typeof(int))
                {
                    if (int.TryParse(this.value, out int result))
                        this.Property.SetValue(this.instance, result);
                }
                else if (this.Property.PropertyType == typeof(double))
                {
                    if (double.TryParse(this.value, NumberStyles.Any, CultureInfo.InvariantCulture, out double result))
                        this.Property.SetValue(this.instance, result);
                }
                else if (this.Property.PropertyType == typeof(string))
                    this.Property.SetValue(this.instance, this.value);
                else if (this.Property.PropertyType.IsEnum)
                {
                    if (Enum.GetNames(this.Property.PropertyType).Any(n => n == this.value))
                        this.Property.SetValue(this.instance, Enum.Parse(this.Property.PropertyType, this.value));
                }
                else if (this.Property.PropertyType == typeof(Color))
                {
                    if (Regex.IsMatch(this.value, @"^\#(([A-F]|[a-f]|[0-9]){6})$|(([A-F]|[a-f]|[0-9]){8})$"))
                    {
                        var color = Color.FromHex(this.value);
                        this.Property.SetValue(this.instance, color);
                    }
                }
            }

            public event PropertyChangedEventHandler PropertyChanged;

            private void OnPropertyChanged([CallerMemberName] string propertyName = null)
            {
                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}