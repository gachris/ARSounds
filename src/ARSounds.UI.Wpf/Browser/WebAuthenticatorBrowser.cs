using System.IO;
using ARSounds.UI.Wpf.Windows;
using IdentityModel.OidcClient.Browser;
using Microsoft.Web.WebView2.Core;

namespace ARSounds.UI.Wpf.Browser;

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

        var window = new WindowWebView2
        {
            Owner = System.Windows.Application.Current.MainWindow,
        };

        window.WebView.NavigationStarting += (s, e) =>
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
                window.Close();
            }
        };

        window.Closing += (s, e) =>
        {
            signal.Release();
        };

        window.Show();

        var userDataFolder = Path.Combine(Path.GetTempPath(), "ARSounds");
        var cwv2Environment = await CoreWebView2Environment.CreateAsync(null, userDataFolder, null);

        await window.WebView.EnsureCoreWebView2Async(cwv2Environment);

        window.WebView.CoreWebView2.Settings.AreDevToolsEnabled = false;
        window.WebView.CoreWebView2.Settings.AreDefaultContextMenusEnabled = false;
        window.WebView.CoreWebView2.Settings.AreHostObjectsAllowed = false;
        window.WebView.CoreWebView2.Settings.IsPasswordAutosaveEnabled = true;
        window.WebView.CoreWebView2.Settings.IsGeneralAutofillEnabled = true;

        window.WebView.Source = new Uri(_options.StartUrl);

        await signal.WaitAsync(cancellationToken);

        return result;
    }

    private bool BrowserIsNavigatingToRedirectUri(string uriString)
    {
        ArgumentNullException.ThrowIfNull(_options, nameof(_options));
        return uriString.StartsWith(_options.EndUrl, StringComparison.OrdinalIgnoreCase);
    }
}
