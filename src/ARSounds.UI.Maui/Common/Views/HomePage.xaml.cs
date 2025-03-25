using ARSounds.UI.Maui.Common.ViewModels;
using Microsoft.Maui.Controls;

namespace ARSounds.UI.Maui.Common.Views;

public partial class HomePage : ContentPage
{
    private HomeViewModel _bindingContext;

    public HomePage(HomeViewModel bindingContext)
    {
        _bindingContext = bindingContext;
        BindingContext = bindingContext;
        
        Loaded += MainPage_Loaded;
        
        InitializeComponent();
    }

    private async void MainPage_Loaded(object sender, System.EventArgs e)
    {
        await _bindingContext.InitializeAsync(null);
    }
}