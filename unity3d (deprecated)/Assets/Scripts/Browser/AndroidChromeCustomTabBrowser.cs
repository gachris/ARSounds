namespace Assets
{
    public class AndroidChromeCustomTabBrowser : BrowserBase
    {
        protected override void Launch(string url)
        {
            AndroidChromeCustomTab.LaunchUrl(url);
        }
    }
}
