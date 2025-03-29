using ARSounds.Localization.Properties;
using ARSounds.UI.Maui.Services;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace ARSounds.UI.Maui.ViewModels;

public partial class WalkthroughViewModel : ViewModelBase
{
    #region Fields/Consts

    private readonly ObservableCollection<WalkthroughBoarding> _boardingsCollection = new();
    private readonly IReadOnlyCollection<WalkthroughBoarding> _boardingsReadOnlyCollection;
    private readonly INavigationService _navigationService;
    private bool _isSkipButtonVisible = true;
    private int _position = 0;

    #endregion

    #region Properties

    public IReadOnlyCollection<WalkthroughBoarding> Boardings => _boardingsReadOnlyCollection;

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

    public WalkthroughViewModel(INavigationService navigationService) : base(navigationService)
    {
        _navigationService = navigationService;
        _boardingsReadOnlyCollection = new ReadOnlyObservableCollection<WalkthroughBoarding>(_boardingsCollection);
    }

    #region RelayCommands

    [RelayCommand]
    private async Task Skip()
    {
        await _navigationService.PushMainAsync(typeof(AppShellViewModel));
    }

    [RelayCommand]
    private async Task Next()
    {
        if (!ValidateAndUpdatePosition()) return;
        await _navigationService.PushMainAsync(typeof(AppShellViewModel));
    }

    #endregion

    #region Methods

    public override Task InitializeAsync(object initParams)
    {
        CreateBoardingCollection();

        return Task.CompletedTask;
    }

    private bool ValidateAndUpdatePosition()
    {
        ValidateSelection(Position + 1);

        if (Position >= Boardings.Count - 1) return true;

        Position++;

        return false;
    }

    private void ValidateSelection(int index)
    {
        IsSkipButtonVisible = index <= Boardings.Count - 2;
    }

    private void CreateBoardingCollection()
    {
        _boardingsCollection.Add(new WalkthroughBoarding("walkthrough_01_image.jpg", Resources.StringWalkthroughTitleStep1, Resources.StringWalkthroughSubtitleStep1));
        _boardingsCollection.Add(new WalkthroughBoarding("walkthrough_02_image.jpg", Resources.StringWalkthroughTitleStep2, Resources.StringWalkthroughSubtitleStep2));
        _boardingsCollection.Add(new WalkthroughBoarding("walkthrough_03_image.jpg", Resources.StringWalkthroughTitleStep3, Resources.StringWalkthroughSubtitleStep3));
    }

    #endregion
}
