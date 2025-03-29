namespace ARSounds.UI.Common.Contracts;

public interface IViewModelAware
{
    void OnNavigated(object? parameter);

    void OnNavigatedAway();
}