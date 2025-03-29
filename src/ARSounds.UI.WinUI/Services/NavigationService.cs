using System.Diagnostics.CodeAnalysis;
using ARSounds.UI.Common.Contracts;
using ARSounds.UI.WinUI.Contracts;
using ARSounds.UI.WinUI.Helpers;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;

namespace ARSounds.UI.WinUI.Services;

public class NavigationService : INavigationService
{
    #region Fields/Consts

    private readonly IAppWindowService _appWindowService;
    private readonly IPageService _pageService;
    private object? _lastParameterUsed;
    private Frame? _frame;

    public event NavigatedEventHandler? Navigated;

    #endregion

    #region Properties

    public Frame? Frame
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
            _frame = value;
            RegisterFrameEvents();
        }
    }

    [MemberNotNullWhen(true, nameof(Frame), nameof(_frame))]
    public bool CanGoBack => Frame != null && Frame.CanGoBack;

    #endregion

    public NavigationService(IPageService pageService, IAppWindowService appWindowService)
    {
        _pageService = pageService;
        _appWindowService = appWindowService;
    }

    #region INavigationService Methods Implementation

    public bool GoBack()
    {
        if (CanGoBack)
        {
            var vmBeforeNavigation = _frame.GetPageViewModel();
            _frame.GoBack();
            if (vmBeforeNavigation is IViewModelAware navigationAware)
            {
                navigationAware.OnNavigatedAway();
            }

            return true;
        }

        return false;
    }

    public bool NavigateTo(string pageKey, object? parameter = null, bool clearNavigation = false)
    {
        var pageType = _pageService.GetPageType(pageKey);

        if (_frame != null && (_frame.Content?.GetType() != pageType || parameter != null && !parameter.Equals(_lastParameterUsed)))
        {
            _frame.Tag = clearNavigation;
            var vmBeforeNavigation = _frame.GetPageViewModel();
            var navigated = _frame.Navigate(pageType, parameter);
            if (navigated)
            {
                _lastParameterUsed = parameter;
                if (vmBeforeNavigation is IViewModelAware navigationAware)
                {
                    navigationAware.OnNavigated(parameter);
                }
            }

            return navigated;
        }

        return false;
    }

    #endregion

    #region Methods

    private void RegisterFrameEvents()
    {
        if (_frame != null)
        {
            _frame.Navigated += OnNavigated;
        }
    }

    private void UnregisterFrameEvents()
    {
        if (_frame != null)
        {
            _frame.Navigated -= OnNavigated;
        }
    }

    #endregion

    #region Events Subscriptions

    private void OnNavigated(object sender, NavigationEventArgs e)
    {
        if (sender is Frame frame)
        {
            var clearNavigation = (bool)frame.Tag;
            if (clearNavigation)
            {
                frame.BackStack.Clear();
            }

            if (frame.GetPageViewModel() is IViewModelAware navigationAware)
            {
                navigationAware.OnNavigated(e.Parameter);
            }

            Navigated?.Invoke(sender, e);
        }
    }

    #endregion
}
