using System.Reflection;
using ARSounds.Localization.Properties;
using ARSounds.UI.Common.ViewModels;
using ARSounds.UI.Wpf.Contracts;
using CommunityToolkit.Mvvm.Input;
using DevToolbox.Wpf.Media;

namespace ARSounds.UI.Wpf.ViewModels;

public partial class SettingsViewModel : BaseSettingsViewModel
{
    #region Fields/Conts

    private readonly IAppUISettings _appUISettings;

    private ElementTheme _elementTheme;

    #endregion

    #region Properties

    public ElementTheme ElementTheme
    {
        get => _elementTheme;
        private set => SetProperty(ref _elementTheme, value, nameof(ElementTheme));
    }

    #endregion

    public SettingsViewModel(IAppUISettings appUISettings)
    {
        _appUISettings = appUISettings;

        ThemeManager.RequestedThemeChanged += ThemeManager_RequestedThemeChanged;

        ElementTheme = _appUISettings.Theme;
    }

    #region Methods

    protected override string GetVersionDescription()
    {
        var version = Assembly.GetExecutingAssembly().GetName().Version!;
        return $"{Resources.Application_title} - {version.Major}.{version.Minor}.{version.Build}.{version.Revision}";
    }

    #endregion

    #region Relay Commands

    [RelayCommand]
    private async Task SwitchTheme(ElementTheme theme)
    {
        await _appUISettings.SetThemeAsync(theme);
    }

    #endregion

    #region Events Subscriptions

    private void ThemeManager_RequestedThemeChanged(object? sender, EventArgs e)
    {
        ElementTheme = _appUISettings.Theme;
    }

    #endregion
}