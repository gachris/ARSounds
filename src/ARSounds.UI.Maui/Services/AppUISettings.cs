using ARSounds.Application.Services;
using ARSounds.UI.Maui.Contracts;

namespace ARSounds.UI.Maui.Services;

public class AppUISettings : IAppUISettings
{
    #region Fields/Consts

    private const string SettingsKey = "MauiAppBackgroundRequestedTheme";

    private readonly ILocalSettingsService _localSettingsService;

    #endregion

    #region Properties

    public AppTheme Theme { get; private set; }

    public static bool IsFirstLaunching
    {
        get => Preferences.Get(nameof(IsFirstLaunching), true);
        set => Preferences.Set(nameof(IsFirstLaunching), value);
    }

    #endregion

    public AppUISettings(ILocalSettingsService localSettingsService)
    {
        _localSettingsService = localSettingsService;
    }

    #region Methods

    public async Task InitializeAsync()
    {
        var themeName = await _localSettingsService.ReadSettingAsync<string>(SettingsKey);

        if (!Enum.TryParse(themeName, out AppTheme cacheTheme))
        {
            cacheTheme = AppTheme.Unspecified;
        }

        Theme = cacheTheme;
        
        if (Microsoft.Maui.Controls.Application.Current is not null)
        {
            Microsoft.Maui.Controls.Application.Current.UserAppTheme = Theme;
        }
    }

    public async Task SetThemeAsync(AppTheme appTheme)
    {
        await _localSettingsService.SaveSettingAsync(SettingsKey, appTheme.ToString());

        Theme = appTheme;

        if (Microsoft.Maui.Controls.Application.Current is not null)
        {
            Microsoft.Maui.Controls.Application.Current.UserAppTheme = Theme;
        }
    }

    #endregion
}
