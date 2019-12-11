using System;

namespace Oxard.XControls
{
    /// <summary>
    /// Used by Xamarin. Do not use.
    /// </summary>
    [AttributeUsage(AttributeTargets.All)]
    public sealed class PreserveAttribute : Attribute
    {
        /// <summary>
        /// Used by Xamarin. Do not use.
        /// </summary>
        public bool AllMembers;

        /// <summary>
        /// Used by Xamarin. Do not use.
        /// </summary>
        public bool Conditional;

        /// <summary>
        /// Used by Xamarin. Do not use.
        /// </summary>
        public PreserveAttribute(bool allMembers, bool conditional)
        {
            AllMembers = allMembers;
            Conditional = conditional;
        }

        /// <summary>
        /// Used by Xamarin. Do not use.
        /// </summary>
        public PreserveAttribute()
        {
        }
    }
}
