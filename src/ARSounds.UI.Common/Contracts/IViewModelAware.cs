namespace ARSounds.UI.Common.Contracts;

public interface IViewModelAware
{
    void OnNavigated();

    void OnNavigatedAway();
}