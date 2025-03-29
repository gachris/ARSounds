using ARSounds.UI.Maui.Services;
using CommunityToolkit.Mvvm.Input;

namespace ARSounds.UI.Maui.ViewModels;

public partial class BackgroundViewModel : ViewModelBase
{
    #region Fields/Consts

    private readonly INavigationService _navigationService;

    #endregion

    public BackgroundViewModel(INavigationService navigationService) : base(navigationService)
    {
        _navigationService = navigationService;
    }

    #region RelayCommands

    [RelayCommand]
    private async Task TakeTour()
    {
        await _navigationService.PushAsync<WalkthroughViewModel>();
    }

    [RelayCommand]
    private async Task Skip()
    {
        await _navigationService.PushMainAsync(typeof(AppShellViewModel));
    }

    #endregion
}
