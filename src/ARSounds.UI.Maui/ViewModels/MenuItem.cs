namespace ARSounds.UI.Maui.ViewModels;

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
