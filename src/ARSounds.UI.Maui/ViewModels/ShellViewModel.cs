using ARSounds.UI.Common.Contracts;
using ARSounds.UI.Common.ViewModel;
using MediatR;

namespace ARSounds.UI.Maui.ViewModels;

public class ShellViewModel : BaseShellViewModel, IViewModelAware
{
    public ShellViewModel(IMediator mediator) : base(mediator)
    {
    }

    #region Methods

    public void OnNavigated()
    {
    }

    public void OnNavigatedAway()
    {
    }

    #endregion
}