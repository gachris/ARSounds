using ARSounds.UI.Common.Contracts;
using ARSounds.UI.Common.Data;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace ARSounds.UI.Common.ViewModels;

public partial class BackgroundViewModel : ObservableObject, IViewModelAware
{
    #region Fields/Consts

    private readonly INavigationService _navigationService;

    #endregion

    public BackgroundViewModel(INavigationService navigationService)
    {
        _navigationService = navigationService;
    }

    #region Methods

    public virtual void OnNavigated()
    {
    }

    public virtual void OnNavigatedAway()
    {
    }

    #endregion

    #region Relay Commands

    [RelayCommand]
    private async Task TakeTour()
    {
        await _navigationService.NavigateToAsync(PageKeys.WalkthroughPage);
    }

    [RelayCommand]
    private async Task Skip()
    {
        await _navigationService.NavigateToAsync(PageKeys.CameraPage, clearNavigation: true);
    }

    #endregion
}