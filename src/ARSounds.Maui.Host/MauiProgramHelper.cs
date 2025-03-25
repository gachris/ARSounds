using ARSounds.UI.Maui.Controls.Videos;
using ARSounds.UI.Maui.Handlers;
using Microsoft.Extensions.Configuration;
using OpenVision.Maui.Controls;
using SkiaSharp.Views.Maui.Controls;
using SkiaSharp.Views.Maui.Handlers;

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
        return fonts.AddFont("Poppins-Regular.otf", "AppFontFamily")
                    .AddFont("Poppins-Medium.otf", "AppMediumFontFamily")
                    .AddFont("Poppins-SemiBold.otf", "AppSemiBoldFontFamily")
                    .AddFont("Poppins-Bold.otf", "AppBoldFontFamily")
                    .AddFont("SecondFontFamily-Bold.ttf", "SecondBoldFontFamily")
                    .AddFont("SecondFontFamily-Medium.ttf", "SecondMediumFontFamily")
                    .AddFont("SecondFontFamily-Regular.ttf", "SecondFontFamily")
                    .AddFont("fa-solid-900.ttf", "FaPro")
                    .AddFont("fa-brands-400.ttf", "FaBrands")
                    .AddFont("fa-regular-400.ttf", "FaRegular")
                    .AddFont("line-awesome.ttf", "LineAwesome")
                    .AddFont("material-icons-outlined-regular.otf", "MaterialDesign")
                    .AddFont("ionicons.ttf", "IonIcons")
                    .AddFont("icon.ttf", "MauiKitIcons");
    }

    public static IMauiHandlersCollection AddHandlers(this IMauiHandlersCollection handlers)
    {
        return handlers.AddHandler(typeof(ARCamera), typeof(ARCameraHandler))
                       .AddHandler(typeof(SKCanvasView), typeof(SKCanvasViewHandler))
                       .AddHandler(typeof(Video), typeof(VideoHandler));
    }
}
