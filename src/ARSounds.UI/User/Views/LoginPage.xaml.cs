using ARSounds.UI.User.ViewModels;
using Microsoft.Maui.Controls;

namespace ARSounds.UI.User.Views;

public partial class LoginPage : ContentPage
{
    public LoginPage(LoginViewModel bindingContext)
    {
        InitializeComponent();
        BindingContext = bindingContext;
    }
}