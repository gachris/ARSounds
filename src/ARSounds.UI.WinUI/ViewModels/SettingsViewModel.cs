using ARSounds.UI.Common.Helpers;
using ARSounds.UI.WinUI.Helpers;
using ARSounds.UI.WinUI.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.UI.Xaml;
using System.Reflection;
using Windows.ApplicationModel;

namespace ARSounds.UI.WinUI.ViewModels;

public partial class SettingsViewModel : ObservableObject
{
    #region Fields/Consts

    private readonly IThemeSelectorService _themeSelectorService;

    private ElementTheme _elementTheme;

    #endregion

    #region Properties

    public string VersionDescription { get; }

    public ElementTheme ElementTheme
    {
        get => _elementTheme;
        private set => SetProperty(ref _elementTheme, value, nameof(ElementTheme));
    }

    #endregion

    public SettingsViewModel(IThemeSelectorService themeSelectorService)
    {
        _themeSelectorService = themeSelectorService;
        _elementTheme = _themeSelectorService.Theme;

        VersionDescription = GetVersionDescription();
    }

    #region Methods

    private static string GetVersionDescription()
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

    #endregion

    #region Relay Commands

    [RelayCommand]
    private async Task SwitchTheme(ElementTheme theme)
    {
        if (ElementTheme != theme)
        {
            ElementTheme = theme;
            await _themeSelectorService.SetThemeAsync(theme);
        }
    }

    #endregion
}