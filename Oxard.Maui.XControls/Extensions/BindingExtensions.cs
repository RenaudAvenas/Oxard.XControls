namespace Oxard.Maui.XControls.Extensions
{
    /// <summary>
    /// Extends method for <see cref="BindingBase"/> class.
    /// </summary>
    public static class BindingExtensions
    {
        /// <summary>
        /// Clone this bindingBase
        /// </summary>
        /// <param name="bindingBase">Source binding</param>
        /// <returns>Cloned value</returns>
        public static Binding Clone(this BindingBase bindingBase)
        {
            var result = new Binding { FallbackValue = bindingBase.FallbackValue, Mode = bindingBase.Mode, StringFormat = bindingBase.StringFormat, TargetNullValue = bindingBase.TargetNullValue };

            if(bindingBase is Binding binding)
            {
                result.Path = binding.Path;
                result.Converter = binding.Converter;
                result.ConverterParameter = binding.ConverterParameter;
                result.Source = binding.Source;
            }

            return result;
        }
    }
}
