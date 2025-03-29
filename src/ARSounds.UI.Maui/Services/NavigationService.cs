using ARSounds.UI.Maui.ViewModels;

namespace ARSounds.UI.Maui.Services;

public class NavigationService : INavigationService
{
    #region Fields/Consts

    protected readonly IReadOnlyDictionary<Type, Type> _mappings;

    #endregion

    #region Properties

    private static INavigation? Navigation => Microsoft.Maui.Controls.Application.Current?.Windows[0].Page?.Navigation;

    #endregion

    public NavigationService(NavigationMapping navigationMapping)
    {
        _mappings = navigationMapping.Mappings;
    }

    #region INavigationService Implementation

    public Task PushAsync<TViewModel>() where TViewModel : ViewModelBase
    {
        return InternalPushAsync(typeof(TViewModel), null);
    }

    public Task PushAsync<TViewModel>(object initParams) where TViewModel : ViewModelBase
    {
        return InternalPushAsync(typeof(TViewModel), initParams);
    }

    public Task PushAsync(Type viewModelType)
    {
        return InternalPushAsync(viewModelType, null);
    }

    public Task PushAsync(Type viewModelType, object initParams)
    {
        return InternalPushAsync(viewModelType, initParams);
    }

    public Task PushModalAsync<TViewModel>() where TViewModel : ViewModelBase
    {
        throw new NotImplementedException();
    }

    public Task PushModalAsync<TViewModel>(object initParams) where TViewModel : ViewModelBase
    {
        throw new NotImplementedException();
    }

    public Task PushModalAsync(Type viewModelType, object initParams)
    {
        throw new NotImplementedException();
    }

    public Task PushModalAsync(Type viewModelType)
    {
        return InternalPushModalAsync(viewModelType, null);
    }

    public Task PushMainAsync(Type type)
    {
        return InternalPushMainAsync(type, null);
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

    protected Type GetPageTypeForViewModel(Type viewModelType)
    {
        return !_mappings.ContainsKey(viewModelType)
            ? throw new KeyNotFoundException($"No map for ${viewModelType} was found on navigation mappings")
            : _mappings[viewModelType];
    }

    protected Page? CreateAndBindPage(Type viewModelType)
    {
        var pageType = GetPageTypeForViewModel(viewModelType);
        return IPlatformApplication.Current?.Services.GetService(pageType) as Page;
    }

    protected virtual async Task InternalPushAsync(Type viewModelType, object? initParams)
    {
        var page = CreateAndBindPage(viewModelType);

        await (Navigation?.PushAsync(page) ?? Task.CompletedTask);

        if (page?.BindingContext is ViewModelBase modelBase)
        {
            await modelBase.InitializeAsync(initParams);
        }
    }

    protected virtual async Task InternalPushModalAsync(Type viewModelType, object? initParams)
    {
        var page = CreateAndBindPage(viewModelType);

        await (Navigation?.PushModalAsync(page) ?? Task.CompletedTask);

        if (page?.BindingContext is ViewModelBase modelBase)
        {
            await modelBase.InitializeAsync(initParams);
        }
    }

    protected virtual async Task InternalPushMainAsync(Type type, object? value)
    {
        var page = CreateAndBindPage(type);

        if (Microsoft.Maui.Controls.Application.Current?.Windows[0] is Window window)
        {
            window.Page = page;
        }

        if (page?.BindingContext is ViewModelBase modelBase)
        {
            await modelBase.InitializeAsync(value);
        }
    }

    #endregion
}