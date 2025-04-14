using System.Windows;
using ARSounds.Localization.Properties;
using ARSounds.UI.Common.Windows;
using ARSounds.UI.Wpf.Contracts;
using ARSounds.UI.Wpf.Windows;
using ARSounds.UI.Wpf.Windows.ViewModels;
using ARSounds.UI.Wpf.Windows.Views;

namespace ARSounds.UI.Wpf.Services;

public class DialogService : IDialogService
{
    #region Fields/Consts

    private readonly Dictionary<object, Window> _dialogs = [];

    #endregion

    #region IDialogService Implementation

    public bool? ShowDialog(Window dialog)
    {
        var viewModel = dialog.DataContext;

        if (viewModel is not null)
        {
            _dialogs.Add(viewModel, dialog);
        }

        dialog.Owner ??= (System.Windows.Application.Current.MainWindow.IsVisible ? System.Windows.Application.Current.MainWindow : null);
        dialog.WindowStartupLocation = dialog.Owner is null ? WindowStartupLocation.CenterScreen : WindowStartupLocation.CenterOwner;

        var modalResult = dialog.ShowDialog();

        if (viewModel is not null)
        {
            _dialogs.Remove(viewModel);
        }

        return modalResult;
    }

    public ModalResult ShowDialog(Window? owner, BaseDialogView dialogView, DialogOptions dialogOptions)
    {
        var viewModel = dialogView.DataContext;

        using var dialog = new DialogWindow(dialogView, dialogOptions);

        if (viewModel is not null)
        {
            _dialogs.Add(viewModel, dialog);
        }

        dialog.Owner = owner ?? (System.Windows.Application.Current.MainWindow.IsVisible ? System.Windows.Application.Current.MainWindow : null);
        dialog.WindowStartupLocation = dialog.Owner is null ? WindowStartupLocation.CenterScreen : WindowStartupLocation.CenterOwner;

        var modalResult = dialog.ShowModal();

        if (viewModel is not null)
        {
            _dialogs.Remove(viewModel);
        }

        return modalResult;
    }

    public ModalResult ShowDialog(Window? owner, DialogViewModel viewModel, DialogOptions dialogOptions)
    {
        using var dialog = new DialogWindow(new DialogView(viewModel), dialogOptions);

        _dialogs.Add(viewModel, dialog);

        dialog.Owner = owner ?? (System.Windows.Application.Current.MainWindow.IsVisible ? System.Windows.Application.Current.MainWindow : null);
        dialog.WindowStartupLocation = dialog.Owner is null ? WindowStartupLocation.CenterScreen : WindowStartupLocation.CenterOwner;

        var modalResult = dialog.ShowModal();

        _dialogs.Remove(viewModel);

        return modalResult;
    }

    public ModalResult ShowDialog(
       Window? owner,
       string title,
       string message,
       PluginButtons pluginButtons = PluginButtons.OK | PluginButtons.Cancel,
       string? okButtonText = null,
       string? cancelButtonText = null,
       string? yesButtonText = null,
       string? noButtonText = null,
       DialogImage dialogType = DialogImage.None)
    {
        var imageSource = dialogType switch
        {
            DialogImage.Info => "pack://application:,,,/ARSounds.UI.Wpf;component/Assets/info-100.png",
            DialogImage.Warning => "pack://application:,,,/ARSounds.UI.Wpf;component/Assets/warning-100.png",
            DialogImage.Error => "pack://application:,,,/ARSounds.UI.Wpf;component/Assets/error-100.png",
            _ => null,
        };

        var dialogOptions = new DialogOptions()
        {
            Width = 580,
            MinHeight = 100,
            SizeToContent = SizeToContent.Height,
            IsTitleBarVisible = false
        };

        if (pluginButtons.HasFlag(PluginButtons.Yes))
        {
            dialogOptions.PluginButtons.Add(new PluginButton()
            {
                ButtonOrder = 10,
                ButtonPosition = PluginButtonPosition.Right,
                ButtonType = PluginButtonType.Yes,
                Content = yesButtonText ?? Resources.Yes
            });
        }

        if (pluginButtons.HasFlag(PluginButtons.No))
        {
            dialogOptions.PluginButtons.Add(new PluginButton()
            {
                ButtonOrder = 20,
                ButtonPosition = PluginButtonPosition.Right,
                ButtonType = PluginButtonType.No,
                Content = noButtonText ?? Resources.No
            });
        }

        if (pluginButtons.HasFlag(PluginButtons.OK))
        {
            dialogOptions.PluginButtons.Add(new PluginButton()
            {
                IsDefault = !pluginButtons.HasFlag(PluginButtons.Cancel),
                ButtonOrder = 30,
                ButtonPosition = PluginButtonPosition.Right,
                ButtonType = PluginButtonType.OK,
                Content = okButtonText ?? Resources.OK
            });
        }

        if (pluginButtons.HasFlag(PluginButtons.Cancel))
        {
            dialogOptions.PluginButtons.Add(new PluginButton()
            {
                IsCancel = true,
                IsDefault = true,
                ButtonOrder = 40,
                ButtonPosition = PluginButtonPosition.Right,
                ButtonType = PluginButtonType.Cancel,
                Content = cancelButtonText ?? Resources.Cancel
            });
        }

        var view = new MessageDialogView(
            title,
            message,
            imageSource is not null ? new Uri(imageSource, UriKind.RelativeOrAbsolute) : null);

        return ShowDialog(owner, view, dialogOptions);
    }


    public bool TryGetDialogByViewModel(DialogViewModel view, out Window dialog)
    {
        return _dialogs.TryGetValue(view, out dialog!);
    }

    public bool TryGetDialogByViewModelType(Type viewModelType, out Window dialog)
    {
        foreach (var kvp in _dialogs)
        {
            if (kvp.Key.GetType() == viewModelType)
            {
                dialog = kvp.Value;
                return true;
            }
        }

        dialog = null!;
        return false;
    }

    #endregion
}