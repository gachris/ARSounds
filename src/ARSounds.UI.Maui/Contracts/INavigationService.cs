namespace ARSounds.UI.Maui.Contracts;

public interface INavigationService
{
    Task PushAsync<TPage>() where TPage : Page;
    Task PushAsync<TPage>(object initParams) where TPage : Page;
    Task PushAsync(Type viewModelType);
    Task PushAsync(Type viewModelType, object initParams);

    Task PushModalAsync<TPage>() where TPage : Page;
    Task PushModalAsync<TPage>(object initParams) where TPage : Page;
    Task PushModalAsync(Type viewModelType);
    Task PushModalAsync(Type viewModelType, object initParams);

    void PushMain<TPage>() where TPage : Page;
    void PushMain(Type type);

    Task PopAsync();
    Task PopModalAsync();
}
