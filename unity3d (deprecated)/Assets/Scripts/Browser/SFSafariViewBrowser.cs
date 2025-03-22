namespace Assets
{
    public class SFSafariViewBrowser : BrowserBase
    {
        protected override void Launch(string url)
        {
            SFSafariView.LaunchUrl(url);
        }

        public override void Dismiss()
        {
            SFSafariView.Dismiss();
        }
    }
}
