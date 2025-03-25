using ARSounds.Application.Auth;
using ARSounds.Application.Auth.Commands;
using ARSounds.UI.Handlers;
using ARSounds.UI.Common.Views;
using ARSounds.UI.Helpers;
using ARSounds.UI.User.Views;
using MediatR;
using OpenVision.Core.Configuration;

namespace ARSounds.Maui.Host;

public partial class App : Microsoft.Maui.Controls.Application
{
    public App()
    {
        VisionSystemConfig.WebSocketUrl = "wss://localhost:44320/ws";

        InitializeComponent();

        #region Handlers

        //Borderless entry
        Microsoft.Maui.Handlers.EntryHandler.Mapper.AppendToMapping(nameof(BorderlessEntry), (handler, view) =>
        {
            if (view is BorderlessEntry)
            {
#if __ANDROID__
                handler.PlatformView.SetBackgroundColor(Android.Graphics.Color.Transparent);
#elif __IOS__
                handler.PlatformView.BorderStyle = UIKit.UITextBorderStyle.None;
#elif __WINDOWS__
                handler.PlatformView.TextBox.BorderThickness = new Thickness(0);
#endif
            }
        });

        //Borderless editor
        Microsoft.Maui.Handlers.EntryHandler.Mapper.AppendToMapping(nameof(BorderlessEditor), (handler, view) =>
        {
            if (view is BorderlessEditor)
            {
#if __ANDROID__
                handler.PlatformView.SetBackgroundColor(Android.Graphics.Color.Transparent);
#elif __IOS__
                handler.PlatformView.BorderStyle = UIKit.UITextBorderStyle.None;
#elif __WINDOWS__
                handler.PlatformView.BorderThickness = new Thickness(0);
#endif
            }
        });

        #endregion

        MainPage = new LoadingPage();
    }

    protected override async void OnStart()
    {
        base.OnStart();

        var mediator = ServiceHelper.GetService<IMediator>();
        var authService = ServiceHelper.GetService<IAuthService>();

        await mediator.Send(new SignInUserFromCacheCommand()).ConfigureAwait(false);

        MainThread.BeginInvokeOnMainThread(() =>
        {
            if (!authService.IsUserLoggedIn)
            {
                var loginPage = ServiceHelper.GetService<LoginPage>();
                MainPage = new NavigationPage(loginPage);
            }
            else
            {
                var appShell = ServiceHelper.GetService<AppShell>();
                MainPage = appShell;
            }
        });
    }

    protected override Window CreateWindow(IActivationState? activationState)
    {
        var window = base.CreateWindow(activationState);

        const int width = 640;
        const int height = 506;

        window.Width = width;
        window.Height = height;

        return window;
    }
}
