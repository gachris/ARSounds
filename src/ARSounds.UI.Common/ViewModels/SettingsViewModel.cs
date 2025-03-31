using ARSounds.UI.Common.Contracts;
using ARSounds.UI.Common.Media;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace ARSounds.UI.Common.ViewModels;

public partial class SettingsViewModel : ObservableObject, IViewModelAware
{
    #region Fields/Consts

    private readonly IAppUISettings _appUISettings;

    private Theme _theme;

    #endregion

    #region Properties

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
        VersionDescription = appUISettings.GetVersionDescription();

        Theme = appUISettings.Theme;
    }

    #region Methods

    public virtual void OnNavigated()
    {
    }

    public virtual void OnNavigatedAway()
    {
    }

    #endregion

    #region Relay Commands

    [RelayCommand]
    private async Task SwitchTheme(Theme theme)
    {
        if (Theme != theme)
        {
            Theme = theme;
            await _appUISettings.SetThemeAsync(theme);
        }
    }

    #endregion
}