using System.Collections.ObjectModel;
using ARSounds.Localization.Properties;
using ARSounds.UI.Common.Windows;
using ARSounds.UI.Wpf.Windows.Commands;
using ARSounds.UI.Wpf.Windows.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace ARSounds.UI.Wpf.Windows.ViewModels;

public partial class BaseDialogWindowViewModel : ObservableObject, IDisposable
{
    #region Fields/Consts

    private readonly List<PluginButtonViewModel> _leftButtons = new();
    private readonly List<PluginButtonViewModel> _rightButtons = new();
    private readonly DialogWindow _widnow;

    private bool _disposedValue;

    [ObservableProperty]
    private string? _waitingAnimationTitle;

    [ObservableProperty]
    private string? _waitingAnimationMessage;

    [ObservableProperty]
    private bool _waitingAnimationBusy;

    #endregion

    #region Properties

    public bool IsFooterVisible { get; }

    public BaseDialogView View { get; }

    public ReadOnlyCollection<PluginButtonViewModel> LeftButtons { get; }

    public ReadOnlyCollection<PluginButtonViewModel> RightButtons { get; }

    #endregion

    public BaseDialogWindowViewModel(DialogWindow window, BaseDialogView view, DialogOptions dialogOptions)
    {
        _widnow = window;
        View = view;

        LeftButtons = new ReadOnlyCollection<PluginButtonViewModel>(_leftButtons);
        RightButtons = new ReadOnlyCollection<PluginButtonViewModel>(_rightButtons);

        SetupButtons(dialogOptions);

        IsFooterVisible = LeftButtons.Count != 0 || RightButtons.Count != 0;
    }

    private void SetupButtons(DialogOptions dialogOptions)
    {
        if (dialogOptions.PluginButtons?.Any() ?? false)
        {
            var leftButtons = dialogOptions.PluginButtons!.Where(x => x.ButtonPosition == PluginButtonPosition.Left).OrderBy(x => x.ButtonOrder).ToList();
            var rightButtons = dialogOptions.PluginButtons!.Where(x => x.ButtonPosition == PluginButtonPosition.Right).OrderBy(x => x.ButtonOrder).ToList();

            _leftButtons.AddRange(leftButtons.Select(GetButton));
            _rightButtons.AddRange(rightButtons.Select(GetButton));
        }
    }

    #region Methods

    private static PluginButtonViewModel GetButton(PluginButton pluginButton)
    {
        var content = pluginButton.Content is string text && !string.IsNullOrEmpty(text)
            ? pluginButton.Content
            : pluginButton.ButtonType switch
            {
                PluginButtonType.Yes => Resources.No,
                PluginButtonType.No => Resources.No,
                PluginButtonType.OK => Resources.OK,
                PluginButtonType.Cancel => Resources.Cancel,
                _ => throw new ArgumentOutOfRangeException(nameof(pluginButton.ButtonType)),
            };

        var command = pluginButton.ButtonType switch
        {
            PluginButtonType.Yes => DialogWindowCommands.Yes,
            PluginButtonType.No => DialogWindowCommands.No,
            PluginButtonType.OK => DialogWindowCommands.OK,
            PluginButtonType.Cancel => DialogWindowCommands.Cancel,
            _ => throw new ArgumentOutOfRangeException(nameof(pluginButton.ButtonType)),
        };

        var button = new PluginButtonViewModel(content, command, pluginButton.IsDefault, pluginButton.IsCancel);

        return button;
    }

    #endregion

    #region Relay Commands

    [RelayCommand]
    private void Yes(object e)
    {
        _widnow.ModalResult = ModalResult.Yes;
    }

    [RelayCommand]
    private void No(object e)
    {
        _widnow.ModalResult = ModalResult.No;
    }

    [RelayCommand]
    private void OK(object e)
    {
        _widnow.ModalResult = ModalResult.OK;
    }

    [RelayCommand]
    private void Cancel(object e)
    {
        _widnow.ModalResult = ModalResult.Cancel;
    }

    #endregion

    #region IDisposable Implementation

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposedValue)
        {
            if (disposing)
            {
                _leftButtons.Clear();
                _rightButtons.Clear();

                if (View is IDisposable disposable)
                {
                    disposable.Dispose();
                }
            }

            // TODO: free unmanaged resources (unmanaged objects) and override finalizer
            // TODO: set large fields to null
            _disposedValue = true;
        }
    }

    // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
    // ~DialogViewModel()
    // {
    //     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
    //     Dispose(disposing: false);
    // }

    public void Dispose()
    {
        // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }

    #endregion
}
