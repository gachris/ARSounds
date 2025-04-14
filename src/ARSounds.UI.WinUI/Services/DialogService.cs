using ARSounds.Localization.Properties;
using ARSounds.UI.Common.Windows;
using ARSounds.UI.WinUI.Contracts;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace ARSounds.UI.WinUI.Services;

public class DialogService : IDialogService
{
    #region Fields/Consts

    private readonly Dictionary<object, ContentDialog> _dialogs = [];
    private readonly IAppWindowService _appWindowService;

    #endregion

    public DialogService(IAppWindowService appWindowService)
    {
        _appWindowService = appWindowService;
    }

    #region IDialogService Implementation

    public object GetDialogByView(object view)
    {
        return _dialogs[view];
    }

    public async Task<ModalResult> ShowDialogAsync(
        string title,
        string message,
        PluginButtons pluginButtons = PluginButtons.OK | PluginButtons.Cancel,
        string? okButtonText = null,
        string? cancelButtonText = null,
        DialogImage dialogType = DialogImage.None)
    {
        var dialog = new ContentDialog
        {
            Title = title,
            Content = new StackPanel
            {
                Spacing = 12,
                Children =
                {
                    new TextBlock
                    {
                        Text = message,
                        TextWrapping = TextWrapping.Wrap,
                        FontSize = 16,
                        HorizontalAlignment = HorizontalAlignment.Center,
                        Margin = new Thickness(0, 0, 0, 16)
                    }
                }
            },
            PrimaryButtonText = okButtonText ?? Resources.OK,
            SecondaryButtonText = cancelButtonText ?? Resources.Cancel,
            CloseButtonText = pluginButtons == PluginButtons.Cancel ? Resources.Cancel : null,
            XamlRoot = _appWindowService.MainWindow.Content.XamlRoot
        };

        if (pluginButtons.HasFlag(PluginButtons.OK) && pluginButtons.HasFlag(PluginButtons.Cancel))
        {
            dialog.PrimaryButtonText = okButtonText ?? "Delete";
            dialog.SecondaryButtonText = cancelButtonText ?? "Cancel";
        }
        else if (pluginButtons == PluginButtons.OK)
        {
            dialog.PrimaryButtonText = okButtonText ?? Resources.OK;
        }
        else if (pluginButtons == PluginButtons.Cancel)
        {
            dialog.CloseButtonText = cancelButtonText ?? Resources.Cancel;
        }

        // Show dialog and handle result
        var result = await dialog.ShowAsync();

        var modalResult = result switch
        {
            ContentDialogResult.Primary => ModalResult.OK,
            ContentDialogResult.Secondary => ModalResult.Cancel,
            _ => ModalResult.Cancel
        };

        return modalResult;
    }

    public async Task ShowErrorAsync(Exception exception)
    {
        var dialog = new ContentDialog
        {
            Title = Resources.Unhandled_exception,
            Content = new ScrollViewer
            {
                Content = new TextBlock
                {
                    Text = $"{exception.Message}\n\n{exception.StackTrace}",
                    TextWrapping = TextWrapping.Wrap
                },
                MaxHeight = 560
            },
            PrimaryButtonText = Resources.OK,
            XamlRoot = _appWindowService.MainWindow.Content.XamlRoot
        };

        await dialog.ShowAsync();
    }

    #endregion
}
