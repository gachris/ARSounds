using ARSounds.Localization.Properties;
using DevToolbox.WinUI.Helpers;
using Windows.UI.ViewManagement;
using WinUIEx;

namespace ARSounds.UI.WinUI.Windows;

public sealed partial class WindowWebView2 : WindowEx
{
    #region Fields/Consts

    private readonly UISettings _settings;

    #endregion

    public WindowWebView2()
    {
        InitializeComponent();

        IsMaximizable = false;
        IsResizable = false;
        IsAlwaysOnTop = true;
        IsShownInSwitchers = false;
        IsMinimizable = false;
        IsTitleBarVisible = true;

        Title = Resources.Application_title;

        ExtendsContentIntoTitleBar = true;
        SetTitleBar(AppTitleBar);
        AppTitleBarText.Text = Resources.Application_title;

        _settings = new UISettings();
        _settings.ColorValuesChanged += Settings_ColorValuesChanged;

        TitleBarHelper.UpdateTitleBar(this, WebView.RequestedTheme);
    }

    #region Events Subscriptions

    private void Settings_ColorValuesChanged(UISettings sender, object args)
    {
        if (AppTitleBarText != null)
        {
            TitleBarHelper.UpdateTitleBar(this, AppTitleBarText.ActualTheme);
        }
    }

    #endregion
}
