using Microsoft.Maui.Controls;

namespace ARSounds.UI.Helpers;

public static class ResourceHelper
{
    public static T FindResource<T>(string resourceKey)
    {
        if (Microsoft.Maui.Controls.Application.Current?.Resources != null && Microsoft.Maui.Controls.Application.Current.Resources.TryGetValue(resourceKey, out var result))
        {
            if (result is T t)
            {
                return t;
            }
            else if (result is OnPlatform<T> platform)
            {
                return platform;
            }
        }

        return default;
    }
}
