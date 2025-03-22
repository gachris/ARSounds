using Android;
using Android.App;
using Android.Content.PM;
using AndroidX.Core.App;
using AndroidX.Core.Content;

[assembly: UsesPermission(Android.Manifest.Permission.Camera)]

namespace OpenVision.Maui.Demo;

[Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, LaunchMode = LaunchMode.SingleTop, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
public class MainActivity : MauiAppCompatActivity
{
    private const int CameraPermissionRequestCode = 1000;

    protected override void OnStart()
    {
        if (ContextCompat.CheckSelfPermission(this, Manifest.Permission.Camera) != Permission.Granted)
        {
            ActivityCompat.RequestPermissions(this, new[] { Manifest.Permission.Camera }, CameraPermissionRequestCode);
        }

        base.OnStart();
    }
}
