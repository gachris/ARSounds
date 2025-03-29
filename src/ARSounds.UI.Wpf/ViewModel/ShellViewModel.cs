using ARSounds.UI.Wpf.Contracts;
using ARSounds.UI.Wpf.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace ARSounds.UI.Wpf.ViewModels;

public partial class ShellViewModel : ObservableObject
{
    #region Fields/Consts

    private readonly INavigationService _navigationService;

    private bool _isSettingsOpen;

    #endregion

    #region Properties

    public bool IsSettingsOpen
    {
        get => _isSettingsOpen;
        private set => SetProperty(ref _isSettingsOpen, value);
    }

    #endregion

    public ShellViewModel(INavigationService navigationService)
    {
        _navigationService = navigationService;
    }

    #region Relay Commands

    [RelayCommand]
    private void OpenSettings()
    {
        IsSettingsOpen = !IsSettingsOpen;

        if (IsSettingsOpen)
        {
            _navigationService.NavigateTo(NavigationFrameKeys.Shell, typeof(SettingsPage));
        }
        else
        {
            _navigationService.GoBack(NavigationFrameKeys.Shell);
        }
    }

    #endregion
}