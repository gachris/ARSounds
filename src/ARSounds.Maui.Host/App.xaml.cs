using ARSounds.UI.Maui.Views;

namespace ARSounds.Maui.Host;

public partial class App : Microsoft.Maui.Controls.Application
{
    public App()
    {
        InitializeComponent();
    }

    protected override Window CreateWindow(IActivationState? activationState)
    {
        const int width = 640;
        const int height = 506;

        var appShell = IPlatformApplication.Current?.Services.GetService<AppShell>()!;
        var window = new Window(appShell)
        {
            Width = width,
            Height = height
        };

        return window;
    }
}
