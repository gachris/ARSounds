using System.Windows;
using System.Windows.Controls;
using ARSounds.UI.Common.Data;
using ARSounds.UI.Common.ViewModels;
using DevToolbox.Core.Contracts;
using DevToolbox.Core.Extensions;

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