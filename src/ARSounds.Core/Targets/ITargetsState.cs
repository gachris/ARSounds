namespace ARSounds.Core.Targets;

public interface ITargetsState
{
    public IReadOnlyList<Target> Targets { get; }

    void ClearEvents();

    void SetTargetsResult(IEnumerable<Target> targets);
}
