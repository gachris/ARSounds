using ARSounds.Application.Auth.Commands;
using ARSounds.ApplicationFlow;
using ARSounds.Core.Auth.Events;
using ARSounds.UI.Maui.Common.ViewModels;
using ARSounds.UI.Maui.Common.Views;
using ARSounds.UI.Maui.Helpers;
using ARSounds.UI.Maui.Onboardings.Views;
using ARSounds.UI.Maui.Services;
using CommunityToolkit.Mvvm.Input;
using MediatR;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Networking;
using System.Threading.Tasks;

namespace ARSounds.UI.Maui.User.ViewModels;

public partial class LoginViewModel : ViewModelBase
{
    #region Fields/Consts

    private readonly IMediator _mediator;
    private readonly INavigationService _navigationService;
    private readonly IConnectivity _connectivity;
    private bool _isUserLoggedIn;

    #endregion

    #region Properties

    public bool IsUserLoggedIn
    {
        get => _isUserLoggedIn;
        private set => SetProperty(ref _isUserLoggedIn, value);
    }

    #endregion

    public LoginViewModel(IMediator mediator, INavigationService navigationService, IConnectivity connectivity, IApplicationEvents applicationEvents) : base(navigationService)
    {
        _mediator = mediator;
        _navigationService = navigationService;
        _connectivity = connectivity;

        applicationEvents.Register<UserLoggedInEvent>(OnUserLoggedIn);
        applicationEvents.Register<UserLoggedOutEvent>(OnUserLoggedOut);
        applicationEvents.Register<SignInStartedEvent>(OnSignInStarted);
        applicationEvents.Register<SignInFinishedEvent>(OnSignInFinished);
    }

    #region Methods

    private bool SignInCanExecute() => !IsUserLoggedIn;

    private void OnUserLoggedIn(UserLoggedInEvent obj)
    {
        IsUserLoggedIn = true;

        if (AppSettings.IsFirstLaunching)
        {
            AppSettings.IsFirstLaunching = false;

            var backgroundPage = ServiceHelper.GetService<BackgroundPage>();
            Microsoft.Maui.Controls.Application.Current.MainPage = new NavigationPage(backgroundPage);
        }
        else
        {
            var appShell = ServiceHelper.GetService<AppShell>();
            Microsoft.Maui.Controls.Application.Current.MainPage = appShell;
        }
    }

    private void OnUserLoggedOut(UserLoggedOutEvent obj) => IsUserLoggedIn = false;

    private void OnSignInStarted(SignInStartedEvent obj)
    {
        LoadingText = "Loading...";
        IsBusy = true;
        SetDataLoadingIndicators(true);
    }

    private void OnSignInFinished(SignInFinishedEvent obj)
    {
        IsBusy = false;
        SetDataLoadingIndicators(false);
    }

    #endregion

    #region Relay Commands

    [RelayCommand(CanExecute = nameof(SignInCanExecute))]
    private async Task SignIn()
    {
        if (_connectivity.NetworkAccess is not NetworkAccess.Internet)
        {
            await Shell.Current.DisplayAlert("Internet Offline", "Check your internet and try again!", "OK");
            return;
        }

        var signInUserResultDto = await _mediator.Send(new SignInUserCommand());
        if (!signInUserResultDto.Success)
        {
            await Shell.Current.DisplayAlert("Error", signInUserResultDto.ErrorMessage, "OK");
        }
    }

    #endregion
}
