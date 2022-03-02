using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Oxard.XControls.MarkupExtensions
{
    /// <summary>
    /// Implement dynamic resource for bindable object that are not Xamarin.Forms.IResourceProvider (internal interface of xamarin to manage resources).
    /// </summary>
    public class BindableObjectDynamicResource : ExtendedDynamicResourceImplementation<BindableObject>
    {
        private readonly BindableProperty targetProperty;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="targetObject">The object where apply a dynamic resource.</param>
        /// <param name="container">The container which contains the ResourceDictionary where DynamicResource can be found.</param>
        /// <param name="resourceKey">The dynamic resource key.</param>
        /// <param name="provideValueTarget">The provide value target.</param>
        public BindableObjectDynamicResource(BindableObject targetObject, Element container, string resourceKey, IProvideValueTarget provideValueTarget)
            : base(targetObject, container, resourceKey, provideValueTarget)
        {
            this.targetProperty = (BindableProperty)provideValueTarget.TargetProperty;
        }

        /// <summary>
        /// Get the expected type of the dynamic resource.
        /// </summary>
        protected override Type ResourceType => targetProperty.ReturnType;

        /// <summary>
        /// Get the default value of the dynamic resource.
        /// </summary>
        protected override object DefaultValue => targetProperty.DefaultValue;

        /// <summary>
        /// Call when the dynamic resource value changed.
        /// </summary>
        /// <param name="newValue">The new value of dynamic resource.</param>
        protected override void OnDynamicResourceChanged(object newValue)
        {
            this.TargetObject.SetValue(this.targetProperty, newValue);
        }
    }
}