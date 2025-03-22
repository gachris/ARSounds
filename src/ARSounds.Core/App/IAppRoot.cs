using ARSounds.Core.Targets;

namespace ARSounds.Core.App;

public interface IAppRoot : IDomainRoot
{
    ITargetsComponent Targets { get; }
}
