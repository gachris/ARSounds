using ARSounds.UI.Common;
using ARSounds.UI.WinUI.Contracts;
using ARSounds.UI.WinUI.Views;
using Microsoft.UI.Xaml.Controls;

namespace ARSounds.UI.WinUI.Services;

public class PageService : IPageService
{
    #region Fields/Consts

    private readonly Dictionary<string, Type> _pages = [];

    #endregion

    public PageService()
    {
        Configure<ARCameraPage>(PageKeys.CameraPage);
        Configure<SettingsPage>(PageKeys.SettingsPage);
    }

    #region IPageService Implementation

    public Type GetPageType(string key)
    {
        Type? pageType;
        lock (_pages)
        {
            if (!_pages.TryGetValue(key, out pageType))
            {
                throw new ArgumentException($"Page not found: {key}. Did you forget to call PageService.Configure?");
            }
        }

        return pageType;
    }

    #endregion

    #region Methods

    private void Configure<V>(string key) where V : Page
    {
        lock (_pages)
        {
            if (_pages.ContainsKey(key))
            {
                throw new ArgumentException($"The key {key} is already configured in PageService");
            }

            var type = typeof(V);
            if (_pages.ContainsValue(type))
            {
                throw new ArgumentException($"This type is already configured with key {_pages.First(p => p.Value == type).Key}");
            }

            _pages.Add(key, type);
        }
    }

    #endregion
}