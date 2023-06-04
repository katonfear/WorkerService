using Microsoft.Extensions.Logging;
using zoft.MauiExtensions.Controls;
using CommunityToolkit.Maui;
using Camera.MAUI;
using CommunityToolkit.Mvvm;
using CommunityToolkit.Maui.Core;
using Microsoft.Maui.Controls.Compatibility.Hosting;

namespace MobileClient
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit()
                .UseZoftAutoCompleteEntry()
                .UseMauiCommunityToolkitCore()
                .UseMauiCompatibility()
                .UseMauiCameraView()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

#if DEBUG
		builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}