using OpenVision.Api.Target.Resources;

namespace ARSounds.Server.Core.Contracts;

/// <summary>
/// Defines methods to access resources provided by the OpenVision API.
/// </summary>
public interface IOpenVisionService
{
    /// <summary>
    /// Retrieves the target list resource for managing and querying target data.
    /// </summary>
    /// <returns>
    /// An instance of <see cref="TargetListResource"/> that encapsulates the configuration and endpoints for interacting with the target list.
    /// </returns>
    TargetListResource GetTargetListResource();
}
