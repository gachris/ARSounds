namespace ARSounds.UI.Common.Contracts;

public interface INavigationService
{
    object? Frame { get; set; }

    bool CanGoBack { get; }

    Task<bool> AddBackEntryAsync(string pageKey);

    Task<bool> GoBackAsync();

    Task<bool> NavigateToAsync(string pageKey, object? parameter = null, bool clearNavigation = false);
}