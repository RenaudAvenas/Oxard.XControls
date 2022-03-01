using System;
using System.Collections;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Oxard.XControls.MarkupExtensions
{
    /// <summary>
    /// Adds DynamicResource management for BindableObject that are not VisualElement like Brush...
    /// This extension provides a binding.
    /// </summary>
    [ContentProperty(nameof(Key))]
    public class ExtendedDynamicResourceExtension : IMarkupExtension<BindingBase>
    {
        /// <summary>
        /// Get or set the key reference of dynamic resource
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// Get or set the visual element where resources are stored.
        /// Default value is null and Application.Current is used if <see cref="UseReflection"/> is false.
        /// Otherwise, the first visual parent object founded will be used.
        /// </summary>
        public VisualElement Container { get; set; }

        /// <summary>
        /// Get or set a value indicate if container must be resolve by reflection or not.
        /// The default value is false.
        /// </summary>
        public bool UseReflection { get; set; }

        /// <summary>
        /// Provide the dynamic resource value
        /// </summary>
        /// <param name="serviceProvider">The service provider</param>
        /// <returns>The current value of dynamic resource if found; otherwise the default value</returns>
        /// <exception cref="InvalidOperationException">Thrown if target is not BindableObject or Key property is null.</exception>
        public object ProvideValue(IServiceProvider serviceProvider)
        {
            if (Key == null)
                throw new InvalidOperationException("The Key property must be set");

            var provideValueTarget = serviceProvider.GetService<IProvideValueTarget>();

            if (!(provideValueTarget.TargetObject is BindableObject))
                throw new InvalidOperationException($"ElementDynamicResourceExtension must be applied to a BindableObject (current is {provideValueTarget.TargetObject})");

            var bindableProperty = provideValueTarget.TargetProperty as BindableProperty;

            if (UseReflection && Container == null)
            {
                var parents = (IEnumerable)provideValueTarget.GetType().GetProperty("Xamarin.Forms.Xaml.IProvideParentValues.ParentObjects", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).GetValue(provideValueTarget);
                foreach (var parent in parents)
                {
                    if (parent is VisualElement visualElement)
                    {
                        Container = visualElement;
                        break;
                    }
                }
            }

            var associatedDynamicResource = new AssociatedDynamicResource(Container, this.Key, bindableProperty);

            return new Binding(nameof(associatedDynamicResource.Value), source: associatedDynamicResource);
        }

        BindingBase IMarkupExtension<BindingBase>.ProvideValue(IServiceProvider serviceProvider)
        {
            return (BindingBase)this.ProvideValue(serviceProvider);
        }

        private class AssociatedDynamicResource : INotifyPropertyChanged
        {
            private readonly BindableProperty dynamicResourceProperty;

            public event PropertyChangedEventHandler PropertyChanged;

            public AssociatedDynamicResource(Element container, string resourceKey, BindableProperty targetedProperty)
            {
                dynamicResourceProperty = BindableProperty.Create("DynamicResource", targetedProperty.ReturnType, typeof(AssociatedDynamicResource), targetedProperty.DefaultValue, propertyChanged: OnDynamicResourceChanged);
                this.Value = targetedProperty.DefaultValue;

                var realContainer = container ?? Application.Current;
                realContainer.SetDynamicResource(dynamicResourceProperty, resourceKey); ;
            }

            public object Value { get; private set; }

            private void OnDynamicResourceChanged(BindableObject bindable, object oldValue, object newValue)
            {
                Value = newValue;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Value)));
            }
        }
    }
}