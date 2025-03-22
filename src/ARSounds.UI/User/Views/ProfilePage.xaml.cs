using ARSounds.UI.User.ViewModels;
using Microsoft.Maui.Controls;

namespace ARSounds.UI.User.Views;

public partial class ProfilePage : ContentPage
{
    public ProfilePage(ProfileViewModel bindingContext)
    {
        InitializeComponent();
        BindingContext = bindingContext;
    }
}