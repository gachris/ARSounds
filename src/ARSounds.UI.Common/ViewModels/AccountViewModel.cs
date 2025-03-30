using ARSounds.Application.Commands;
using ARSounds.ApplicationFlow;
using ARSounds.Core.Auth.Events;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MediatR;

namespace ARSounds.UI.Common.ViewModels;

public partial class AccountViewModel : ObservableObject
{
    #region Fields/Conts

    private readonly IMediator _mediator;

    private bool _isAuthenticated;
    private bool _isBusy;

    #endregion

    #region Properties

    public bool IsAuthenticated
    {
        get => _isAuthenticated;
        private set
        {
            SetProperty(ref _isAuthenticated, value);

            SignInCommand.NotifyCanExecuteChanged();
            SignOutCommand.NotifyCanExecuteChanged();
        }
    }

    public bool IsBusy
    {
        get => _isBusy;
        private set
        {
            SetProperty(ref _isBusy, value);

            SignInCommand.NotifyCanExecuteChanged();
            SignOutCommand.NotifyCanExecuteChanged();
        }
    }

    #endregion

    public AccountViewModel(IMediator mediator, IApplicationEvents applicationEvents)
    {
        _mediator = mediator;

        applicationEvents.Register<SignedInEvent>(OnSignedIn);
        applicationEvents.Register<SignedOutEvent>(OnSignedOut);
        applicationEvents.Register<SignInStartedEvent>(OnSignInStarted);
        applicationEvents.Register<SignInFinishedEvent>(OnSignInFinished);
    }

    #region Methods

    private bool SignInCanExecute()
    {
        return !IsAuthenticated && !IsBusy;
    }

    private bool SignOutCanExecute()
    {
        return IsAuthenticated && !IsBusy;
    }

    #endregion

    #region Relay Commands

    [RelayCommand(CanExecute = nameof(SignInCanExecute))]
    private async Task SignInAsync()
    {
        var signInUserResultDto = await _mediator.Send(new SignInCommand());
    }

    [RelayCommand(CanExecute = nameof(SignOutCanExecute))]
    private async Task SignOutAsync()
    {
        var signInUserResultDto = await _mediator.Send(new SignOutCommand());
    }

    #endregion

    #region Events Subscriptions

    private void OnSignedIn(SignedInEvent obj)
    {
        IsAuthenticated = true;
    }

    private void OnSignedOut(SignedOutEvent obj)
    {
        IsAuthenticated = false;
    }

    private void OnSignInStarted(SignInStartedEvent obj)
    {
        IsBusy = true;
    }

    private void OnSignInFinished(SignInFinishedEvent obj)
    {
        IsBusy = false;
    }

    #endregion
}