using ARSounds.UI.Common.Contracts;
using ARSounds.UI.Maui.ViewModels;

namespace ARSounds.UI.Maui.Views;

public partial class AppShellPage : Shell
{
    #region Fields/Consts

    private Page? _lastPage;

    #endregion

    public AppShellPage(ShellViewModel viewModel)
    {
        BindingContext = viewModel;
        InitializeComponent();

        Loaded += AppShell_Loaded;
        Navigated += AppShell_Navigated;
    }

    #region Events Subscriptions

    private async void AppShell_Loaded(object? sender, EventArgs e)
    {
        var viewModel = (ShellViewModel)BindingContext;
        await viewModel.InitializeAsync();
    }

    private void AppShell_Navigated(object? sender, ShellNavigatedEventArgs e)
    {
        if (_lastPage?.BindingContext is IViewModelAware oldViewModel)
        {
            oldViewModel.OnNavigatedAway();
        }

        if (CurrentPage?.BindingContext is IViewModelAware newViewModel)
        {
            newViewModel.OnNavigated();
        }

        _lastPage = CurrentPage;
    } 

    #endregion
}