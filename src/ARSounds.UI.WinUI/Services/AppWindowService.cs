using ARSounds.UI.WinUI.Contracts;
using WinUIEx;

namespace ARSounds.UI.WinUI.Services;

public class AppWindowService : IAppWindowService
{
    #region Properties

    public WindowEx MainWindow { get; private set; } = null!;

    #endregion

    #region IAppWindowService Implementation

    public void Register(WindowEx windowEx)
    {
        if (MainWindow != null)
        {
            throw new InvalidOperationException("MainWindow has already been registered. It cannot be registered again.");
        }

        MainWindow = windowEx;
    }

    #endregion
}
