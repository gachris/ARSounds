using ARSounds.UI.Maui.ViewModels;

namespace ARSounds.UI.Maui.Views;

public partial class HomePage : ContentPage
{
    public HomePage(HomeViewModel viewModel)
    {
        BindingContext = viewModel;
        InitializeComponent();
        Loaded += MainPage_Loaded;
    }

    private async void MainPage_Loaded(object? sender, EventArgs e)
    {
        if (BindingContext is not HomeViewModel homeViewModel)
        {
            return;
        }

        await homeViewModel.InitializeAsync(default);
    }
}