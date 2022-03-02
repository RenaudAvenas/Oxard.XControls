namespace Oxard.XControls.MarkupExtensions
{
    /// <summary>
    /// Interface for extended dynamic resource implementations.
    /// </summary>
    public interface IExtendedDynamicResourceImplementation
    {
        /// <summary>
        /// Get the current value of dynamic resource.
        /// </summary>
        object Value { get; }
    }
}