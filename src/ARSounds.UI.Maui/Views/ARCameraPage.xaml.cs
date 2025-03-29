using ARSounds.UI.Maui.ViewModels;

namespace ARSounds.UI.Maui.Views;

public partial class ARCameraPage : ContentPage
{
    public ARCameraPage(ARCameraViewModel viewModel)
    {
        BindingContext = viewModel;
        InitializeComponent();
        Loaded += MainPage_Loaded;
    }

    private async void MainPage_Loaded(object? sender, EventArgs e)
    {
        if (BindingContext is not ARCameraViewModel viewModel)
        {
            return;
        }

        await viewModel.InitializeAsync(default);
    }
}