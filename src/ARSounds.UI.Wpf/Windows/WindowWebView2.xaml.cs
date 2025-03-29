using DevToolbox.Wpf.Windows;

namespace ARSounds.UI.Wpf.Windows;

public partial class WindowWebView2 : WindowEx, IDisposable
{
    public WindowWebView2()
    {
        InitializeComponent();
    }

    public void Dispose()
    {
        WebView.Dispose();
    }
}