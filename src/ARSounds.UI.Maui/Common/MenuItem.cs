using System;

namespace ARSounds.UI.Maui.Common;

public class MenuItems
{
    public string Title { get; }

    public string Icon { get; }

    public Type TargetType { get; set; }

    public MenuItems(string title, string icon, Type targetType)
    {
        Title = title;
        Icon = icon;
        TargetType = targetType;
    }
}
