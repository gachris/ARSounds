using ARSounds.Application.Commands;
using ARSounds.UI.Common.Data;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DevToolbox.Core.Contracts;
using MediatR;

namespace ARSounds.UI.Common.ViewModels;

public partial class ShellViewModel : ObservableObject
{
    #region Fields/Consts

    private readonly IMediator _mediator;
    private readonly INavigationService _navigationService;

    private bool _isSettingsOpen;
    private string _selectedViewItem = PageKeys.CameraPage;

    #endregion

    #region Properties

    public bool IsSettingsOpen
    {
        get => _isSettingsOpen;
        protected set => SetProperty(ref _isSettingsOpen, value);
    }

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

    public ShellViewModel(IMediator mediator, INavigationService navigationService)
    {
        _mediator = mediator;
        _navigationService = navigationService;
    }

    #region Methods

    public virtual async Task InitializeAsync()
    {
        await _mediator.Send(new SignInSilentCommand());
    }

    protected virtual async void OnSelectedViewItemChanged()
    {
        await _navigationService.NavigateToAsync(SelectedViewItem);
    }

    #endregion

    #region Relay Commands

    [RelayCommand]
    private async Task ToggleSettings()
    {
        IsSettingsOpen = !IsSettingsOpen;

        if (IsSettingsOpen)
        {
            await _navigationService.NavigateToAsync(PageKeys.SettingsPage);
        }
        else
        {
            await _navigationService.GoBackAsync();
        }
    }

    #endregion
}
