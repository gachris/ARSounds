using OpenVision.Core.Configuration;

namespace OpenVision.Maui.Demo;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();

        MainPage = new AppShell();
    }
}
