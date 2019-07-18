namespace Oxard.XControls.Shapes
{
    /// <summary>
    /// Enum of stretch modes
    /// </summary>
    public enum Stretch
    {
        /// <summary>
        /// Stretch on width and height
        /// </summary>
        Fill,
        /// <summary>
        /// Do not stretch
        /// </summary>
        None,
        /// <summary>
        /// Stretch until all is visible and preserve proportions
        /// </summary>
        Uniform,
        /// <summary>
        /// Stretch until all is filled and preserve proportions
        /// </summary>
        UniformToFill
    }
}
