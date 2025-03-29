using ARSounds.UI.Maui.Helpers;

namespace ARSounds.UI.Maui.Services;

public class AppSettings
{
    #region Fields/Consts

    private readonly AppTheme _currentTheme;
    private bool _isDarkTheme = false;
    private int _selectedPrimaryColor;

    #endregion

    #region Properties

    public static AppSettings Instance { get; }

    public static bool IsFirstLaunching
    {
        get => Preferences.Get(nameof(IsFirstLaunching), true);
        set => Preferences.Set(nameof(IsFirstLaunching), value);
    }

    public bool IsDarkTheme
    {
        get => _isDarkTheme;
        set
        {
            if (_isDarkTheme == value) return;

            _isDarkTheme = value;
            if (_isDarkTheme)
            {
                // Dark Theme
                Microsoft.Maui.Controls.Application.Current.Resources.ApplyDarkTheme();
            }
            else
            {
                // Light Theme
                Microsoft.Maui.Controls.Application.Current.Resources.ApplyLightTheme();
            }
        }
    }

    public int SelectedPrimaryColor
    {
        get => _selectedPrimaryColor;
        set
        {
            if (_selectedPrimaryColor == value) return;

            _selectedPrimaryColor = value;
            ThemeHelpers.ApplyColorSet(_selectedPrimaryColor);
        }
    }

    #endregion

    static AppSettings()
    {
        Instance = new AppSettings();
    }

    private AppSettings()
    {
        _currentTheme = Microsoft.Maui.Controls.Application.Current.RequestedTheme;
        _selectedPrimaryColor = _currentTheme == AppTheme.Light ? 0 : 1;  //ThemeUtil: ApplyColorSet1 by default for LightTheme, ApplyColorSet2 by default for DarkTheme
    }
}
