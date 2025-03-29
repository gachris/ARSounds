namespace ARSounds.UI.WinUI.Activation;

public abstract class ActivationHandler<T> : IActivationHandler where T : class
{
    #region Methods

    protected virtual bool CanHandleInternal(T args) => true;

    protected abstract Task HandleInternalAsync(T args);

    #endregion

    #region IActivationHandler Implementation

    public bool CanHandle(object args) => args is T && CanHandleInternal((args as T)!);

    public async Task HandleAsync(object args) => await HandleInternalAsync((args as T)!);

    #endregion
}
