using System.Reflection;
using ARSounds.Localization.Properties;
using ARSounds.UI.Wpf.Contracts;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DevToolbox.Wpf.Media;

namespace ARSounds.UI.Wpf.ViewModels;

public partial class SettingsViewModel : ObservableObject
{
    #region Fields/Conts

    private ElementTheme _theme;
    private readonly IAppUISettings _appUISettings;

    #endregion

    #region Properties

    public string ApplicationVersion { get; }

    public ElementTheme Theme
    {
        get => _theme;
        private set => SetProperty(ref _theme, value, nameof(Theme));
    }

    #endregion

    public SettingsViewModel(IAppUISettings appUISettings)
    {
        _appUISettings = appUISettings;

        ThemeManager.RequestedThemeChanged += ThemeManager_RequestedThemeChanged;

        ApplicationVersion = GetApplicationVersion();

        Theme = _appUISettings.Theme;
    }

    #region Methods

    private static string GetApplicationVersion()
    {
        var version = Assembly.GetExecutingAssembly().GetName().Version!;
        return $"{Resources.Application_title} - {version.Major}.{version.Minor}.{version.Build}.{version.Revision}";
    }

    #endregion

    #region Relay Commands

    [RelayCommand]
    private async Task ChangeTheme(ElementTheme theme)
    {
        await _appUISettings.SetThemeAsync(theme);
    }

    #endregion

    #region Events Subscriptions

    private void ThemeManager_RequestedThemeChanged(object? sender, EventArgs e)
    {
        Theme = _appUISettings.Theme;
    }

    #endregion
}