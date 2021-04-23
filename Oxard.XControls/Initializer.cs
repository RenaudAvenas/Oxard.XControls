using Oxard.XControls.Components;

namespace Oxard.XControls
{
    /// <summary>
    /// Use this class to force Xamarin compiler to locate Oxard.XControls library (if you want to use XmlnsDefinition it can be required).
    /// </summary>
    public static class Initializer
    {
        /// <summary>
        /// Force Oxard.XControls assembly loading
        /// </summary>
        public static  void Init()
        {
            var buttonFake = new Button();
        }
    }
}
