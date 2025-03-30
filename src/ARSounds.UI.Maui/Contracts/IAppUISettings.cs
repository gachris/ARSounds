namespace ARSounds.UI.Maui.Contracts;

public interface IAppUISettings
{
    AppTheme Theme { get; }

    Task InitializeAsync();

    Task SetThemeAsync(AppTheme appTheme);
}
