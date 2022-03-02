using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Oxard.XControls.MarkupExtensions
{
    /// <summary>
    /// Delegate used to create extended dynamic resource implementation.
    /// </summary>
    /// <param name="targetObject">The bindable object where apply a dynamic resource.</param>
    /// <param name="container">The container which contains the ResourceDictionary where DynamicResource can be found.</param>
    /// <param name="resourceKey">The dynamic resource key.</param>
    /// <param name="targetedProperty">The target property to set with dynamic resource.</param>
    /// <returns>The extended dynamic resource implementation.</returns>
    public delegate IExtendedDynamicResourceImplementation CreateExtendedDynamicResource(object targetObject, Element container, string resourceKey, IProvideValueTarget provideValueTarget);

    /// <summary>
    /// Adds DynamicResource management for BindableObject that are not VisualElement like Brush...
    /// It works too with <see cref="Xamarin.Forms.Setter"/> on <see cref="Xamarin.Forms.Setter.Value"/> property
    /// and with <see cref="Interactivity.Setter"/> on <see cref="Interactivity.Setter.Value"/> property
    /// </summary>
    [ContentProperty(nameof(Key))]
    public class ExtendedDynamicResourceExtension : IMarkupExtension
    {
        private static readonly Dictionary<Type, CreateExtendedDynamicResource> extensions = new Dictionary<Type, CreateExtendedDynamicResource>()
        {
            { typeof(BindableObject), (target, container, resourceKey, provideValueTarget) => new BindableObjectDynamicResource((BindableObject)target, container, resourceKey, provideValueTarget) },
            { typeof(Setter),  (target, container, resourceKey, provideValueTarget) => new XFSetterDynamicResource((Setter)target, container, resourceKey, provideValueTarget) },
            { typeof(Interactivity.Setter),  (target, container, resourceKey, provideValueTarget) => new OxardSetterDynamicResource((Interactivity.Setter)target, container, resourceKey, provideValueTarget) }
        };

        /// <summary>
        /// Get or set the key reference of dynamic resource.
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
        /// Register an extension to manage dynamic resource on specific type.
        /// </summary>
        /// <param name="extendedType">Type to apply dynamic resource</param>
        /// <param name="createExtendedDynamicResource">Method used to create the implementation.</param>
        public static void RegisterExtension(Type extendedType, CreateExtendedDynamicResource createExtendedDynamicResource)
        {
            extensions[extendedType] = createExtendedDynamicResource;
        }

        /// <summary>
        /// Provide the dynamic resource value.
        /// </summary>
        /// <param name="serviceProvider">The service provider.</param>
        /// <returns>The current value of dynamic resource if found; otherwise the default value.</returns>
        /// <exception cref="InvalidOperationException">Thrown if target is not BindableObject or Key property is null.</exception>
        public object ProvideValue(IServiceProvider serviceProvider)
        {
            if (Key == null)
                throw new InvalidOperationException("The Key property must be set");

            var provideValueTarget = serviceProvider.GetService<IProvideValueTarget>();

            var createFunc = GetExtendedDynamicResource(provideValueTarget.TargetObject.GetType());

            if (createFunc == null)
                throw new InvalidOperationException($"ElementDynamicResourceExtension is not supported for {provideValueTarget.TargetObject}.");

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

            var extendedDynamicResource = createFunc(provideValueTarget.TargetObject, Container, this.Key, provideValueTarget);
            ((IExtendedDynamicResourceCreateValueGetter)extendedDynamicResource).CreateValueGetter();
            return extendedDynamicResource.Value;
        }

        private static CreateExtendedDynamicResource GetExtendedDynamicResource(Type targetObjectType)
        {
            foreach (var kvp in extensions)
            {
                if (kvp.Key.IsAssignableFrom(targetObjectType))
                    return kvp.Value;
            }

            return null;
        }
    }
}