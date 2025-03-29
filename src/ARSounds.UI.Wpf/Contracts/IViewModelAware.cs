namespace ARSounds.UI.Wpf.Contracts;

public interface IViewModelAware
{
    void OnNavigated(object? parameter);

    void OnNavigatedAway();
}