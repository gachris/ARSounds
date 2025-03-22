namespace ARSounds.Core;

public class ApplicationEvent
{
    #region Fields/Consts

    private static long _index;

    #endregion

    #region Properties

    public long Index { get; } = GetNextIndex();

    #endregion

    private static long GetNextIndex() => Interlocked.Increment(ref _index);
}
