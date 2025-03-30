using ARSounds.UI.Common.Contracts;
using ARSounds.UI.Maui.ViewModels;

namespace ARSounds.UI.Maui.Views;

public partial class AppShell : Shell
{
    public AppShell(ShellViewModel viewModel)
    {
        RegisterRoutes();

        BindingContext = viewModel;
        InitializeComponent();

        Loaded += AppShell_Loaded;
        Navigated += AppShell_Navigated;
    }

    private void AppShell_Navigated(object? sender, ShellNavigatedEventArgs e)
    {
        if (CurrentPage.BindingContext is IViewModelAware modelBase)
        {
            modelBase.OnNavigated();
        }
    }

    private async void AppShell_Loaded(object? sender, EventArgs e)
    {
        var viewModel = (ShellViewModel)BindingContext;
        await viewModel.InitializeAsync();
    }

    private static void RegisterRoutes()
    {
        Routing.RegisterRoute(nameof(ARCameraPage), typeof(ARCameraPage));
    }
}