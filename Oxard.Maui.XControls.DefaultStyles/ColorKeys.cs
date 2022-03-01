namespace Oxard.Maui.XControls.DefaultStyles
{
    public static class ColorKeys
    {
        public static string ContentBackgroundColor { get; } = nameof(ContentBackgroundColor);
        public static string ButtonForegroundColor { get; } = nameof(ButtonForegroundColor);
        public static string ButtonDisableForegroundColor { get; } = nameof(ButtonDisableForegroundColor);
        public static string ButtonBackgroundColor { get; } = nameof(ButtonBackgroundColor);
        public static string ButtonPressedBackgroundColor { get; } = nameof(ButtonPressedBackgroundColor);
        public static string ButtonDisableBackgroundColor { get; } = nameof(ButtonDisableBackgroundColor);
        public static string SelectedBackgroundColor { get; } = nameof(SelectedBackgroundColor);
        public static string SelectedBorderColor { get; } = nameof(SelectedBorderColor);

        public static string ButtonBackgroundBrush { get; } = nameof(ButtonBackgroundBrush);
        public static string ButtonPressedBackgroundBrush { get; } = nameof(ButtonPressedBackgroundBrush);
        public static string ButtonDisableBackgroundBrush { get; } = nameof(ButtonDisableBackgroundBrush);
        public static string SelectedBackgroundBrush { get; } = nameof(SelectedBackgroundBrush);
        public static string SelectedBorderBrush { get; } = nameof(SelectedBorderBrush);

        internal static void Init()
        {
            // Preserve type with XmlnsDefinition
        }
    }
}