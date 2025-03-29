using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using ARSounds.UI.Wpf.Contracts;
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
            if (navigationService.Content != pageInstance)
            {
                navigationService.Navigate(pageInstance);
            }
        }
    }

    #region Fields/Consts

    private readonly Dictionary<string, Frame> _frames = [];
    private readonly IServiceLocator _serviceLocator;
    private readonly Dictionary<Frame, FrameworkElement?> _lastViews = [];

    #endregion

    public NavigationService(IServiceLocator serviceLocator)
    {
        _serviceLocator = serviceLocator;
    }

    #region Methods

    public void RegisterFrame(string key, Frame frame)
    {
        if (!_frames.ContainsKey(key))
        {
            _frames[key] = frame;
            frame.Navigating += OnFrameNavigating;
            frame.Navigated += OnFrameNavigated;
        }
    }

    public void UnregisterFrame(string key)
    {
        if (_frames.TryGetValue(key, out var frame))
        {
            frame.Navigating -= OnFrameNavigating;
            frame.Navigated -= OnFrameNavigated;
            _frames.Remove(key);
        }
    }

    public void FrameNavigated(string key, Action<object, NavigationEventArgs> action)
    {
        if (_frames.TryGetValue(key, out var frame))
        {
            frame.Navigated += action.Invoke;
        }
    }

    public void AddBackEntry(string key, Type pageType)
    {
        if (!_frames.TryGetValue(key, out var frame))
        {
            throw new KeyNotFoundException($"No frame found with key: {key}");
        }

        frame.NavigationService?.AddBackEntry(new DIContentState(pageType, _serviceLocator));
    }

    public void NavigateTo(string key, Type pageType)
    {
        if (_frames.TryGetValue(key, out var frame) && pageType != null)
        {
            var pageInstance = _serviceLocator.GetService(pageType);
            if (pageInstance is not null)
            {
                frame.Navigate(pageInstance);
            }
        }
    }

    public void NavigateTo(string key, Type pageType, object parameter)
    {
        if (_frames.TryGetValue(key, out var frame) && pageType != null)
        {
            var pageInstance = _serviceLocator.GetService(pageType);
            if (pageInstance is not null)
            {
                frame.Navigate(pageInstance, parameter);
            }
        }
    }

    public void GoBack(string key)
    {
        if (_frames.TryGetValue(key, out var frame) && frame.CanGoBack)
        {
            frame.GoBack();
        }
    }

    #endregion

    #region Events Subscriptions

    private void OnFrameNavigating(object sender, NavigatingCancelEventArgs e)
    {
        if (sender is Frame frame && _lastViews.TryGetValue(frame, out var lastView) && lastView is FrameworkElement fe && fe.DataContext is IViewModelAware vm)
        {
            vm.OnNavigatedAway();
        }
    }

    private void OnFrameNavigated(object sender, NavigationEventArgs e)
    {
        if (sender is Frame frame)
        {
            if (e.Content is FrameworkElement fe && fe.DataContext is IViewModelAware vm)
            {
                vm.OnNavigated(e.ExtraData);
            }
            _lastViews[frame] = e.Content as FrameworkElement;
        }
    }

    #endregion
}