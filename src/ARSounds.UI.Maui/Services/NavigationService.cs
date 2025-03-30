using System.Diagnostics.CodeAnalysis;
using ARSounds.UI.Common.Contracts;
using CommonServiceLocator;
using MauiApplication = Microsoft.Maui.Controls.Application;

namespace ARSounds.UI.Maui.Services;

public class NavigationService : INavigationService
{
    #region Fields/Consts

    private readonly IPageService _pageService;
    private readonly IServiceLocator _serviceLocator;
    private readonly List<Page> _lastViews = [];
    private Window? _window;

    #endregion

    #region Properties

    public object? Frame
    {
        get => _window ??= MauiApplication.Current?.Windows[0];
        set => _window = value as Window;
    }

    [MemberNotNullWhen(true, nameof(Frame), nameof(_window))]
    public bool CanGoBack => (_window?.Page as NavigationPage)?.Navigation.NavigationStack?.Count > 1;

    #endregion

    public NavigationService(IPageService pageService, IServiceLocator serviceLocator)
    {
        _pageService = pageService;
        _serviceLocator = serviceLocator;
    }

    #region Methods

    public Task<bool> AddBackEntryAsync(string pageKey)
    {
        // MAUI doesn't support manually pushing back entries without navigation
        return Task.FromResult(false);
    }

    public async Task<bool> GoBackAsync()
    {
        if (_window?.Page is not NavigationPage navPage || !CanGoBack)
            return false;

        var currentPage = navPage.CurrentPage;

        if (currentPage.BindingContext is IViewModelAware vm)
        {
            vm.OnNavigatedAway();
        }

        _lastViews.Remove(currentPage);

        await navPage.Navigation.PopAsync();

        return true;
    }

    public async Task<bool> NavigateToAsync(string pageKey, object? parameter = null, bool clearNavigation = false)
    {
        if (_window == null)
            return false;

        var pageType = _pageService.GetPageType(pageKey);
        if (_serviceLocator.GetService(pageType) is not Page pageInstance)
            return false;

        if (pageInstance.BindingContext is IViewModelAware vm)
        {
            vm.OnNavigated();
        }

        var navPage = _window.Page as NavigationPage;

        if (clearNavigation)
        {
            navPage = new NavigationPage(pageInstance);
            _window.Page = navPage;
        }
        else if (navPage != null)
        {
            await navPage.Navigation.PushAsync(pageInstance);
        }
        else
        {
            _window.Page = new NavigationPage(pageInstance);
        }

        _lastViews.Add(pageInstance);
        return true;
    }

    #endregion
}