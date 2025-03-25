using ARSounds.UI.Maui.User.ViewModels;
using Microsoft.Maui.Controls;

namespace ARSounds.UI.Maui.User.Views;

public partial class ProfilePage : ContentPage
{
    public ProfilePage(ProfileViewModel bindingContext)
    {
        InitializeComponent();
        BindingContext = bindingContext;
    }
}