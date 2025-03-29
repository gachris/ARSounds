using ARSounds.UI.Wpf.Contracts;

namespace ARSounds.UI.Wpf.Services;

public class ApplicationService : IApplicationService
{
    #region Fields/Consts

    private readonly IApplication _application;

    #endregion

    public ApplicationService(IApplication application)
    {
        _application = application;
    }

    #region Methods

    public void Shutdown()
    {
        _application.Shutdown();
    }

    #endregion
}