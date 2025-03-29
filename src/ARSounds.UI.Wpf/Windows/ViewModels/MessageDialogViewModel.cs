namespace ARSounds.UI.Wpf.Windows.ViewModels;

public partial class MessageDialogViewModel : DialogViewModel
{
    #region Properties

    public string Title { get; }

    public string Message { get; }

    public Uri? ImageSource { get; }

    #endregion

    public MessageDialogViewModel(string title, string message, Uri? imageSource)
    {
        Title = title;
        Message = message;
        ImageSource = imageSource;
    }
}