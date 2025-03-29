using ARSounds.UI.Maui.ViewModels;

namespace ARSounds.UI.Maui.Services;

public interface INavigationService
{
    Task PushAsync<TViewModel>() where TViewModel : ViewModelBase;
    Task PushAsync<TViewModel>(object initParams) where TViewModel : ViewModelBase;
    Task PushAsync(Type viewModelType);
    Task PushAsync(Type viewModelType, object initParams);

    Task PushModalAsync<TViewModel>() where TViewModel : ViewModelBase;
    Task PushModalAsync<TViewModel>(object initParams) where TViewModel : ViewModelBase;
    Task PushModalAsync(Type viewModelType);
    Task PushModalAsync(Type viewModelType, object initParams);

    Task PushMainAsync(Type type);

    Task PopAsync();
    Task PopModalAsync();
}
