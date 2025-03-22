using System;


namespace ARSounds.MauiApp.Controls.Videos;

public interface IVideoController
{
    VideoStatus Status { get; set; }
    TimeSpan Duration { get; set; }
}
