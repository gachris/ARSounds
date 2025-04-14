using System.Reflection;
using ARSounds.Localization.Properties;
using ARSounds.UI.Common.Contracts;
using ARSounds.UI.Common.Media;
using DevToolbox.Wpf.Media;

namespace ARSounds.UI.Wpf.Services;

public class AppUISettings : IAppUISettings
{
    #region Fields/Consts

    private const string SettingsKey = "AppBackgroundRequestedTheme";

    private readonly ILocalSettingsService _localSettingsService;

    #endregion

    #region Properties

    public Theme Theme { get; private set; }

    #endregion

    public AppUISettings(ILocalSettingsService localSettingsService)
    {
        _localSettingsService = localSettingsService;
    }

    #region Methods

    public async Task InitializeAsync()
    {
        FontSizeManager.TextScaleEnabled = true;

        var themeName = await _localSettingsService.ReadSettingAsync<string>(SettingsKey);

        if (!Enum.TryParse(themeName, out ElementTheme cacheTheme))
        {
            cacheTheme = ElementTheme.WindowsDefault;
        }

        Theme = ElementThemeToTheme(cacheTheme);
        ThemeManager.RequestedTheme = ThemeToElementTheme(Theme);
    }

    public async Task SetThemeAsync(Theme theme)
    {
        await _localSettingsService.SaveSettingAsync(SettingsKey, theme.ToString());

        Theme = theme;
        ThemeManager.RequestedTheme = ThemeToElementTheme(Theme);
    }

    public string GetVersionDescription()
    {
        var version = Assembly.GetExecutingAssembly().GetName().Version!;
        return $"{Resources.Application_title} - {version.Major}.{version.Minor}.{version.Build}.{version.Revision}";
    }

    private static Theme ElementThemeToTheme(ElementTheme elementTheme)
    {
        return elementTheme switch
        {
            ElementTheme.Default => Theme.Default,
            ElementTheme.WindowsDefault => Theme.Default,
            ElementTheme.Light => Theme.Light,
            ElementTheme.Dark => Theme.Dark,
            _ => throw new Exception(),
        };
    }

    private static ElementTheme ThemeToElementTheme(Theme theme)
    {
        return theme switch
        {
            Theme.Default => ElementTheme.WindowsDefault,
            Theme.Light => ElementTheme.Light,
            Theme.Dark => ElementTheme.Dark,
            _ => throw new Exception(),
        };
    }

    #endregion
}