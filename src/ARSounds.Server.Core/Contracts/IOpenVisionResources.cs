using OpenVision.Api.Target.Resources;

namespace ARSounds.Server.Core.Contracts;

public interface IOpenVisionResources
{
    TargetListResource GetTargetListResource();
}