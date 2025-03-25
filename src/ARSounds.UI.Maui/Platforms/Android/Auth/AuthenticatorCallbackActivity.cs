using Android.App;
using Android.Content;
using Android.Content.PM;

namespace ARSounds.UI.Maui.Platforms.Android.Auth;

[Activity(NoHistory = true, LaunchMode = LaunchMode.SingleTop, Exported = true)]
[IntentFilter([Intent.ActionView],
    Categories = [Intent.CategoryDefault, Intent.CategoryBrowsable],
    DataScheme = CALLBACK_SCHEME)]
public class AuthenticatorCallbackActivity : WebAuthenticatorCallbackActivity
{
    const string CALLBACK_SCHEME = "arsounds";
}