using ARSounds.MauiApp.Enums;
using ARSounds.UI.Helpers;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using System;
using System.Globalization;

namespace ARSounds.MauiApp.Converters;

public class AvatarWithStatusColorConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    {
        string resourceName;

        var stringValue = value != null ? value.ToString() : "";

        switch (stringValue)
        {
            case nameof(AvatarStatus.Online):
                resourceName = "Green";
                break;

            case nameof(AvatarStatus.Busy):
                resourceName = "Red";
                break;

            case nameof(AvatarStatus.Away):
                resourceName = "Orange";
                break;

            default: // Offline
                resourceName = "DisabledColor";
                break;
        }

        return ResourceHelper.FindResource<Color>(resourceName);
    }

    public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    {
        throw new NotSupportedException();
    }
}
