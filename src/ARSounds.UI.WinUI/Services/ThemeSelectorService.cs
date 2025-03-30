using ARSounds.UI.Common.Contracts;
using ARSounds.UI.WinUI.Contracts;
using ARSounds.UI.WinUI.Helpers;
using Microsoft.UI.Xaml;

namespace ARSounds.UI.WinUI.Services;

public class ThemeSelectorService : IThemeSelectorService
{
    #region Fields/Consts

    private const string SettingsKey = "AppBackgroundRequestedTheme";

    private readonly ILocalSettingsService _localSettingsService;
    private readonly IAppWindowService _appWindowService;

    #endregion

    #region Properties

    public ElementTheme Theme { get; set; } = ElementTheme.Default;

    #endregion

    public ThemeSelectorService(ILocalSettingsService localSettingsService, IAppWindowService appWindowService)
    {
        _localSettingsService = localSettingsService;
        _appWindowService = appWindowService;
    }

    #region IThemeSelectorService Implementation

    public async Task InitializeAsync()
    {
        Theme = await LoadThemeFromSettingsAsync();
        await Task.CompletedTask;
    }

    public async Task SetThemeAsync(ElementTheme theme)
    {
        Theme = theme;

        await SetRequestedThemeAsync();
        await SaveThemeInSettingsAsync(Theme);
    }

    public async Task SetRequestedThemeAsync()
    {
        if (_appWindowService.MainWindow.Content is FrameworkElement rootElement)
        {
            rootElement.RequestedTheme = Theme;

            TitleBarHelper.UpdateTitleBar(_appWindowService.MainWindow, Theme);
        }

        await Task.CompletedTask;
    }

    #endregion

    #region Methods

    private async Task<ElementTheme> LoadThemeFromSettingsAsync()
    {
        var themeName = await _localSettingsService.ReadSettingAsync<string>(SettingsKey);

        if (Enum.TryParse(themeName, out ElementTheme cacheTheme))
        {
            return cacheTheme;
        }

        return ElementTheme.Default;
    }

    private async Task SaveThemeInSettingsAsync(ElementTheme theme)
    {
        await _localSettingsService.SaveSettingAsync(SettingsKey, theme.ToString());
    }

    #endregion
}
