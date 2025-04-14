using ARSounds.UI.Common.Contracts;
using ARSounds.UI.Maui.Services;
using ARSounds.UI.Maui.Views;
using CommonServiceLocator;

namespace ARSounds.Maui.Host;

public partial class App : Microsoft.Maui.Controls.Application
{
    public App()
    {
        InitializeComponent();
    }

    protected override async void OnStart()
    {
        base.OnStart();

        var applicationManager = ServiceLocator.Current.GetInstance<IAppUISettings>();
        await applicationManager.InitializeAsync();
    }

    protected override Window CreateWindow(IActivationState? activationState)
    {
        const int width = 1200;
        const int height = 850;

        Page page;

        if (AppUISettings.IsFirstLaunching)
        {
            AppUISettings.IsFirstLaunching = false;

            var backgroundPage = ServiceLocator.Current.GetInstance<BackgroundPage>();
            page = new NavigationPage(backgroundPage);
        }
        else
        {
            page = ServiceLocator.Current.GetInstance<AppShell>();
        }

        var window = new Window(page)
        {
            Title = Localization.Properties.Resources.Application_title,
            Width = width,
            Height = height
        };

        var navigationService = ServiceLocator.Current.GetInstance<INavigationService>();
        navigationService.Frame = window;

        return window;
    }
}
