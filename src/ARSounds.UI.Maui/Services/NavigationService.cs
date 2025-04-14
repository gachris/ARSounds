using System.Diagnostics.CodeAnalysis;
using ARSounds.UI.Common.Contracts;
using ARSounds.UI.Maui.Views;
using CommonServiceLocator;

namespace ARSounds.UI.Maui.Services;

public class NavigationService : INavigationService
{
    #region Fields/Consts

    private readonly IPageService _pageService;
    private readonly IServiceLocator _serviceLocator;
    private Window? _frame;

    #endregion

    #region Properties

    public object? Frame
    {
        get => _frame;
        set => _frame = value as Window;
    }

    [MemberNotNullWhen(true, nameof(Frame), nameof(_frame))]
    public bool CanGoBack => _frame?.Page?.Navigation.NavigationStack.Count > 1;

    #endregion

    public NavigationService(IPageService pageService, IServiceLocator serviceLocator)
    {
        _pageService = pageService;
        _serviceLocator = serviceLocator;
    }

    #region Methods

    public Task<bool> AddBackEntryAsync(string pageKey)
    {
        throw new NotImplementedException("MAUI doesn't support manually pushing back entries without navigation.");
    }

    public async Task<bool> GoBackAsync()
    {
        if (!CanGoBack || _frame.Page is null)
            return false;

        await _frame.Page.Navigation.PopAsync();

        return true;
    }

    public async Task<bool> NavigateToAsync(string pageKey, object? parameter = null, bool clearNavigation = false)
    {
        if (_frame == null)
            return false;

        var pageType = _pageService.GetPageType(pageKey);
        if (_serviceLocator.GetService(pageType) is not Page pageInstance)
            return false;

        if (clearNavigation)
        {
            _frame.Page = ServiceLocator.Current.GetInstance<AppShell>();
        }
        else if (_frame.Page != null)
        {
            await _frame.Page.Navigation.PushAsync(pageInstance);
        }
        else
        {
            _frame.Page = ServiceLocator.Current.GetInstance<AppShell>();
        }

        return true;
    }

    #endregion
}