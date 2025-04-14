using ARSounds.UI.Common.ViewModels;

namespace ARSounds.UI.Maui.Views;

public partial class WalkthroughPage : ContentPage
{
    #region Fields/Consts

    private readonly WalkthroughViewModel _viewModel;

    #endregion

    public WalkthroughPage(WalkthroughViewModel viewModel)
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