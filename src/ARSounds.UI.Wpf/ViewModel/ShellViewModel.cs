using ARSounds.UI.Wpf.Contracts;
using ARSounds.UI.Wpf.Views;
using CommunityToolkit.Mvvm.Input;
using MediatR;

namespace ARSounds.UI.Wpf.ViewModels;

public partial class ShellViewModel : Common.ViewModels.ShellViewModel
{
    #region Fields/Consts

    private readonly INavigationService _navigationService;

    #endregion

    public ShellViewModel(IMediator mediator, INavigationService navigationService) : base(mediator)
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