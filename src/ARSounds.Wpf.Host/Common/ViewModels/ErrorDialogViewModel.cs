using ARSounds.UI.Wpf.Windows.ViewModels;

namespace ARSounds.Wpf.Host.Common.ViewModels;

public partial class ErrorDialogViewModel : DialogViewModel
{
    #region Properties

    public string Message { get; }

    public string? StackTrace { get; }

    #endregion

    public ErrorDialogViewModel(string message, string? stackTrace)
    {
        Message = message;
        StackTrace = stackTrace;
    }
}