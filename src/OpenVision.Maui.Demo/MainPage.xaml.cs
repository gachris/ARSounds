namespace OpenVision.Maui.Demo;

public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();
    }

    private async void Image_Reco_Button_Click(object sender, EventArgs e)
    {
        // Navigate to AR Camera page
        await Navigation.PushAsync(new OpenVision.Maui.Demo.ARCamera.MainPage());
    }

    private async void Image_Classification_Button_Click(object sender, EventArgs e)
    {
        // Navigate to Image Classification page
        await Navigation.PushAsync(new OpenVision.ML.Maui.Demo.MainPage());
    }
}
