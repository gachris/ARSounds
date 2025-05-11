using System.Collections.ObjectModel;
using ARSounds.Localization.Properties;
using ARSounds.UI.Common.Data;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DevToolbox.Core.Contracts;

namespace ARSounds.UI.Common.ViewModels;

public partial class WalkthroughViewModel : ObservableObject, INavigationViewModelAware
{
    #region Fields/Consts

    private readonly ReadOnlyObservableCollection<WalkthroughBoarding> _boardingsReadOnlyCollection;
    private readonly INavigationService _navigationService;
    private bool _isSkipButtonVisible = true;
    private int _position = 0;

    #endregion

    #region Properties

    public bool CanGoBack => true;

    public ReadOnlyObservableCollection<WalkthroughBoarding> Boardings => _boardingsReadOnlyCollection;

    public bool IsSkipButtonVisible
    {
        get => _isSkipButtonVisible;
        private set => SetProperty(ref _isSkipButtonVisible, value);
    }

    public int Position
    {
        get => _position;
        private set => SetProperty(ref _position, value);
    }

    #endregion

    public WalkthroughViewModel(INavigationService navigationService)
    {
        _navigationService = navigationService;
        _boardingsReadOnlyCollection = new ReadOnlyObservableCollection<WalkthroughBoarding>
        (
            [
                new WalkthroughBoarding("walkthrough_01_image.jpg", Resources.Walkthrough_title_step_1, Resources.Walkthrough_subtitle_step_1),
                new WalkthroughBoarding("walkthrough_02_image.jpg", Resources.Walkthrough_title_step_2, Resources.Walkthrough_subtitle_step_2),
                new WalkthroughBoarding("walkthrough_03_image.jpg", Resources.Walkthrough_title_step_3, Resources.Walkthrough_subtitle_step_3),
            ]
        );
    }

    #region Methods

    public virtual void OnNavigated()
    {
    }

    public virtual void OnNavigatedAway()
    {
    }

    private bool ValidateAndUpdatePosition()
    {
        ValidateSelection(Position + 1);

        if (Position >= Boardings.Count - 1)
        {
            return true;
        }

        Position++;

        return false;
    }

    private void ValidateSelection(int index)
    {
        IsSkipButtonVisible = index <= Boardings.Count - 2;
    }

    #endregion

    #region Relay Commands

    [RelayCommand]
    private async Task Skip()
    {
        await _navigationService.NavigateToAsync(PageKeys.CameraPage, clearNavigation: true);
    }

    [RelayCommand]
    private async Task Next()
    {
        if (!ValidateAndUpdatePosition())
        {
            return;
        }

        await _navigationService.NavigateToAsync(PageKeys.CameraPage, clearNavigation: true);
    }

    #endregion
}