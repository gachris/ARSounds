using ARSounds.UI.Maui.ViewModels;

namespace ARSounds.UI.Maui.Views;

public partial class AppShell : Shell
{
    public AppShell(AppShellViewModel viewModel)
    {
        RegisterRoutes();

        BindingContext = viewModel;
        InitializeComponent();
    }

    private static void RegisterRoutes()
    {
        Routing.RegisterRoute(nameof(HomePage), typeof(HomePage));
    }
}