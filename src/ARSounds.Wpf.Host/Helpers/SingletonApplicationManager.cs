namespace ARSounds.Wpf.Host.Helpers;

public class SingletonApplicationManager
{
    #region Fields/Consts

    /// <summary>
    /// The event mutex name.
    /// </summary>
    private readonly string _uniqueEventName;

    /// <summary>
    /// The unique mutex name.
    /// </summary>
    private readonly string _uniqueMutexName;

    /// <summary>
    /// The event wait handle.
    /// </summary>
    private EventWaitHandle? _eventWaitHandle;

    /// <summary>
    /// The mutex.
    /// </summary>
    private Mutex? _mutex;

    #endregion

    public SingletonApplicationManager(string uniqueEventName, string uniqueMutexName)
    {
        _uniqueEventName = uniqueEventName;
        _uniqueMutexName = uniqueMutexName;
    }

    #region Methods

    public void Register(System.Windows.Application application, Action onRegistered, Action OnAppFocused)
    {
        ArgumentNullException.ThrowIfNull(application);
        ArgumentNullException.ThrowIfNull(onRegistered);

        _mutex = new Mutex(true, _uniqueMutexName, out bool isOwned);
        _eventWaitHandle = new EventWaitHandle(false, EventResetMode.AutoReset, _uniqueEventName);

        // So, R# would not give a warning that this variable is not used.
        GC.KeepAlive(_mutex);

        if (isOwned)
        {
            // Spawn a thread which will be waiting for our event
            var thread = new Thread(() =>
            {
                while (_eventWaitHandle.WaitOne())
                {
                    application.Dispatcher.Invoke(() => OnAppFocused.Invoke());
                }
            })
            {
                IsBackground = true
            };

            thread.Start();
            onRegistered.Invoke();

            return;
        }

        // Notify other instance so it could bring itself to foreground.
        _eventWaitHandle.Set();

        // Terminate this instance.
        application.Shutdown();
    }

    #endregion
}