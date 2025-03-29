using Microsoft.Extensions.Configuration;
using OpenVision.Maui.Controls;

namespace ARSounds.Maui.Host;

public static class MauiProgramHelper
{
    public static IConfigurationBuilder AddJsonFromPackageFile(this IConfigurationBuilder configuration, string fileName)
    {
        using var stream = FileSystem.OpenAppPackageFileAsync(fileName).ConfigureAwait(false).GetAwaiter().GetResult();
        return configuration.AddJsonStream(stream);
    }

    public static IServiceCollection AddSynchronizationContext(this IServiceCollection services)
    {
        if (SynchronizationContext.Current is not null)
            services.AddSingleton(SynchronizationContext.Current);

        return services;
    }

    public static IFontCollection AddFonts(this IFontCollection fonts)
    {
        return fonts.AddFont("ionicons.ttf", "IonIcons")
                    .AddFont("Poppins-Bold.otf", "AppBoldFontFamily")
                    .AddFont("Poppins-Medium.otf", "AppMediumFontFamily")
                    .AddFont("Poppins-Regular.otf", "AppFontFamily")
                    .AddFont("Poppins-SemiBold.otf", "AppSemiBoldFontFamily")
                    .AddFont("Roboto-Bold.ttf", "SecondBoldFontFamily")
                    .AddFont("Roboto-Medium.ttf", "SecondMediumFontFamily")
                    .AddFont("Roboto-Regular.ttf", "SecondFontFamily");
    }

    public static IMauiHandlersCollection AddHandlers(this IMauiHandlersCollection handlers)
    {
        return handlers.AddHandler<ARCamera, ARCameraHandler>();
    }
}
