using ARSounds.UI.Common.ViewModels;
using ARSounds.UI.Services;
using CommunityToolkit.Mvvm.Input;
using System.Threading.Tasks;

namespace ARSounds.UI.Onboardings.ViewModels;

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
