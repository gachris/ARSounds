using ARSounds.UI.Maui.Contracts;
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

        Page mainPage;

        if (AppUISettings.IsFirstLaunching)
        {
            AppUISettings.IsFirstLaunching = false;

            var backgroundPage = IPlatformApplication.Current?.Services.GetService<BackgroundPage>()!;
            mainPage = new NavigationPage(backgroundPage);
        }
        else
        {
            mainPage = IPlatformApplication.Current?.Services.GetService<AppShell>()!;
        }

        var window = new Window(mainPage)
        {
            Width = width,
            Height = height
        };

        return window;
    }
}
