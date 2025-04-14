using System.Diagnostics.CodeAnalysis;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using ARSounds.UI.Common.Contracts;
using CommonServiceLocator;

namespace ARSounds.UI.Wpf.Services;

public class NavigationService : INavigationService
{
    [Serializable]
    private class DIContentState : CustomContentState
    {
        private readonly Type _pageType;
        private readonly IServiceLocator _serviceLocator;

        public DIContentState(Type pageType, IServiceLocator serviceLocator)
        {
            _pageType = pageType;
            _serviceLocator = serviceLocator;
        }

        public override void Replay(System.Windows.Navigation.NavigationService navigationService, NavigationMode mode)
        {
            var pageInstance = _serviceLocator.GetService(_pageType);
            if (pageInstance is not null && navigationService.Content != pageInstance)
            {
                navigationService.Navigate(pageInstance);
            }
        }
    }

    #region Fields/Consts

    private readonly IPageService _pageService;
    private readonly IServiceLocator _serviceLocator;
    private readonly List<FrameworkElement> _lastViews = [];
    private Frame? _frame;

    public event NavigatedEventHandler? Navigated;

    #endregion

    #region Properties

    public object? Frame
    {
        get => _frame;
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

    public NavigationService(IPageService pageService, IServiceLocator serviceLocator)
    {
        _pageService = pageService;
        _serviceLocator = serviceLocator;
    }

    #region Methods

    private void RegisterFrameEvents()
    {
        if (_frame is not null)
        {
            _frame.Navigating -= Frame_Navigating;
            _frame.Navigated -= Frame_Navigated;
        }
    }

    private void UnregisterFrameEvents()
    {
        if (_frame is not null)
        {
            _frame.Navigating += Frame_Navigating;
            _frame.Navigated += Frame_Navigated;
        }
    }

    public async Task<bool> AddBackEntryAsync(string pageKey)
    {
        if (_frame?.NavigationService != null)
        {
            var pageType = _pageService.GetPageType(pageKey);

            _frame.NavigationService.AddBackEntry(new DIContentState(pageType, _serviceLocator));

            return await Task.FromResult(true);
        }

        return await Task.FromResult(false);
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
        if (_frame is null)
            return await Task.FromResult(false);

        var pageType = _pageService.GetPageType(pageKey);

        var pageInstance = _serviceLocator.GetService(pageType);
        if (pageInstance is null)
            return await Task.FromResult(false);

        if (clearNavigation && _frame.NavigationService is not null)
        {
            while (_frame.CanGoBack)
                _frame.RemoveBackEntry();
        }

        return await Task.FromResult(_frame.Navigate(pageInstance, parameter));
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