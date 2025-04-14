using System.Diagnostics.CodeAnalysis;
using ARSounds.UI.Common.Contracts;
using ARSounds.UI.WinUI.Contracts;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;

namespace ARSounds.UI.WinUI.Services;

public class NavigationService : INavigationService
{
    #region Fields/Consts

    private readonly IPageService _pageService;
    private readonly IAppWindowService _appWindowService;
    private readonly List<FrameworkElement> _lastViews = [];
    private Frame? _frame;

    public event NavigatedEventHandler? Navigated;

    #endregion

    #region Properties

    public object? Frame
    {
        get
        {
            if (_frame == null)
            {
                _frame = _appWindowService.MainWindow.Content as Frame;
                RegisterFrameEvents();
            }

            return _frame;
        }
        set
        {
            UnregisterFrameEvents();
            _frame = value as Frame;
            RegisterFrameEvents();
        }
    }

    [MemberNotNullWhen(true, nameof(Frame), nameof(_frame))]
    public bool CanGoBack => _frame?.CanGoBack == true;

    #endregion

    public NavigationService(IPageService pageService, IAppWindowService appWindowService)
    {
        _pageService = pageService;
        _appWindowService = appWindowService;
    }

    #region Methods

    private void RegisterFrameEvents()
    {
        if (_frame != null)
        {
            _frame.Navigating += Frame_Navigating;
            _frame.Navigated += Frame_Navigated;
        }
    }

    private void UnregisterFrameEvents()
    {
        if (_frame != null)
        {
            _frame.Navigating -= Frame_Navigating;
            _frame.Navigated -= Frame_Navigated;
        }
    }

    public async Task<bool> AddBackEntryAsync(string pageKey)
    {
        if (_frame is null)
            return await Task.FromResult(false);

        var pageType = _pageService.GetPageType(pageKey);

        var entry = new PageStackEntry(pageType, null, null);

        _frame.BackStack.Add(entry);
        return await Task.FromResult(true);
    }

    public async Task<bool> GoBackAsync()
    {
        if (!CanGoBack)
            return await Task.FromResult(false);

        _frame.GoBack();

        return await Task.FromResult(true);
    }

    public async Task<bool> NavigateToAsync(string pageKey, object? parameter = null, bool clearNavigation = false)
    {
        var pageType = _pageService.GetPageType(pageKey);

        if (_frame != null && _frame.Content?.GetType() != pageType)
        {
            _frame.Tag = clearNavigation;
            return await Task.FromResult(_frame.Navigate(pageType, parameter));
        }

        return await Task.FromResult(true);
    }

    #endregion

    #region Events Subscriptions

    private void Frame_Navigating(object sender, NavigatingCancelEventArgs e)
    {
        var lastView = _lastViews.LastOrDefault();
        if (lastView?.DataContext is IViewModelAware vm)
        {
            vm.OnNavigatedAway();
        }

        if (lastView is not null)
        {
            _lastViews.Remove(lastView);
        }
    }

    private void Frame_Navigated(object sender, NavigationEventArgs e)
    {
        if (_frame is null)
            return;

        var clearNavigation = (bool)_frame.Tag;
        if (clearNavigation)
        {
            _frame.BackStack.Clear();
        }

        if (e.Content is FrameworkElement fe)
        {
            if (fe.DataContext is IViewModelAware vm)
            {
                vm.OnNavigated();
            }

            _lastViews.Add(fe);
        }

        Navigated?.Invoke(sender, e);
    }

    #endregion
}
