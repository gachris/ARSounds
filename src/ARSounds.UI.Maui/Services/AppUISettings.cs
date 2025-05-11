using DevToolbox.Core.Contracts;
using DevToolbox.Core.Media;

namespace ARSounds.UI.Maui.Services;

public class AppUISettings : IAppUISettings
{
    #region Fields/Consts

    private const string SettingsKey = "AppBackgroundRequestedTheme";

    private readonly ILocalSettingsService _localSettingsService;

    #endregion

    #region Properties

    public Theme Theme { get; private set; }

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

        if (!Enum.TryParse(themeName, out Theme cacheTheme))
        {
            cacheTheme = Theme.Default;
        }

        Theme = cacheTheme;

        if (Microsoft.Maui.Controls.Application.Current is not null)
        {
            Microsoft.Maui.Controls.Application.Current.UserAppTheme = ThemeToAppTheme(Theme);
        }
    }

    public async Task SetThemeAsync(Theme theme)
    {
        await _localSettingsService.SaveSettingAsync(SettingsKey, theme.ToString());

        Theme = theme;

        if (Microsoft.Maui.Controls.Application.Current is not null)
        {
            Microsoft.Maui.Controls.Application.Current.UserAppTheme = ThemeToAppTheme(Theme);
        }
    }

    private static AppTheme ThemeToAppTheme(Theme theme)
    {
        return theme switch
        {
            Theme.Default => AppTheme.Unspecified,
            Theme.Light => AppTheme.Light,
            Theme.Dark => AppTheme.Dark,
            _ => throw new Exception(),
        };
    }

    #endregion
}
