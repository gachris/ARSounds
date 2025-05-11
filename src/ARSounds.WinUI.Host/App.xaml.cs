using CommonServiceLocator;
using DevToolbox.Core.Contracts;
using DevToolbox.WinUI;
using DevToolbox.WinUI.Contracts;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;

namespace ARSounds.WinUI.Host;

/// <summary>
/// Provides application-specific behavior to supplement the default Application class.
/// </summary>
public partial class App : Microsoft.UI.Xaml.Application
{
    /// <summary>
    /// Initializes the singleton application object.  This is the first line of authored code
    /// executed, and as such is the logical equivalent of main() or WinMain().
    /// </summary>
    public App()
    {
        InitializeComponent();

        IocConfiguration.Setup();
        GlobalExceptionHandler.SetupExceptionHandling();
    }

    protected async override void OnLaunched(LaunchActivatedEventArgs args)
    {
        base.OnLaunched(args);

        var applicationManager = ServiceLocator.Current.GetInstance<IAppUISettings>();
        await applicationManager.InitializeAsync();

        await ServiceLocator.Current.GetService<IActivationService>()!.ActivateAsync(args);
    }
}
