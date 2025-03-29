using System.Windows.Controls;
using System.Windows.Navigation;

namespace ARSounds.UI.Wpf.Contracts;

public interface INavigationService
{
    void RegisterFrame(string key, Frame frame);

    void UnregisterFrame(string key);

    void FrameNavigated(string key, Action<object, NavigationEventArgs> action);

    void AddBackEntry(string key, Type pageType);

    void NavigateTo(string key, Type pageType);

    void NavigateTo(string key, Type pageType, object parameter);

    void GoBack(string key);
}
