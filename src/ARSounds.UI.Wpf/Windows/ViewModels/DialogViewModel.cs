using CommunityToolkit.Mvvm.ComponentModel;

namespace ARSounds.UI.Wpf.Windows.ViewModels;

public partial class DialogViewModel : ObservableObject, IDisposable
{
    #region Fileds/Consts

    private DialogWindow _hostWindow = default!;
    private bool _disposedValue;

    #endregion

    #region Properties

    public DialogWindow HostWindow
    {
        get => _hostWindow;
        set
        {
            _hostWindow = value;
            AfterLoad();
        }
    }

    #endregion

    #region Methods

    protected virtual void AfterLoad()
    {
    }

    #endregion

    #region IDisposable Implementation

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposedValue)
        {
            if (disposing)
            {
                // TODO: dispose managed state (managed objects)
            }

            // TODO: free unmanaged resources (unmanaged objects) and override finalizer
            // TODO: set large fields to null
            _disposedValue = true;
        }
    }

    // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
    // ~DialogViewModel()
    // {
    //     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
    //     Dispose(disposing: false);
    // }

    public void Dispose()
    {
        // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }

    #endregion
}
