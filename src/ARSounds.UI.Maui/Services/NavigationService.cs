using ARSounds.UI.Maui.Contracts;

namespace ARSounds.UI.Maui.Services;

public class NavigationService : INavigationService
{
    #region Properties

    private static INavigation? Navigation => Microsoft.Maui.Controls.Application.Current?.Windows[0].Page?.Navigation;

    #endregion

    public NavigationService()
    {
    }

    #region INavigationService Implementation

    public Task PushAsync<TPage>() where TPage : Page
    {
        return InternalPushAsync(typeof(TPage), null);
    }

    public Task PushAsync<TPage>(object initParams) where TPage : Page
    {
        return InternalPushAsync(typeof(TPage), initParams);
    }

    public Task PushAsync(Type viewModelType)
    {
        return InternalPushAsync(viewModelType, null);
    }

    public Task PushAsync(Type viewModelType, object initParams)
    {
        return InternalPushAsync(viewModelType, initParams);
    }

    public Task PushModalAsync<TPage>() where TPage : Page
    {
        return InternalPushModalAsync(typeof(TPage), null);
    }

    public Task PushModalAsync<TPage>(object initParams) where TPage : Page
    {
        return InternalPushModalAsync(typeof(TPage), initParams);
    }

    public Task PushModalAsync(Type viewModelType, object initParams)
    {
        return InternalPushModalAsync(viewModelType, initParams);
    }

    public Task PushModalAsync(Type viewModelType)
    {
        return InternalPushModalAsync(viewModelType, null);
    }

    public void PushMain<TPage>() where TPage : Page
    {
        InternalPushMain(typeof(TPage), null);
    }

    public void PushMain(Type type)
    {
        InternalPushMain(type, null);
    }

    public async Task PopAsync()
    {
        await (Navigation?.PopAsync() ?? Task.CompletedTask);
    }

    public async Task PopModalAsync()
    {
        await (Navigation?.PopModalAsync() ?? Task.CompletedTask);
    }

    #endregion

    #region Methods

    protected virtual async Task InternalPushAsync(Type pageType, object? initParams)
    {
        var page = IPlatformApplication.Current?.Services.GetService(pageType) as Page;

        await (Navigation?.PushAsync(page) ?? Task.CompletedTask);
    }

    protected virtual async Task InternalPushModalAsync(Type pageType, object? initParams)
    {
        var page = IPlatformApplication.Current?.Services.GetService(pageType) as Page;

        await (Navigation?.PushModalAsync(page) ?? Task.CompletedTask);
    }

    protected virtual void InternalPushMain(Type pageType, object? value)
    {
        var page = IPlatformApplication.Current?.Services.GetService(pageType) as Page;

        if (Microsoft.Maui.Controls.Application.Current?.Windows[0] is Window window)
        {
            window.Page = page;
        }
    }

    #endregion
}