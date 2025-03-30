using ARSounds.Application.Commands;
using CommunityToolkit.Mvvm.ComponentModel;
using MediatR;

namespace ARSounds.UI.Common.ViewModel;

public abstract class BaseShellViewModel : ObservableObject
{
    #region Fields/Consts

    private readonly IMediator _mediator;

    private bool _isSettingsOpen;

    #endregion

    #region Properties

    public bool IsSettingsOpen
    {
        get => _isSettingsOpen;
        protected set => SetProperty(ref _isSettingsOpen, value);
    }

    #endregion

    public BaseShellViewModel(IMediator mediator)
    {
        _mediator = mediator;
    }

    #region Methods

    public virtual async Task InitializeAsync()
    {
        await _mediator.Send(new SignInSilentCommand());
    }

    #endregion
}
