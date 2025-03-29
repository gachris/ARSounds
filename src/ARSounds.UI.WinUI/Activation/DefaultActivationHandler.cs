using ARSounds.UI.WinUI.Services;
using Microsoft.UI.Xaml;

namespace ARSounds.UI.WinUI.Activation;

public class DefaultActivationHandler : ActivationHandler<LaunchActivatedEventArgs>
{
    #region Fields/Consts

    private readonly INavigationService _navigationService;

    #endregion

    public DefaultActivationHandler(INavigationService navigationService)
    {
        _navigationService = navigationService;
    }

    #region Methods Overrides

    protected override bool CanHandleInternal(LaunchActivatedEventArgs args)
    {
        return _navigationService.Frame?.Content == null;
    }

    protected async override Task HandleInternalAsync(LaunchActivatedEventArgs args)
    {
        _navigationService.NavigateTo(PageKeys.CameraPage, args.Arguments);

        await Task.CompletedTask;
    }

    #endregion
}
