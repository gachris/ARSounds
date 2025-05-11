using DevToolbox.Core;
using ARSounds.UI.Common.Data;
using ARSounds.UI.Common.ViewModels;
using ARSounds.UI.Maui.Services;
using ARSounds.UI.Maui.Views;
using DevToolbox.Core.Contracts;
using DevToolbox.Core.Services;
using Microsoft.Extensions.Configuration;

namespace ARSounds.UI.Maui;

public static class ServiceCollectionExtensions
{
    #region Methods

    public static IServiceCollection AddUI(this IServiceCollection services)
    {
        // Services
        services.AddSingleton<IAppUISettings, AppUISettings>();
        services.AddSingleton<INavigationService, NavigationService>();
        services.AddSingleton<IFileService, FileService>();
        services.AddSingleton<ILocalSettingsService, LocalSettingsService>();
        services.AddIUIServices(typeof(ServiceCollectionExtensions).Assembly);

        // Pages
        services.AddPageService(options =>
        {
            options.Configure(PageKeys.CameraPage, typeof(ARCameraPage));
            options.Configure(PageKeys.SettingsPage, typeof(SettingsPage));
            options.Configure(PageKeys.BackgroundPage, typeof(BackgroundPage));
            options.Configure(PageKeys.WalkthroughPage, typeof(WalkthroughPage));
        });

        // Controls
        services.AddSingleton<AppShell>();
        services.AddSingleton<ARCameraPage>();
        services.AddSingleton<SettingsPage>();
        services.AddSingleton<BackgroundPage>();
        services.AddSingleton<WalkthroughPage>();

        // ViewModels
        services.AddSingleton<ShellViewModel>();
        services.AddSingleton<AccountViewModel>();
        services.AddSingleton<ARCameraViewModel>();
        services.AddSingleton<SettingsViewModel>();
        services.AddSingleton<BackgroundViewModel>();
        services.AddSingleton<WalkthroughViewModel>();

        // AutoMapper
        services.AddAutoMapper(typeof(ServiceCollectionExtensions).Assembly);

        return services;
    }

    public static void AddMaterialIcons(this IFontCollection fonts)
    {
        fonts.AddFont("MaterialIcons-Regular.ttf", "MaterialIconsFilled");
        fonts.AddFont("MaterialIconsOutlined-Regular.otf", "MaterialIconsOutlined");
        fonts.AddFont("MaterialIconsRound-Regular.otf", "MaterialIconsRound");
        fonts.AddFont("MaterialIconsSharp-Regular.otf", "MaterialIconsSharp");
        fonts.AddFont("MaterialIconsTwoTone-Regular.otf", "MaterialIconsTwoTone");
    }

    public static IConfigurationBuilder AddJsonStreamPackageFile(this IConfigurationBuilder configuration, string fileName)
    {
        using var stream = FileSystem.OpenAppPackageFileAsync(fileName).ConfigureAwait(false).GetAwaiter().GetResult();
        return configuration.AddJsonStream(stream);
    }

    public static MauiAppBuilder ConfigureServices(this MauiAppBuilder mauiAppBuilder, Action<IServiceCollection> configureDelegate)
    {
        configureDelegate.Invoke(mauiAppBuilder.Services);
        return mauiAppBuilder;
    }

    #endregion
}