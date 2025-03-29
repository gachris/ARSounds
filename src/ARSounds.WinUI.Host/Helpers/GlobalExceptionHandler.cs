using ARSounds.UI.WinUI.Contracts;
using CommonServiceLocator;
using Microsoft.Extensions.DependencyInjection;

namespace ARSounds.WinUI.Host.Helpers;

public class GlobalExceptionHandler
{
    #region Methods

    public static void SetupExceptionHandling()
    {
        AppDomain.CurrentDomain.UnhandledException += (s, e) =>
            ShowErrorDialog((Exception)e.ExceptionObject);

        Microsoft.UI.Xaml.Application.Current.DebugSettings.XamlResourceReferenceFailed += (s, e) =>
        {
            ShowErrorDialog(new Exception(e.Message));
        };

        Microsoft.UI.Xaml.Application.Current.UnhandledException += (s, e) =>
        {
            ShowErrorDialog(e.Exception);
            e.Handled = true;
        };

        TaskScheduler.UnobservedTaskException += (s, e) =>
        {
            ShowErrorDialog(e.Exception);
            e.SetObserved();
        };
    }

    private static async void ShowErrorDialog(Exception exception)
    {
        var dialogService = ServiceLocator.Current.GetService<IDialogService>();

        ArgumentNullException.ThrowIfNull(dialogService, nameof(dialogService));

        await dialogService.ShowErrorAsync(exception);
    }

    #endregion
}