using ARSounds.UI.Maui.ViewModels;

namespace ARSounds.UI.Maui.Views;

public partial class AppShell : Shell
{
    public AppShell(AppShellViewModel viewModel)
    {
        RegisterRoutes();

        BindingContext = viewModel;
        InitializeComponent();

        Loaded += AppShell_Loaded;
    }

    private async void AppShell_Loaded(object? sender, EventArgs e)
    {
        var viewModel = (AppShellViewModel)BindingContext;
        await viewModel.InitializeAsync(null);
    }

    private static void RegisterRoutes()
    {
        Routing.RegisterRoute(nameof(ARCameraPage), typeof(ARCameraPage));
    }
}