using ARSounds.UI.Common.ViewModels;

namespace ARSounds.UI.Maui.Views;

public partial class ARCameraPage : ContentPage
{
    #region Fields/Consts

    private readonly ARCameraViewModel _viewModel;

    #endregion

    public ARCameraPage(ARCameraViewModel viewModel)
    {
        _viewModel = viewModel;

        BindingContext = viewModel;
        InitializeComponent();
    }

    #region Methods Overrides

    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);

        _viewModel.OnNavigated();
    }

    protected override void OnNavigatedFrom(NavigatedFromEventArgs args)
    {
        base.OnNavigatedFrom(args);

        _viewModel.OnNavigatedAway();
    }

    #endregion

    #region Events Subscriptions

    private void OnFlyoutToggleClicked(object sender, EventArgs e)
    {
        Shell.Current.FlyoutIsPresented = !Shell.Current.FlyoutIsPresented;
    }

    #endregion
}