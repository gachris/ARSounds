using System.Collections.ObjectModel;
using ARSounds.UI.Maui.Helpers;
using ARSounds.UI.Maui.Services;
using ARSounds.UI.Maui.Themes;
using CommunityToolkit.Mvvm.Input;

namespace ARSounds.UI.Maui.ViewModels;

public partial class SettingsViewModel : ViewModelBase
{
    private readonly ObservableCollection<ThemePalette> _viewCollection = [];

    private Color _lightThemeButtonBackgroundColor = null!;
    private Color _lightThemeButtonTextColor = null!;
    private Color _darkThemeButtonBackgroundColor = null!;
    private Color _darkThemeButtonTextColor = null!;

    public Color LightThemeButtonBackgroundColor
    {
        get => _lightThemeButtonBackgroundColor;
        set => SetProperty(ref _lightThemeButtonBackgroundColor, value);
    }

    public Color LightThemeButtonTextColor
    {
        get => _lightThemeButtonTextColor;
        set => SetProperty(ref _lightThemeButtonTextColor, value);
    }

    public Color DarkThemeButtonBackgroundColor
    {
        get => _darkThemeButtonBackgroundColor;
        set => SetProperty(ref _darkThemeButtonBackgroundColor, value);
    }

    public Color DarkThemeButtonTextColor
    {
        get => _darkThemeButtonTextColor;
        set => SetProperty(ref _darkThemeButtonTextColor, value);
    }

    public ObservableCollection<ThemePalette> ThemePalettes => _viewCollection;

    public ThemePalette SelectedPrimaryColorItem { get; set; } = null!;

    public SettingsViewModel(INavigationService navigationService) : base(navigationService)
    {
        Microsoft.Maui.Controls.Application.Current!.Resources.TryGetValue("ThemePrimaryColorOption1", out var primaryColorOption1);
        Microsoft.Maui.Controls.Application.Current!.Resources.TryGetValue("ThemePrimaryColorOption2", out var primaryColorOption2);
        Microsoft.Maui.Controls.Application.Current!.Resources.TryGetValue("ThemePrimaryColorOption3", out var primaryColorOption3);
        Microsoft.Maui.Controls.Application.Current!.Resources.TryGetValue("ThemePrimaryColorOption4", out var primaryColorOption4);
        Microsoft.Maui.Controls.Application.Current!.Resources.TryGetValue("ThemePrimaryColorOption5", out var primaryColorOption5);

        var colorItems = new List<ThemePalette>
        {
            new ThemePalette()
            {
                Index = 0,
                Color = (Color)primaryColorOption1
            },
            new ThemePalette()
            {
                Index = 1,
                Color = (Color)primaryColorOption2
            },
            new ThemePalette()
            {
                Index = 2,
                Color = (Color)primaryColorOption3
            },
            new ThemePalette()
            {
                Index = 3,
                Color = (Color)primaryColorOption4
            },
            new ThemePalette()
            {
                Index = 4,
                Color = (Color)primaryColorOption5
            }
        };

        foreach (var colorItem in colorItems)
        {
            _viewCollection.Add(colorItem);
        }

        UpdatePrimaryColor();
    }

    [RelayCommand]
    private void ColorPaletteSelectionChanged()
    {
        AppSettings.Instance.SelectedPrimaryColor = SelectedPrimaryColorItem.Index;
        UpdatePrimaryColor();
    }

    [RelayCommand]
    private void SettingLightTheme()
    {
        Microsoft.Maui.Controls.Application.Current!.Resources.ApplyLightTheme();
        AppSettings.Instance.SelectedPrimaryColor = 0;
        UpdatePrimaryColor();
    }

    [RelayCommand]
    private void SettingDarkTheme()
    {
        Microsoft.Maui.Controls.Application.Current!.Resources.ApplyDarkTheme();
        AppSettings.Instance.SelectedPrimaryColor = 1;
        UpdatePrimaryColor();
    }

    private void UpdatePrimaryColor()
    {
        Microsoft.Maui.Controls.Application.Current!.Resources.TryGetValue("PrimaryColor", out var primaryColor);
        Microsoft.Maui.Controls.Application.Current!.Resources.TryGetValue("White", out var white);
        Microsoft.Maui.Controls.Application.Current!.Resources.TryGetValue("Gray600", out var gray600);
        Microsoft.Maui.Controls.Application.Current!.Resources.TryGetValue("Black", out var black);

        if (AppSettings.Instance.IsDarkTheme == false)
        {
            LightThemeButtonBackgroundColor = (Color)primaryColor;
            LightThemeButtonTextColor = (Color)white;
            DarkThemeButtonBackgroundColor = (Color)gray600;
            DarkThemeButtonTextColor = (Color)white;
        }
        else
        {
            LightThemeButtonBackgroundColor = (Color)gray600;
            LightThemeButtonTextColor = (Color)black;
            DarkThemeButtonBackgroundColor = (Color)primaryColor;
            DarkThemeButtonTextColor = (Color)white;
        }
    }
}