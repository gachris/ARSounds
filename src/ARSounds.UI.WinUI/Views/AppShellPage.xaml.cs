using ARSounds.UI.Common.ViewModels;
using CommonServiceLocator;
using DevToolbox.Core.Contracts;
using DevToolbox.WinUI.Contracts;
using DevToolbox.WinUI.Helpers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using Windows.System;
using Windows.UI.ViewManagement;

namespace ARSounds.UI.WinUI.Views;

public sealed partial class AppShellPage : Page
{
    #region Fields/Consts

    private readonly IAppWindowService _appWindowService;

    private readonly Microsoft.UI.Dispatching.DispatcherQueue _dispatcherQueue;
    private readonly UISettings _settings;

    #endregion

    #region Properties

    public ShellViewModel ViewModel { get; }

    #endregion

    public AppShellPage(ShellViewModel viewModel, IAppWindowService appWindowService, INavigationService navigationService)
    {
        _appWindowService = appWindowService;

        ViewModel = viewModel;
        DataContext = viewModel;

        InitializeComponent();

        navigationService.Frame = NavigationFrame;

        Loaded += OnLoaded;

        _appWindowService.MainWindow.ExtendsContentIntoTitleBar = true;
        _appWindowService.MainWindow.SetTitleBar(AppTitleBar);
        AppTitleBarText.Text = "AppDisplayName".GetLocalized();

        _dispatcherQueue = Microsoft.UI.Dispatching.DispatcherQueue.GetForCurrentThread();
        _settings = new UISettings();
        _settings.ColorValuesChanged += Settings_ColorValuesChanged;
    }

    #region Methods

    private static KeyboardAccelerator BuildKeyboardAccelerator(VirtualKey key, VirtualKeyModifiers? modifiers = null)
    {
        var keyboardAccelerator = new KeyboardAccelerator() { Key = key };

        if (modifiers.HasValue)
        {
            keyboardAccelerator.Modifiers = modifiers.Value;
        }

        keyboardAccelerator.Invoked += OnKeyboardAcceleratorInvoked;

        return keyboardAccelerator;
    }

    #endregion

    #region Events Subscriptions

    private async void OnLoaded(object sender, RoutedEventArgs e)
    {
        TitleBarHelper.UpdateTitleBar(_appWindowService.MainWindow, RequestedTheme);

        KeyboardAccelerators.Add(BuildKeyboardAccelerator(VirtualKey.Left, VirtualKeyModifiers.Menu));
        KeyboardAccelerators.Add(BuildKeyboardAccelerator(VirtualKey.GoBack));

        await ViewModel.InitializeAsync();
    }

    private static async void OnKeyboardAcceleratorInvoked(KeyboardAccelerator sender, KeyboardAcceleratorInvokedEventArgs args)
    {
        var navigationService = ServiceLocator.Current.GetService<INavigationService>()!;

        var result = await navigationService.GoBackAsync();

        args.Handled = result;
    }

    private void Settings_ColorValuesChanged(UISettings sender, object args)
    {
        _dispatcherQueue.TryEnqueue(() =>
        {
            if (AppTitleBarText != null)
            {
                TitleBarHelper.UpdateTitleBar(_appWindowService.MainWindow, AppTitleBarText.ActualTheme);
            }
        });
    }

    #endregion
}