namespace ARSounds.UI.Common.Data;

public class WalkthroughBoarding
{
    public string ImagePath { get; }

    public string Title { get; }

    public string Subtitle { get; }

    public WalkthroughBoarding(
        string imagePath,
        string title,
        string subtitle)
    {
        ImagePath = imagePath;
        Title = title;
        Subtitle = subtitle;
    }
}
