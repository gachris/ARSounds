using System;


namespace ARSounds.MauiApp.Controls.Videos;

public class VideoPositionEventArgs : EventArgs
{
    public TimeSpan Position { get; private set; }

    public VideoPositionEventArgs(TimeSpan position)
    {
        Position = position;
    }
}
