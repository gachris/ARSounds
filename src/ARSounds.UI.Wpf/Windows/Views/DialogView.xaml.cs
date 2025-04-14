using ARSounds.UI.Wpf.Windows.ViewModels;

namespace ARSounds.UI.Wpf.Windows.Views;

public partial class DialogView : BaseDialogView
{
    public DialogView(DialogViewModel PluginViewModel) : base(PluginViewModel)
    {
        InitializeComponent();
    }
}

public abstract class BaseDialogView : System.Windows.Controls.UserControl, IDisposable
{
    #region Fields/Consts

    private DialogViewModel? _dialogViewModel;
    private bool _disposedValue;

    #endregion

    #region Properties

    protected DialogWindow? HostWindow { get; private set; }

    #endregion

    public BaseDialogView(DialogViewModel PluginViewModel)
    {
        DataContext = PluginViewModel;
        _dialogViewModel = PluginViewModel;
    }

    #region Methods

    public void AttachToWindowEvents(DialogWindow dialogWindow)
    {
        if (_dialogViewModel == null) return;

        HostWindow = dialogWindow;
        _dialogViewModel.HostWindow = HostWindow;
    }

    #endregion

    #region IDisposable Implementation

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposedValue)
        {
            if (disposing)
            {
                _dialogViewModel?.Dispose();
                _dialogViewModel = null;

                HostWindow = null;
            }

            // TODO: free unmanaged resources (unmanaged objects) and override finalizer
            // TODO: set large fields to null
            _disposedValue = true;
        }
    }

    // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
    // ~DialogView()
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
