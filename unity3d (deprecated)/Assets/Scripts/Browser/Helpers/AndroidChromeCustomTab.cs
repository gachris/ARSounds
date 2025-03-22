#if UNITY_ANDROID
using UnityEngine;
#endif

namespace Assets
{
    public static class AndroidChromeCustomTab
    {
        public static void LaunchUrl(string url)
        {
#if UNITY_ANDROID
            using (var unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
            using (var activity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity"))
            using (var intentBuilder = new AndroidJavaObject("android.support.customtabs.CustomTabsIntent$Builder"))
            using (var intent = intentBuilder.Call<AndroidJavaObject>("build"))
            using (var uriClass = new AndroidJavaClass("android.net.Uri"))
            using (var uri = uriClass.CallStatic<AndroidJavaObject>("parse", url))
            {
                intent.Call("launchUrl", activity , uri);
            } 
#endif
        }
    }
}
