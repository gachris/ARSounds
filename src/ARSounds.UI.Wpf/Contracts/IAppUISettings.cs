using DevToolbox.Wpf.Media;

namespace ARSounds.UI.Wpf.Contracts;

public interface IAppUISettings
{
    ElementTheme Theme { get; }

    Task InitializeAsync();

    Task SetThemeAsync(ElementTheme appTheme);
}
