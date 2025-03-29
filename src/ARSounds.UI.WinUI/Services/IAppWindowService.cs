using WinUIEx;

namespace ARSounds.UI.WinUI.Services;

public interface IAppWindowService
{
    public WindowEx MainWindow { get; }

    void Register(WindowEx windowEx);
}
