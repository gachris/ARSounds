using ARSounds.Core.Targets;
using ARSounds.UI.Maui.FontIcons;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Maui.Graphics;

namespace ARSounds.UI.Maui.Targets.ViewModels;

public class TargetViewModel : ObservableObject
{
    public string Title { get; private set; }

    public string Description { get; private set; }

    public Color BackgroundColor { get; private set; }

    public string BackgroundImage { get; private set; }

    public string Icon { get; private set; }

    public int ItemCount { get; private set; }

    internal static TargetViewModel FromTarget(Target arg1, int arg2)
    {
        return new TargetViewModel()
        {
            Title = "Social",
            Description = "",
            BackgroundColor = Color.FromArgb("#FF921243"),
            BackgroundImage = "https://raw.githubusercontent.com/tlssoftware/raw-material/master/maui-kit/articles/article_01.jpg",
            Icon = IonIcons.SocialFacebook,
            ItemCount = 6,
        };
    }
}
