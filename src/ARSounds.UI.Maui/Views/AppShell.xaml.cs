using ARSounds.UI.Common.ViewModels;

namespace ARSounds.UI.Maui.Views;

public partial class AppShell : Shell
{
    #region Fields/Consts

    private readonly ShellViewModel _viewModel;

    #endregion

    public AppShell(ShellViewModel viewModel)
    {
        _viewModel = viewModel;

        BindingContext = viewModel;
        InitializeComponent();

        Loaded += AppShell_Loaded;
    }

    #region Events Subscriptions

    private async void AppShell_Loaded(object? sender, EventArgs e)
    {
        await _viewModel.InitializeAsync();
    }

    #endregion
}