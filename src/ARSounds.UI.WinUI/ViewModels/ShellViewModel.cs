using ARSounds.Application.Commands;
using ARSounds.UI.WinUI.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using MediatR;

namespace ARSounds.UI.WinUI.ViewModels;

public partial class ShellViewModel : ObservableObject
{
    #region Fields/Consts

    private readonly IMediator _mediator;
    private readonly INavigationService _navigationService;

    private bool _isSettingsSelected;
    private string _selectedViewItem = PageKeys.CameraPage;

    #endregion

    #region Properties

    public bool IsSettingsSelected
    {
        get => _isSettingsSelected;
        private set => SetProperty(ref _isSettingsSelected, value);
    }

    public string SelectedViewItem
    {
        get => _selectedViewItem;
        private set
        {
            SetProperty(ref _selectedViewItem, value);
            OnSelectedViewItemChanged();
        }
    }

    #endregion

    public ShellViewModel(IMediator mediator, INavigationService navigationService)
    {
        _mediator = mediator;
        _navigationService = navigationService;
    }

    internal async Task InitializeAsync()
    {
        await _mediator.Send(new SignInSilentCommand());
    }

    #region Methods Overrides

    #endregion

    #region Methods

    private void OnSelectedViewItemChanged()
    {
        _navigationService.NavigateTo(SelectedViewItem);
    }

    #endregion

    #region Application Events

    #endregion
}
