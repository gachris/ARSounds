namespace ARSounds.Core.Targets;

public interface ITargetsComponent
{
    public IReadOnlyList<Target> Targets { get; }

    void ClearEvents();

    void SetTargetsResult(IEnumerable<Target> targets);
}
