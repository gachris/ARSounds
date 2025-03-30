using System.Reflection;
using ARSounds.UI.Common.Contracts;
using ARSounds.UI.Common.Helpers;
using ARSounds.UI.Common.Media;
using ARSounds.UI.WinUI.Contracts;
using ARSounds.UI.WinUI.Helpers;
using Microsoft.UI.Xaml;
using Windows.ApplicationModel;

namespace ARSounds.UI.WinUI.Services;

public class AppUISettings : IAppUISettings
{
    #region Fields/Consts

    private const string SettingsKey = "AppBackgroundRequestedTheme";

    private readonly ILocalSettingsService _localSettingsService;
    private readonly IThemeSelectorService _themeSelectorService;

    #endregion

    #region Properties

    public Theme Theme { get; private set; }

    #endregion

    public AppUISettings(ILocalSettingsService localSettingsService, IThemeSelectorService themeSelectorService)
    {
        _themeSelectorService = themeSelectorService;
        _localSettingsService = localSettingsService;
    }

    #region Methods

    public async Task InitializeAsync()
    {
        var themeName = await _localSettingsService.ReadSettingAsync<string>(SettingsKey);

        if (!Enum.TryParse(themeName, out ElementTheme cacheTheme))
        {
            cacheTheme = ElementTheme.Default;
        }

        Theme = ElementThemeToTheme(cacheTheme);

        await _themeSelectorService.SetThemeAsync(ThemeToElementTheme(Theme));
    }

    public async Task SetThemeAsync(Theme theme)
    {
        await _localSettingsService.SaveSettingAsync(SettingsKey, theme.ToString());

        Theme = theme;

        await _themeSelectorService.SetThemeAsync(ThemeToElementTheme(Theme));
    }

    public string GetVersionDescription()
    {
        Version version;

        if (RuntimeHelper.IsMSIX)
        {
            var packageVersion = Package.Current.Id.Version;
            version = new(packageVersion.Major, packageVersion.Minor, packageVersion.Build, packageVersion.Revision);
        }
        else
        {
            version = Assembly.GetExecutingAssembly().GetName().Version!;
        }

        return $"{"AppDisplayName".GetLocalized()} - {version.Major}.{version.Minor}.{version.Build}.{version.Revision}";
    }

    private static Theme ElementThemeToTheme(ElementTheme theme)
    {
        return theme switch
        {
            ElementTheme.Default => Theme.Default,
            ElementTheme.Light => Theme.Light,
            ElementTheme.Dark => Theme.Dark,
            _ => throw new Exception(),
        };
    }

    private static ElementTheme ThemeToElementTheme(Theme theme)
    {
        return theme switch
        {
            Theme.Default => ElementTheme.Default,
            Theme.Light => ElementTheme.Light,
            Theme.Dark => ElementTheme.Dark,
            _ => throw new Exception(),
        };
    }

    #endregion
}