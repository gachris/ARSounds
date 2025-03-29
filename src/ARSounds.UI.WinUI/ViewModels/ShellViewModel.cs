using CommunityToolkit.Mvvm.ComponentModel;
using ARSounds.UI.WinUI.Services;
using MediatR;

namespace ARSounds.UI.WinUI.ViewModels;

public partial class ShellViewModel : ObservableObject
{
    #region Fields/Consts

    private readonly IMediator _mediator;
    private readonly INavigationService _navigationService;

    [ObservableProperty]
    private string _selectedViewItem = PageKeys.CameraPage;

    private bool _isSettingsSelected;

    #endregion

    #region Properties

    public bool IsSettingsSelected
    {
        get => _isSettingsSelected;
        protected set => SetProperty(ref _isSettingsSelected, value);
    }

    #endregion

    public ShellViewModel(IMediator mediator, INavigationService navigationService)
    {
        _mediator = mediator;
        _navigationService = navigationService;
    }

    internal async Task InitializeAsync()
    {
        //await _mediator.Send(new SignInSilentCommand());
    }

    #region Methods Overrides

    #endregion

    #region Partial Methods

    partial void OnSelectedViewItemChanged(string? oldValue, string newValue)
    {
        _navigationService.NavigateTo(newValue);
    }

    #endregion

    #region Application Events

    #endregion
}
