using Oxard.Maui.XControls.Components;

namespace Oxard.Maui.TestApp
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            XControls.Initializer.Init();
            XControls.DefaultStyles.Initializer.Init();

            ContentControl dummy = new ContentControl();

            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                });

            return builder.Build();
        }
    }
}