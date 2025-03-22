using ARSounds.UI.Common.ViewModels;
using Microsoft.Maui.Controls;

namespace ARSounds.UI.Common.Views;

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