using ARSounds.Application.Commands;
using ARSounds.UI.Maui.Services;
using MediatR;

namespace ARSounds.UI.Maui.ViewModels;

public class AppShellViewModel : ViewModelBase
{
    private readonly IMediator _mediator;

    public AppShellViewModel(IMediator mediator, INavigationService navigationService) : base(navigationService)
    {
        _mediator = mediator;
    }

    public override async Task InitializeAsync(object? initParams)
    {
        await _mediator.Send(new SignInSilentCommand()).ConfigureAwait(false);
    }
}