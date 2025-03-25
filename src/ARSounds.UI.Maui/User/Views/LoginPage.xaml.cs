using ARSounds.UI.Maui.User.ViewModels;
using Microsoft.Maui.Controls;

namespace ARSounds.UI.Maui.User.Views;

public partial class LoginPage : ContentPage
{
    public LoginPage(LoginViewModel bindingContext)
    {
        InitializeComponent();
        BindingContext = bindingContext;
    }
}