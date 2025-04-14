using Microsoft.UI.Xaml;

namespace ARSounds.UI.WinUI.Contracts;

public interface IThemeSelectorService
{
    ElementTheme Theme { get; }

    Task InitializeAsync();

    Task SetThemeAsync(ElementTheme theme);

    Task SetRequestedThemeAsync();
}
