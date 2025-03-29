using ARSounds.Application.Commands;
using ARSounds.UI.Maui.Services;
using CommunityToolkit.Mvvm.Input;
using MediatR;

namespace ARSounds.UI.Maui.ViewModels;

public partial class ProfileViewModel : ViewModelBase
{
    #region Fields/Consts

    private readonly IMediator _mediator;
    private readonly INavigationService _navigationService;
    private readonly IConnectivity _connectivity;

    #endregion

    #region Properties

    public string Name { get; set; } = "Nura Lineon";

    public string Email { get; set; } = "nr-lineon@maui.com";

    public string ImageUrl { get; set; } = "user2.png";

    #endregion

    public ProfileViewModel(IMediator mediator, INavigationService navigationService, IConnectivity connectivity) : base(navigationService)
    {
        _mediator = mediator;
        _navigationService = navigationService;
        _connectivity = connectivity;
    }

    public override async Task InitializeAsync(object? initParams)
    {
        await Task.CompletedTask;
    }

    #region Methods

    [RelayCommand]
    private async Task SignOut()
    {
        if (_connectivity.NetworkAccess is not NetworkAccess.Internet)
        {
            await Shell.Current.DisplayAlert("Internet Offline", "Check your internet and try again!", "OK");
            return;
        }

        var signInUserResultDto = await _mediator.Send(new SignOutCommand());
        if (!signInUserResultDto.IsSuccess)
        {
            await Shell.Current.DisplayAlert("Error", signInUserResultDto.Error, "OK");
        }
        else
        {
            await _navigationService.PushMainAsync(typeof(AppShellViewModel));
        }
    }

    #endregion
}
