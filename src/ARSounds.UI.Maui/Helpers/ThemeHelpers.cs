using ARSounds.UI.Maui.Services;
using ARSounds.UI.Maui.Themes;

namespace ARSounds.UI.Maui.Helpers;

public static class ThemeHelpers
{
    public static void ApplyDarkTheme(this ResourceDictionary resources)
    {
        if (resources != null)
        {
            var mergedDictionaries = resources.MergedDictionaries;
            var lightTheme = mergedDictionaries.OfType<LightTheme>().FirstOrDefault();
            if (lightTheme != null)
            {
                mergedDictionaries.Remove(lightTheme);
            }

            mergedDictionaries.Add(new DarkTheme());
            AppSettings.Instance.IsDarkTheme = true;
        }
    }

    public static void ApplyLightTheme(this ResourceDictionary resources)
    {
        if (resources != null)
        {
            var mergedDictionaries = resources.MergedDictionaries;

            var darkTheme = mergedDictionaries.OfType<DarkTheme>().FirstOrDefault();
            if (darkTheme != null)
            {
                mergedDictionaries.Remove(darkTheme);
            }

            mergedDictionaries.Add(new LightTheme());
            AppSettings.Instance.IsDarkTheme = false;
        }
    }

    public static void ApplyColorSet(int index)
    {
        switch (index)
        {
            case 0:
                ApplyColorSet1();
                break;
            case 1:
                ApplyColorSet2();
                break;
            case 2:
                ApplyColorSet3();
                break;
            case 3:
                ApplyColorSet4();
                break;
            case 4:
                ApplyColorSet5();
                break;
        }
    }

    private static void ApplyColorSet1()
    {
        Microsoft.Maui.Controls.Application.Current.Resources.TryGetValue("ThemePrimaryColorOption1", out var primaryColorOption1);

        Microsoft.Maui.Controls.Application.Current.Resources["PrimaryColor"] = (Color)primaryColorOption1;
        Microsoft.Maui.Controls.Application.Current.Resources["PrimaryDarkColor"] = Color.FromArgb("#1a5ac6");
        Microsoft.Maui.Controls.Application.Current.Resources["PrimaryDarkenColor"] = Color.FromArgb("#174fb0");
        Microsoft.Maui.Controls.Application.Current.Resources["PrimaryLighterColor"] = Color.FromArgb("#73a0ed");
        Microsoft.Maui.Controls.Application.Current.Resources["PrimaryLight"] = Color.FromArgb("#cdddf9");
        Microsoft.Maui.Controls.Application.Current.Resources["PrimaryGradient"] = Color.FromArgb("#00acff");
    }

    private static void ApplyColorSet2()
    {
        Microsoft.Maui.Controls.Application.Current.Resources.TryGetValue("ThemePrimaryColorOption2", out var primaryColorOption2);

        Microsoft.Maui.Controls.Application.Current.Resources["PrimaryColor"] = (Color)primaryColorOption2;
        Microsoft.Maui.Controls.Application.Current.Resources["PrimaryDarkColor"] = Color.FromArgb("#4b3ae1");
        Microsoft.Maui.Controls.Application.Current.Resources["PrimaryDarkenColor"] = Color.FromArgb("#3829ba");
        Microsoft.Maui.Controls.Application.Current.Resources["PrimaryLighterColor"] = Color.FromArgb("#b5aefb");
        Microsoft.Maui.Controls.Application.Current.Resources["PrimaryLight"] = Color.FromArgb("#eae8fe");
        Microsoft.Maui.Controls.Application.Current.Resources["PrimaryGradient"] = Color.FromArgb("#aa9cfc");
    }

    private static void ApplyColorSet3()
    {
        Microsoft.Maui.Controls.Application.Current.Resources.TryGetValue("ThemePrimaryColorOption3", out var primaryColorOption3);

        Microsoft.Maui.Controls.Application.Current.Resources["PrimaryColor"] = (Color)primaryColorOption3;
        Microsoft.Maui.Controls.Application.Current.Resources["PrimaryDarkColor"] = Color.FromArgb("#056c56");
        Microsoft.Maui.Controls.Application.Current.Resources["PrimaryDarkenColor"] = Color.FromArgb("#045343");
        Microsoft.Maui.Controls.Application.Current.Resources["PrimaryLighterColor"] = Color.FromArgb("#98f0de");
        Microsoft.Maui.Controls.Application.Current.Resources["PrimaryLight"] = Color.FromArgb("#ebf9f7");
        Microsoft.Maui.Controls.Application.Current.Resources["PrimaryGradient"] = Color.FromArgb("#0ed342");
    }

    private static void ApplyColorSet4()
    {
        Microsoft.Maui.Controls.Application.Current.Resources.TryGetValue("ThemePrimaryColorOption4", out var primaryColorOption4);

        Microsoft.Maui.Controls.Application.Current.Resources["PrimaryColor"] = (Color)primaryColorOption4;
        Microsoft.Maui.Controls.Application.Current.Resources["PrimaryDarkColor"] = Color.FromArgb("#d0424f");
        Microsoft.Maui.Controls.Application.Current.Resources["PrimaryDarkenColor"] = Color.FromArgb("#ab3641");
        Microsoft.Maui.Controls.Application.Current.Resources["PrimaryLighterColor"] = Color.FromArgb("#edcacd");
        Microsoft.Maui.Controls.Application.Current.Resources["PrimaryLight"] = Color.FromArgb("#ffe8f4");
        Microsoft.Maui.Controls.Application.Current.Resources["PrimaryGradient"] = Color.FromArgb("e83f94");
    }

    private static void ApplyColorSet5()
    {
        Microsoft.Maui.Controls.Application.Current.Resources.TryGetValue("ThemePrimaryColorOption5", out var primaryColorOption5);

        Microsoft.Maui.Controls.Application.Current.Resources["PrimaryColor"] = (Color)primaryColorOption5;
        Microsoft.Maui.Controls.Application.Current.Resources["PrimaryDarkColor"] = Color.FromArgb("#a43106");
        Microsoft.Maui.Controls.Application.Current.Resources["PrimaryDarkenColor"] = Color.FromArgb("#862805");
        Microsoft.Maui.Controls.Application.Current.Resources["PrimaryLighterColor"] = Color.FromArgb("#fa9e7c");
        Microsoft.Maui.Controls.Application.Current.Resources["PrimaryLight"] = Color.FromArgb("#fee7de");
        Microsoft.Maui.Controls.Application.Current.Resources["PrimaryGradient"] = Color.FromArgb("#ff6374");
    }
}
