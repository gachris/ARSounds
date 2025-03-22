using ARSounds.UI.Common.ViewModels;
using ARSounds.UI.Helpers;
using ARSounds.UI.Services;
using ARSounds.UI.Themes;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Graphics;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace ARSounds.UI.User.ViewModels;

public partial class SettingsViewModel : ViewModelBase
{
    private readonly ObservableCollection<ThemePalette> _viewCollection = new();

    [ObservableProperty]
    private Color _lightThemeButtonBackgroundColor;

    [ObservableProperty]
    private Color _lightThemeButtonTextColor;

    [ObservableProperty]
    private Color _darkThemeButtonBackgroundColor;

    [ObservableProperty]
    private Color _darkThemeButtonTextColor;

    public ObservableCollection<ThemePalette> ThemePalettes => _viewCollection;

    public ThemePalette SelectedPrimaryColorItem { get; set; }

    public SettingsViewModel(INavigationService navigationService) : base(navigationService)
    {
        Microsoft.Maui.Controls.Application.Current.Resources.TryGetValue("ThemePrimaryColorOption1", out var primaryColorOption1);
        Microsoft.Maui.Controls.Application.Current.Resources.TryGetValue("ThemePrimaryColorOption2", out var primaryColorOption2);
        Microsoft.Maui.Controls.Application.Current.Resources.TryGetValue("ThemePrimaryColorOption3", out var primaryColorOption3);
        Microsoft.Maui.Controls.Application.Current.Resources.TryGetValue("ThemePrimaryColorOption4", out var primaryColorOption4);
        Microsoft.Maui.Controls.Application.Current.Resources.TryGetValue("ThemePrimaryColorOption5", out var primaryColorOption5);

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
        Microsoft.Maui.Controls.Application.Current.Resources.ApplyLightTheme();
        AppSettings.Instance.SelectedPrimaryColor = 0;
        UpdatePrimaryColor();
    }

    [RelayCommand]
    private void SettingDarkTheme()
    {
        Microsoft.Maui.Controls.Application.Current.Resources.ApplyDarkTheme();
        AppSettings.Instance.SelectedPrimaryColor = 1;
        UpdatePrimaryColor();
    }

    private void UpdatePrimaryColor()
    {
        Microsoft.Maui.Controls.Application.Current.Resources.TryGetValue("PrimaryColor", out var primaryColor);
        Microsoft.Maui.Controls.Application.Current.Resources.TryGetValue("White", out var white);
        Microsoft.Maui.Controls.Application.Current.Resources.TryGetValue("Gray600", out var gray600);
        Microsoft.Maui.Controls.Application.Current.Resources.TryGetValue("Black", out var black);

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