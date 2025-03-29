using System.Windows;
using System.Windows.Controls;
using ARSounds.UI.Wpf.Contracts;
using ARSounds.UI.Wpf.ViewModels;

namespace ARSounds.UI.Wpf.Views;

public partial class ShellView : UserControl
{
    #region Fields/Consts

    private readonly INavigationService _navigationService;

    #endregion

    public ShellView(ShellViewModel viewModel, INavigationService navigationService)
    {
        DataContext = viewModel;
        InitializeComponent();

        navigationService.RegisterFrame(NavigationFrameKeys.Shell, MainFrame);
        navigationService.NavigateTo(NavigationFrameKeys.Shell, typeof(ARCameraPage));

        Loaded += ShellView_Loaded;
        _navigationService = navigationService;
    }

    #region Events Subscriptions

    private async void ShellView_Loaded(object sender, RoutedEventArgs e)
    {
        var viewModel = (ShellViewModel)DataContext;
        await viewModel.InitializeAsync();

        var window = Window.GetWindow(this);
        window.Closed += (sender, e) => _navigationService.UnregisterFrame(NavigationFrameKeys.Shell);
    }

    #endregion
}
