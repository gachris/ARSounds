namespace ARSounds.ApplicationFlow;

public class ApplicationEvent
{
    #region Fields/Consts

    private static long _index;

    #endregion

    #region Properties

    public long Index { get; } = GetNextIndex();

    #endregion

    #region Methods

    private static long GetNextIndex() => Interlocked.Increment(ref _index);

    #endregion
}