using ARSounds.Application.Commands;
using ARSounds.ApplicationFlow;
using ARSounds.Core.ClaimsPrincipal;
using ARSounds.Core.ClaimsPrincipal.Events;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MediatR;

namespace ARSounds.UI.Common.ViewModels;

public partial class AccountViewModel : ObservableObject
{
    #region Fields/Conts

    private readonly IMediator _mediator;
    private readonly IClaimsPrincipalState _claimsPrincipalState;

    private string? _initials;
    private string? _name;
    private string? _email;
    private bool _isAuthenticated;
    private bool _isBusy;

    #endregion

    #region Properties

    public string? Initials
    {
        get => _initials;
        private set => SetProperty(ref _initials, value);
    }

    public string? Name
    {
        get => _name;
        private set => SetProperty(ref _name, value);
    }

    public string? Email
    {
        get => _email;
        private set => SetProperty(ref _email, value);
    }

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

    public AccountViewModel(IMediator mediator, IClaimsPrincipalState claimsPrincipalState, IApplicationEvents applicationEvents)
    {
        _mediator = mediator;
        _claimsPrincipalState = claimsPrincipalState;

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

    public static string GetInitials(string? fullName)
    {
        if (string.IsNullOrWhiteSpace(fullName))
            return string.Empty;

        return string.Join("", fullName
            .Split(' ', StringSplitOptions.RemoveEmptyEntries)
            .Where(word => !string.IsNullOrWhiteSpace(word))
            .Select(word => char.ToUpperInvariant(word[0])));
    }

    #endregion

    #region Relay Commands

    [RelayCommand(CanExecute = nameof(SignInCanExecute))]
    private async Task SignInAsync()
    {
        await _mediator.Send(new SignInCommand());
    }

    [RelayCommand(CanExecute = nameof(SignOutCanExecute))]
    private async Task SignOutAsync()
    {
        await _mediator.Send(new SignOutCommand());
    }

    #endregion

    #region Events Subscriptions

    private void OnSignedIn(SignedInEvent obj)
    {
        IsAuthenticated = true;

        Initials = GetInitials(_claimsPrincipalState.UserClaims?.Name);
        Name = _claimsPrincipalState.UserClaims?.Name;
        Email = _claimsPrincipalState.UserClaims?.Email;
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