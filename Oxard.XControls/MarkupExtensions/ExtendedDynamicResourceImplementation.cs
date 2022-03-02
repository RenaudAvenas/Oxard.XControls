using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Oxard.XControls.MarkupExtensions
{
    /// <summary>
    /// Base class to implement DynamicResource on object of type <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">Type of object where dynamic resource will be set.</typeparam>
    public abstract class ExtendedDynamicResourceImplementation<T> : IExtendedDynamicResourceImplementation, IExtendedDynamicResourceCreateValueGetter
    {
        private BindableProperty dynamicResourceProperty;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="targetObject">The object where apply a dynamic resource.</param>
        /// <param name="container">The container which contains the ResourceDictionary where DynamicResource can be found.</param>
        /// <param name="resourceKey">The dynamic resource key.</param>
        /// <param name="provideValueTarget">The provide value target.</param>
        protected ExtendedDynamicResourceImplementation(T targetObject, Element container, string resourceKey, IProvideValueTarget provideValueTarget)
        {
            TargetObject = targetObject;
            Container = container ?? Application.Current;
            ResourceKey = resourceKey ?? throw new ArgumentNullException(nameof(resourceKey));
            ProvideValueTarget = provideValueTarget ?? throw new ArgumentNullException(nameof(provideValueTarget));
        }

        /// <summary>
        /// Get the object where apply a dynamic resource.
        /// </summary>
        protected T TargetObject { get; }

        /// <summary>
        /// Get the container which contains the ResourceDictionary where DynamicResource can be found.
        /// </summary>
        protected Element Container { get; }

        /// <summary>
        /// Get the dynamic resource key.
        /// </summary>
        protected string ResourceKey { get; }

        /// <summary>
        /// Get the expected type of the dynamic resource.
        /// </summary>
        protected virtual Type ResourceType { get; } = typeof(object);

        /// <summary>
        /// Get the default value of the dynamic resource.
        /// </summary>
        protected virtual object DefaultValue { get; } = null;

        /// <summary>
        /// Get the provide value target.
        /// </summary>
        protected IProvideValueTarget ProvideValueTarget { get; }

        /// <summary>
        /// Get the current value of dynamic resource.
        /// </summary>
        public object Value { get; private set; }

        /// <summary>
        /// Call when the dynamic resource value changed.
        /// </summary>
        /// <param name="newValue">The new value of dynamic resource.</param>
        protected abstract void OnDynamicResourceChanged(object newValue);

        private void OnDynamicResourceChanged(BindableObject bindable, object oldValue, object newValue)
        {
            this.Value = newValue;
            this.OnDynamicResourceChanged(newValue);
        }

        void IExtendedDynamicResourceCreateValueGetter.CreateValueGetter()
        {
            this.dynamicResourceProperty = BindableProperty.Create("DynamicResource", this.ResourceType, typeof(ExtendedDynamicResourceImplementation<T>), this.DefaultValue, propertyChanged: OnDynamicResourceChanged);
            this.Value = DefaultValue;

            Container.SetDynamicResource(dynamicResourceProperty, ResourceKey);
        }
    }
}