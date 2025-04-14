using ARSounds.UI.WinUI.Windows;
using IdentityModel.OidcClient.Browser;
using Microsoft.Web.WebView2.Core;
using WinUIEx;

namespace ARSounds.UI.WinUI.Browser;

public class WebAuthenticatorBrowser : IBrowser
{
    private BrowserOptions? _options = null;

    public WebAuthenticatorBrowser() { }

    public async Task<BrowserResult> InvokeAsync(BrowserOptions options, CancellationToken cancellationToken = default)
    {
        _options = options;

        var signal = new SemaphoreSlim(0, 1);
        var result = new BrowserResult
        {
            ResultType = BrowserResultType.UserCancel
        };

        var browserWindow = new WindowWebView2();

        browserWindow.CenterOnScreen(550, 750);

        browserWindow.WebView.NavigationStarting += (s, e) =>
        {
            if (BrowserIsNavigatingToRedirectUri(e.Uri))
            {
                e.Cancel = true;

                result = new BrowserResult
                {
                    ResultType = BrowserResultType.Success,
                    Response = e.Uri
                };

                signal.Release();
                browserWindow.Close();
            }
        };

        browserWindow.Closed += (_, _) =>
        {
            signal.Release();
        };

        browserWindow.Activate();

        var environment = await CoreWebView2Environment.CreateAsync();
        await browserWindow.WebView.EnsureCoreWebView2Async(environment);

        // Configure settings
        var settings = browserWindow.WebView.CoreWebView2.Settings;
        settings.AreDevToolsEnabled = false;
        settings.AreDefaultContextMenusEnabled = false;
        settings.AreHostObjectsAllowed = false;
        settings.IsPasswordAutosaveEnabled = true;
        settings.IsGeneralAutofillEnabled = true;

        browserWindow.WebView.Source = new Uri(_options.StartUrl);

        await signal.WaitAsync(cancellationToken);

        return result;
    }

    private bool BrowserIsNavigatingToRedirectUri(string uriString)
    {
        ArgumentNullException.ThrowIfNull(_options, nameof(_options));
        return uriString.StartsWith(_options.EndUrl, StringComparison.OrdinalIgnoreCase);
    }
}
