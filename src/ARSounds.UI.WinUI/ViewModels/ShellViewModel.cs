using ARSounds.UI.WinUI.Contracts;
using MediatR;

namespace ARSounds.UI.WinUI.ViewModels;

public partial class ShellViewModel : Common.ViewModels.ShellViewModel
{
    #region Fields/Consts

    private readonly INavigationService _navigationService;

    #endregion

    public ShellViewModel(IMediator mediator, INavigationService navigationService) : base(mediator)
    {
        _navigationService = navigationService;
    }

    #region Methods

    protected override void OnSelectedViewItemChanged()
    {
        _navigationService.NavigateTo(SelectedViewItem);
    }

    #endregion
}