using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Oxard.TestApp.UserControls
{
    [DefaultProperty("Control")]
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PropertyListControl : ContentView
    {
        public static readonly BindableProperty PropertyValueProperty = BindableProperty.Create(nameof(PropertyValue), typeof(string), typeof(PropertyListControl), propertyChanged: PropertyValuePropertyChanged);
        
        public PropertyListControl()
        {
            this.InitializeComponent();
        }

        public string PropertyValue
        {
            get => (string)this.GetValue(PropertyValueProperty);
            set => this.SetValue(PropertyValueProperty, value);
        }

        protected override void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);
            if (propertyName == nameof(this.Content))
                this.ReflectControl();
        }

        private static void PropertyValuePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
        }

        private void ReflectControl()
        {
            var controlType = this.Content.GetType();

            var priorityProperties = controlType.GetProperties().Where(p => p.DeclaringType == controlType).ToList();
            var inheritedProperties = controlType.GetProperties().Where(p => p.DeclaringType != controlType).ToList();
        }
    }
}