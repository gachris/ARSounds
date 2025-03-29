using ARSounds.UI.Maui.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace ARSounds.UI.Maui.ViewModels;

public partial class ViewModelBase : ObservableObject
{
    private readonly INavigationService _navigationService;

    [ObservableProperty]
    private bool _isBusy;

    [ObservableProperty]
    private string _loadingText;

    [ObservableProperty]
    private bool _dataLoaded;

    [ObservableProperty]
    private bool _isErrorState;

    [ObservableProperty]
    private string _errorMessage;

    [ObservableProperty]
    private string _errorImage;

    public ViewModelBase(INavigationService navigationService)
    {
        _navigationService = navigationService;
    }

    public virtual Task InitializeAsync(object initParams)
    {
        return Task.FromResult(false);
    }

    [RelayCommand]
    private async Task PopAsync()
    {
        await _navigationService.PopAsync();
    }

    [RelayCommand]
    private async Task PopModalAsync()
    {
        await _navigationService.PopModalAsync();
    }

    //Set Loading Indicators for Page
    protected void SetDataLoadingIndicators(bool isStarting = true)
    {
        if (isStarting)
        {
            IsBusy = true;
            DataLoaded = false;
            IsErrorState = false;
            ErrorMessage = "";
            ErrorImage = "";
        }
        else
        {
            LoadingText = "";
            IsBusy = false;
        }
    }
}