using ARSounds.Application.Auth.Commands;
using ARSounds.UI.Common;
using ARSounds.UI.FontIcons;
using ARSounds.UI.Common.ViewModels;
using ARSounds.UI.Services;
using CommunityToolkit.Mvvm.Input;
using MediatR;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Networking;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ARSounds.UI.User.ViewModels;

public partial class ProfileViewModel : ViewModelBase
{
    #region Fields/Consts

    private readonly IMediator _mediator;
    private readonly INavigationService _navigationService;
    private readonly IConnectivity _connectivity;

    #endregion

    #region Properties

    public ICommand TapCommand { get; private set; }

    public string Name { get; set; } = "Nura Lineon";

    public string Email { get; set; } = "nr-lineon@maui.com";

    public string ImageUrl { get; set; } = "user2.png";

    public ObservableCollection<MenuItems> MenuItems { get; } = new ObservableCollection<MenuItems>();

    #endregion

    public ProfileViewModel(IMediator mediator, INavigationService navigationService, IConnectivity connectivity) : base(navigationService)
    {
        _mediator = mediator;
        _navigationService = navigationService;
        _connectivity = connectivity;
    }

    public override async Task InitializeAsync(object initParams)
    {
        MainThread.BeginInvokeOnMainThread(() =>
        {
            PopulateData();
            CommandInit();
        });

        await Task.CompletedTask;
    }

    #region Methods

    private void PopulateData()
    {
        MenuItems.Clear();

        MenuItems.Add(new MenuItems("Edit Profile", IonIcons.Edit, typeof(SettingsViewModel)));
        MenuItems.Add(new MenuItems("Notifications", IonIcons.AndroidNotifications, typeof(SettingsViewModel)));
        MenuItems.Add(new MenuItems("Shipping Address", IonIcons.Location, typeof(SettingsViewModel)));
        MenuItems.Add(new MenuItems("Payment Info", IonIcons.Card, typeof(SettingsViewModel)));
        MenuItems.Add(new MenuItems("Order History", IonIcons.AndroidTime, typeof(SettingsViewModel)));
        MenuItems.Add(new MenuItems("Settings", IonIcons.Settings, typeof(SettingsViewModel)));
        MenuItems.Add(new MenuItems("Delete Account", IonIcons.AndroidDelete, typeof(SettingsViewModel)));
    }

    private void CommandInit()
    {
        TapCommand = new Command<MenuItems>(async item =>
        {
            await _navigationService.PushAsync(item.TargetType);
        });
    }

    [RelayCommand]
    private async Task SignOut()
    {
        if (_connectivity.NetworkAccess is not NetworkAccess.Internet)
        {
            await Shell.Current.DisplayAlert("Internet Offline", "Check your internet and try again!", "OK");
            return;
        }

        var signInUserResultDto = await _mediator.Send(new LogoutUserCommand());
        if (!signInUserResultDto.Success)
        {
            await Shell.Current.DisplayAlert("Error", signInUserResultDto.ErrorMessage, "OK");
        }
        else
        {
            await _navigationService.PushMainAsync(typeof(AppShellViewModel));
        }
    }

    #endregion
}
