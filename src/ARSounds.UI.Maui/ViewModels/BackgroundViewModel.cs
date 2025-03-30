using ARSounds.UI.Common.Contracts;
using ARSounds.UI.Maui.Contracts;
using ARSounds.UI.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace ARSounds.UI.Maui.ViewModels;

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

    public void OnNavigated()
    {
    }

    public void OnNavigatedAway()
    {
    }

    #endregion

    #region Relay Commands

    [RelayCommand]
    private async Task TakeTour()
    {
        await _navigationService.PushAsync<WalkthroughPage>();
    }

    [RelayCommand]
    private void Skip()
    {
        _navigationService.PushMain<AppShell>();
    }

    #endregion
}
