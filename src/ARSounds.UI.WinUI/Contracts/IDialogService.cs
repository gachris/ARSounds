using ARSounds.UI.Common.Windows;

namespace ARSounds.UI.WinUI.Contracts;

public interface IDialogService
{
    Task<ModalResult> ShowDialogAsync(
        string title,
        string message,
        PluginButtons pluginButtons = PluginButtons.OK | PluginButtons.Cancel,
        string? okButtonText = null,
        string? cancelButtonText = null,
        DialogImage dialogType = DialogImage.None);

    Task ShowErrorAsync(Exception exception);

    object GetDialogByView(object view);
}
