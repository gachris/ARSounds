using ARSounds.UI.Maui.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace ARSounds.UI.Maui.ViewModels;

public partial class ViewModelBase : ObservableObject
{
    private readonly INavigationService _navigationService;

    private bool _isBusy;
    private string? _loadingText;
    private bool _dataLoaded;
    private bool _isErrorState;
    private string? _errorMessage;
    private string? _errorImage;

    public bool IsBusy
    {
        get => _isBusy;
        set => SetProperty(ref _isBusy, value);
    }

    public string? LoadingText
    {
        get => _loadingText;
        set => SetProperty(ref _loadingText, value);
    }

    public bool DataLoaded
    {
        get => _dataLoaded;
        set => SetProperty(ref _dataLoaded, value);
    }

    public bool IsErrorState
    {
        get => _isErrorState;
        set => SetProperty(ref _isErrorState, value);
    }

    public string? ErrorMessage
    {
        get => _errorMessage;
        set => SetProperty(ref _errorMessage, value);
    }

    public string? ErrorImage
    {
        get => _errorImage;
        set => SetProperty(ref _errorImage, value);
    }

    public ViewModelBase(INavigationService navigationService)
    {
        _navigationService = navigationService;
    }

    public virtual Task InitializeAsync(object? initParams)
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