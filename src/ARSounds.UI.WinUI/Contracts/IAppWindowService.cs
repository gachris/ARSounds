using WinUIEx;

namespace ARSounds.UI.WinUI.Contracts;

public interface IAppWindowService
{
    public WindowEx MainWindow { get; }

    void Register(WindowEx windowEx);
}
