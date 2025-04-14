using ARSounds.UI.Common.Contracts;
using ARSounds.UI.Common.Data;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

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
        return (_navigationService.Frame as Frame)?.Content == null;
    }

    protected async override Task HandleInternalAsync(LaunchActivatedEventArgs args)
    {
        await _navigationService.NavigateToAsync(PageKeys.CameraPage, args.Arguments);
    }

    #endregion
}
