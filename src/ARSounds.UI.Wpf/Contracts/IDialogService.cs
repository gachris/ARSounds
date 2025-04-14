using System.Windows;
using ARSounds.UI.Common.Windows;
using ARSounds.UI.Wpf.Windows;
using ARSounds.UI.Wpf.Windows.ViewModels;
using ARSounds.UI.Wpf.Windows.Views;

namespace ARSounds.UI.Wpf.Contracts;

public interface IDialogService
{
    bool? ShowDialog(Window dialog);

    ModalResult ShowDialog(Window? owner, BaseDialogView dialogView, DialogOptions dialogOptions);

    ModalResult ShowDialog(Window? owner, DialogViewModel view, DialogOptions dialogOptions);

    ModalResult ShowDialog(
        Window? owner,
        string title,
        string message,
        PluginButtons pluginButtons = PluginButtons.OK | PluginButtons.Cancel,
        string? okButtonText = null,
        string? cancelButtonText = null,
        string? yesButtonText = null,
        string? noButtonText = null,
        DialogImage dialogType = DialogImage.None);

    bool TryGetDialogByViewModel(DialogViewModel view, out Window dialog);

    bool TryGetDialogByViewModelType(Type viewModelType, out Window dialog);
}