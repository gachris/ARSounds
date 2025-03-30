using System.Reflection;
using ARSounds.Localization.Properties;
using ARSounds.UI.Common.ViewModel;
using ARSounds.UI.Maui.Contracts;
using CommunityToolkit.Mvvm.Input;

namespace ARSounds.UI.Maui.ViewModels;

public partial class SettingsViewModel : BaseSettingsViewModel
{
    #region Fields/Conts

    private readonly IAppUISettings _appUISettings;

    private AppTheme _appTheme;

    #endregion

    #region Properties

    public AppTheme AppTheme
    {
        get => _appTheme;
        private set => SetProperty(ref _appTheme, value);
    }

    #endregion

    public SettingsViewModel(IAppUISettings appUISettings)
    {
        _appUISettings = appUISettings;

        AppTheme = _appUISettings.Theme;
    }

    #region Methods Overrides

    protected override string GetVersionDescription()
    {
        var version = Assembly.GetExecutingAssembly().GetName().Version!;
        return $"{Resources.Application_title} - {version.Major}.{version.Minor}.{version.Build}.{version.Revision}";
    }

    #endregion

    #region Methods

    [RelayCommand]
    private async Task SwitchTheme(AppTheme theme)
    {
        await _appUISettings.SetThemeAsync(theme);
    }

    #endregion
}