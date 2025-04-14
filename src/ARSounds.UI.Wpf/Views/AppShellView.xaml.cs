using System.Windows;
using System.Windows.Controls;
using ARSounds.UI.Common.Contracts;
using ARSounds.UI.Common.Data;
using ARSounds.UI.Common.Extensions;
using ARSounds.UI.Common.ViewModels;

namespace ARSounds.UI.Wpf.Views;

public partial class AppShellView : UserControl
{
    public AppShellView(ShellViewModel viewModel, INavigationService navigationService)
    {
        DataContext = viewModel;
        InitializeComponent();

        Loaded += ShellView_Loaded;

        navigationService.Frame = MainFrame;

        navigationService.NavigateToAsync(PageKeys.CameraPage)
            .FireAndForget();
    }

    #region Events Subscriptions

    private async void ShellView_Loaded(object sender, RoutedEventArgs e)
    {
        var viewModel = (ShellViewModel)DataContext;
        await viewModel.InitializeAsync();
    }

    #endregion
}