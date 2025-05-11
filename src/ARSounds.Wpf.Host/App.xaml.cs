using System.Windows;
using CommonServiceLocator;
using DevToolbox.Core.Contracts;
using DevToolbox.Wpf;

namespace ARSounds.Wpf.Host;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : System.Windows.Application
{
    #region Fields/Consts

    /// <summary>
    /// The event mutex name.
    /// </summary>
    private const string _uniqueEventName = "3F6C896A-EF81-4B88-A8FC-1974E21A9465";

    /// <summary>
    /// The unique mutex name.
    /// </summary>
    private const string _uniqueMutexName = "ARSounds";

    /// <summary>
    /// The singleton application manager.
    /// </summary>
    private readonly SingletonApplicationManager _singletonApplicationManager;

    #endregion

    public App()
    {
        _singletonApplicationManager = new SingletonApplicationManager(_uniqueEventName, _uniqueMutexName);
    }

    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        _singletonApplicationManager.Register(this, async () =>
        {
            IocConfiguration.Setup();
            GlobalExceptionHandler.SetupExceptionHandling();

            var applicationManager = ServiceLocator.Current.GetInstance<IAppUISettings>();
            await applicationManager.InitializeAsync();
        },
        () =>
        {
            Current.MainWindow.Activate();
        });
    }
}