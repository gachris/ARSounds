namespace ARSounds.UI.Maui.Browser;

#if WINDOWS
public class WebAuthenticatorBrowser : WinUI.Browser.WebAuthenticatorBrowser
#else
public class WebAuthenticatorBrowser : Platforms.Android.Browser.WebAuthenticatorBrowser
#endif
{
}