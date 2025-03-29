using ARSounds.UI.Wpf.Windows.ViewModels;

namespace ARSounds.UI.Wpf.Windows.Views;

public partial class MessageDialogView : BaseDialogView, IDisposable
{
    public MessageDialogView(string title, string message, Uri? imageSource) : base(new MessageDialogViewModel(title, message, imageSource))
    {
        InitializeComponent();
    }
}
