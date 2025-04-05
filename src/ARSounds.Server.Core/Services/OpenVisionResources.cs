using ARSounds.Server.Core.Configuration;
using ARSounds.Server.Core.Contracts;
using Microsoft.Extensions.Options;
using OpenVision.Api.Auth;
using OpenVision.Api.Core;
using OpenVision.Api.Core.Types;
using OpenVision.Api.Target.Resources;
using OpenVision.Api.Target.Services;

namespace ARSounds.Server.Core.Services;

public class OpenVisionResources : IOpenVisionResources
{
    #region Fields/Consts

    private readonly OpenVisionResourcesOptions _options;

    #endregion

    public OpenVisionResources(IOptions<OpenVisionResourcesOptions> options)
    {
        _options = options.Value;
    }

    #region Methods

    public TargetListResource GetTargetListResource()
    {
        var service = new TargetService(new BaseClientService.Initializer()
        {
            ApplicationName = _options.ApplicationName,
            HttpClientInitializer = new UserCredential(new DatabaseApiKey(_options.DatabaseApiKey)),
            ServerUrl = _options.ServerUrl
        });

        return new TargetListResource(service);
    }

    #endregion
}