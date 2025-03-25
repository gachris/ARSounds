using ARSounds.UI.Maui.Common.ViewModels;
using Microsoft.Maui.Controls;

namespace ARSounds.UI.Maui.Common.Views;

public partial class AppShell : Shell
{
    public AppShell(AppShellViewModel bindingContext)
    {
        RegisterRoutes();
        InitializeComponent();
        BindingContext = bindingContext;
    }

    private static void RegisterRoutes()
    {
        Routing.RegisterRoute(nameof(HomePage), typeof(HomePage));
    }
}