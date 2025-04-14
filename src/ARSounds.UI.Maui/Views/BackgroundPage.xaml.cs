using ARSounds.UI.Common.ViewModels;

namespace ARSounds.UI.Maui.Views;

public partial class BackgroundPage : ContentPage
{
    #region Fields/Consts

    private readonly BackgroundViewModel _viewModel;

    #endregion

    public BackgroundPage(BackgroundViewModel viewModel)
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
}