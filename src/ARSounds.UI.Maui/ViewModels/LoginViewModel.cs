using ARSounds.Application.Commands;
using ARSounds.ApplicationFlow;
using ARSounds.Core.Auth.Events;
using ARSounds.UI.Maui.Helpers;
using ARSounds.UI.Maui.Services;
using ARSounds.UI.Maui.Views;
using CommunityToolkit.Mvvm.Input;
using MediatR;

namespace ARSounds.UI.Maui.ViewModels;

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

        applicationEvents.Register<SignedInEvent>(OnUserLoggedIn);
        applicationEvents.Register<SignedOutEvent>(OnUserLoggedOut);
        applicationEvents.Register<SignInStartedEvent>(OnSignInStarted);
        applicationEvents.Register<SignInFinishedEvent>(OnSignInFinished);
    }

    #region Methods

    private bool SignInCanExecute() => !IsUserLoggedIn;

    private void OnUserLoggedIn(SignedInEvent obj)
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

    private void OnUserLoggedOut(SignedOutEvent obj) => IsUserLoggedIn = false;

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

        var signInUserResultDto = await _mediator.Send(new SignInCommand());
        if (!signInUserResultDto.IsSuccess)
        {
            await Shell.Current.DisplayAlert("Error", signInUserResultDto.Error, "OK");
        }
    }

    #endregion
}
