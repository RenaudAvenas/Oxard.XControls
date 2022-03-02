using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Oxard.XControls.MarkupExtensions
{
    /// <summary>
    /// Implement dynamic resource for <see cref="Interactivity.Setter"/>.
    /// </summary>
    public class OxardSetterDynamicResource : ExtendedDynamicResourceImplementation<Interactivity.Setter>
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="targetObject">The setter where apply a dynamic resource.</param>
        /// <param name="container">The container which contains the ResourceDictionary where DynamicResource can be found.</param>
        /// <param name="resourceKey">The dynamic resource key.</param>
        /// <param name="provideValueTarget">The provide value target.</param>
        public OxardSetterDynamicResource(Interactivity.Setter targetObject, Element container, string resourceKey, IProvideValueTarget provideValueTarget)
            : base(targetObject, container, resourceKey, provideValueTarget)
        {
        }

        /// <summary>
        /// Get the expected type of the dynamic resource.
        /// </summary>
        protected override Type ResourceType => TargetObject.Property.ReturnType;

        /// <summary>
        /// Get the default value of the dynamic resource.
        /// </summary>
        protected override object DefaultValue => TargetObject.Property.DefaultValue;

        /// <summary>
        /// Call when the dynamic resource value changed.
        /// </summary>
        /// <param name="newValue">The new value of dynamic resource.</param>
        protected override void OnDynamicResourceChanged(object newValue)
        {
            this.TargetObject.Value = newValue;
        }
    }
}