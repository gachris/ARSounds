using ARSounds.UI.Common.Media;

namespace ARSounds.UI.Common.Contracts;

public interface IAppUISettings
{
    Theme Theme { get; }

    Task InitializeAsync();

    Task SetThemeAsync(Theme elementTheme);

    string GetVersionDescription();
}