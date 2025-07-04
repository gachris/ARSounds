﻿using ARSounds.UI.Common.Data;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DevToolbox.Core.Contracts;

namespace ARSounds.UI.Common.ViewModels;

public partial class BackgroundViewModel : ObservableObject, INavigationViewModelAware
{
    #region Fields/Consts

    private readonly INavigationService _navigationService;

    #endregion

    #region Properties

    public bool CanGoBack => true;

    #endregion

    public BackgroundViewModel(INavigationService navigationService)
    {
        _navigationService = navigationService;
    }

    #region Methods

    public virtual void OnNavigated(object? parameter = null)
    {
    }

    public virtual void OnNavigatedAway()
    {
    }

    #endregion

    #region Relay Commands

    [RelayCommand]
    private async Task TakeTour()
    {
        await _navigationService.NavigateToAsync(PageKeys.WalkthroughPage);
    }

    [RelayCommand]
    private async Task Skip()
    {
        await _navigationService.NavigateToAsync(PageKeys.CameraPage, clearNavigation: true);
    }

    #endregion
}