using ARSounds.UI.Common.Contracts;

namespace ARSounds.UI.Common.Services;

public class PageService : IPageService
{
    #region Fields/Consts

    private readonly Dictionary<string, Type> _pages = [];

    #endregion

    #region Methods

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

    public void Configure(string key, Type type)
    {
        lock (_pages)
        {
            if (_pages.ContainsKey(key))
            {
                throw new ArgumentException($"The key {key} is already configured in PageService");
            }

            if (_pages.ContainsValue(type))
            {
                throw new ArgumentException($"This type is already configured with key {_pages.First(p => p.Value == type).Key}");
            }

            _pages.Add(key, type);
        }
    }

    #endregion
}