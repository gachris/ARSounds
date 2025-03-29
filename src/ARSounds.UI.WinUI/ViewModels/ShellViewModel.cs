using ARSounds.UI.Common.ViewModels;
using ARSounds.UI.WinUI.Contracts;
using MediatR;

namespace ARSounds.UI.WinUI.ViewModels;

public partial class ShellViewModel : BaseShellViewModel
{
    #region Fields/Consts

    private readonly INavigationService _navigationService;

    private string _selectedViewItem = PageKeys.CameraPage;

    #endregion

    #region Properties

    public string SelectedViewItem
    {
        get => _selectedViewItem;
        set
        {
            SetProperty(ref _selectedViewItem, value);
            OnSelectedViewItemChanged();
        }
    }

    #endregion

    public ShellViewModel(IMediator mediator, INavigationService navigationService) : base(mediator)
    {
        _navigationService = navigationService;
    }

    #region Methods

    private void OnSelectedViewItemChanged()
    {
        _navigationService.NavigateTo(SelectedViewItem);
    }

    #endregion
}