using ARSounds.Application.Services;
using ARSounds.UI.Wpf.Contracts;
using DevToolbox.Wpf.Media;

namespace ARSounds.UI.Wpf.Services;

public class AppUISettings : IAppUISettings
{
    #region Fields/Consts

    private const string SettingsKey = "AppBackgroundRequestedTheme";

    private readonly ILocalSettingsService _localSettingsService;

    #endregion

    #region Properties

    public ElementTheme Theme { get; private set; }

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

        Theme = cacheTheme;
        ThemeManager.RequestedTheme = Theme;
    }

    public async Task SetThemeAsync(ElementTheme appTheme)
    {
        await _localSettingsService.SaveSettingAsync(SettingsKey, appTheme.ToString());

        Theme = appTheme;
        ThemeManager.RequestedTheme = Theme;
    }

    #endregion
}