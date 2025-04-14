using ARSounds.UI.Common.Windows;
using ARSounds.UI.Wpf.Windows.ViewModels;
using ARSounds.UI.Wpf.Windows.Views;
using DevToolbox.Wpf.Windows;
using DevToolbox.Wpf.Windows.Effects;

namespace ARSounds.UI.Wpf.Windows;

public partial class DialogWindow : WindowEx, IDisposable
{
    #region Fields/Consts

    private bool _disposedValue;
    private ModalResult _modalResult;
    private BaseDialogWindowViewModel? _dialogWindowViewModel;

    #endregion

    #region Properties

    public ModalResult ModalResult
    {
        get => _modalResult;
        set
        {
            DialogResult = true;
            _modalResult = value;
            StopWaitingAnimation();
            Close();
        }
    }

    #endregion

    public DialogWindow()
    {
        InitializeComponent();

        WindowBehavior.SetWindowEffect(this, new Mica());
    }

    public DialogWindow(BaseDialogView view, DialogOptions dialogOptions) : this()
    {
        InitializeViewComponent(view, dialogOptions);
    }

    #region Methods

    private void InitializeViewComponent(BaseDialogView view, DialogOptions dialogOptions)
    {
        view.AttachToWindowEvents(this);

        Width = dialogOptions.Width;
        Height = dialogOptions.Height;
        MaxWidth = dialogOptions.MaxWidth;
        MaxHeight = dialogOptions.MaxHeight;
        MinWidth = dialogOptions.MinWidth;
        MinHeight = dialogOptions.MinHeight;
        SizeToContent = dialogOptions.SizeToContent;
        ResizeMode = dialogOptions.ResizeMode;
        Title = dialogOptions.WindowTitle ?? string.Empty;
        IsTitleBarVisible = dialogOptions.IsTitleBarVisible;

        if (ResizeMode == System.Windows.ResizeMode.NoResize)
        {
            Chrome.ResizeBorderThickness = new System.Windows.Thickness(0);
        }

        if (!dialogOptions.IsTitleBarVisible)
        {
            Chrome.CaptionHeight = 0;
        }

        _dialogWindowViewModel = new(this, view, dialogOptions)
        {
            WaitingAnimationTitle = dialogOptions.AnimationTitle,
            WaitingAnimationMessage = dialogOptions.AnimationMessage
        };

        DataContext = _dialogWindowViewModel;
    }

    public void StartWaitingAnimation()
    {
        if (_dialogWindowViewModel is not null)
            _dialogWindowViewModel.WaitingAnimationBusy = true;
    }

    public void StopWaitingAnimation()
    {
        if (_dialogWindowViewModel is not null)
            _dialogWindowViewModel.WaitingAnimationBusy = false;
    }

    public ModalResult ShowModal()
    {
        return ShowDialog() ?? false ? ModalResult : ModalResult.Cancel;
    }

    #endregion

    #region IDisposable Implementation

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposedValue)
        {
            if (disposing)
            {
                _dialogWindowViewModel?.Dispose();
                _dialogWindowViewModel = null;

                DataContext = null;
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