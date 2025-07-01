using ARSounds.Localization.Properties;
using System.Reflection;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DevToolbox.Core.Contracts;
using DevToolbox.Core.Media;

namespace ARSounds.UI.Common.ViewModels;

public partial class SettingsViewModel : ObservableObject, INavigationViewModelAware
{
    #region Fields/Consts

    private readonly IAppUISettings _appUISettings;

    private Theme _theme;

    #endregion

    #region Properties

    public bool CanGoBack => true;

    public string VersionDescription { get; }

    public Theme Theme
    {
        get => _theme;
        private set => SetProperty(ref _theme, value);
    }

    #endregion

    public SettingsViewModel(IAppUISettings appUISettings)
    {
        _appUISettings = appUISettings;
        VersionDescription = GetVersionDescription();
    }

    #region Methods

    public virtual void OnNavigated(object? parameter = null)
    {
        Theme = _appUISettings.Theme;
    }

    public virtual void OnNavigatedAway()
    {
    }

    private static string GetVersionDescription()
    {
        var version = Assembly.GetExecutingAssembly().GetName().Version!;
        return $"{Resources.Application_title} - {version.Major}.{version.Minor}.{version.Build}.{version.Revision}";
    }

    #endregion

    #region Relay Commands

    [RelayCommand]
    private async Task ChangeTheme(Theme theme)
    {
        if (Theme != theme)
        {
            Theme = theme;
            await _appUISettings.SetThemeAsync(theme);
        }
    }

    #endregion
}