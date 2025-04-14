using System.Windows;
using ARSounds.Localization.Properties;
using ARSounds.UI.Common.Windows;
using ARSounds.UI.Wpf.Contracts;
using ARSounds.UI.Wpf.Windows;
using ARSounds.Wpf.Host.Common.ViewModels;
using CommonServiceLocator;
using Microsoft.Extensions.DependencyInjection;

namespace ARSounds.Wpf.Host.Helpers;

public class GlobalExceptionHandler
{
    #region Fields/Consts

    private static readonly DialogOptions ErrorDialogOptions = new()
    {
        Width = 458,
        Height = 560,
        SizeToContent = SizeToContent.Manual,
        WindowTitle = Resources.Unhandled_exception,
        PluginButtons =
        {
            new()
            {
                IsDefault = true,
                ButtonOrder = 10,
                ButtonPosition = PluginButtonPosition.Right,
                ButtonType = PluginButtonType.OK,
            }
        }
    };

    #endregion

    #region Methods

    public static void SetupExceptionHandling()
    {
        AppDomain.CurrentDomain.UnhandledException += (s, e) =>
        {
            ShowErrorDialog((Exception)e.ExceptionObject);
        };

        System.Windows.Application.Current.DispatcherUnhandledException += (s, e) =>
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

    private static void ShowErrorDialog(Exception exception)
    {
        var dialogService = ServiceLocator.Current.GetService<IDialogService>();

        ArgumentNullException.ThrowIfNull(dialogService, nameof(dialogService));

        dialogService.ShowDialog(null, new ErrorDialogViewModel(exception.Message, exception.StackTrace), ErrorDialogOptions);
    }

    #endregion
}