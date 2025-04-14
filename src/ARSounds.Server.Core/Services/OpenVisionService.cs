using ARSounds.Server.Core.Configuration;
using ARSounds.Server.Core.Contracts;
using Microsoft.Extensions.Options;
using OpenVision.Api.Auth;
using OpenVision.Api.Core;
using OpenVision.Api.Core.Types;
using OpenVision.Api.Target.Resources;
using OpenVision.Api.Target.Services;

namespace ARSounds.Server.Core.Services;

/// <summary>
/// Provides methods for accessing resources from the OpenVision API.
/// </summary>
public class OpenVisionService : IOpenVisionService
{
    #region Fields/Consts

    private readonly OpenVisionOptions _options;

    #endregion

    /// <summary>
    /// Initializes a new instance of the <see cref="OpenVisionService"/> class.
    /// </summary>
    /// <param name="options">The OpenVision configuration options.</param>
    public OpenVisionService(IOptions<OpenVisionOptions> options)
    {
        _options = options.Value;
    }

    #region Methods

    /// <summary>
    /// Retrieves the target list resource, which provides endpoints for managing and querying target data.
    /// </summary>
    /// <returns>
    /// An instance of <see cref="TargetListResource"/> that encapsulates the configuration and endpoints for interacting with the target list.
    /// </returns>
    public TargetListResource GetTargetListResource()
    {
        var service = new TargetService(new BaseClientService.Initializer()
        {
            ApplicationName = _options.ApplicationName,
            HttpClientInitializer = new UserCredential(new DatabaseApiKey(_options.DatabaseApiKey)),
            ServerBaseUri = _options.ServerUrl
        });

        return new TargetListResource(service);
    }

    #endregion
}
