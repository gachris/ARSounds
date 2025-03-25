#if IOS || MACCATALYST
using PlatformView = ARSounds.UI.Maui.Platforms.MaciOS.MauiVideoPlayer;
#elif ANDROID
using PlatformView = ARSounds.UI.Maui.Platforms.Android.MauiVideoPlayer;
#elif WINDOWS
using PlatformView = ARSounds.UI.Maui.Platforms.Windows.MauiVideoPlayer;
#elif (NETSTANDARD || !PLATFORM) || (NET6_0_OR_GREATER && !IOS && !ANDROID)
using PlatformView = System.Object;
#endif

using Microsoft.Maui.Handlers;
using Microsoft.Maui;
using ARSounds.UI.Maui.Controls.Videos;

namespace ARSounds.UI.Maui.Handlers;

public partial class VideoHandler
{
    public static IPropertyMapper<Video, VideoHandler> PropertyMapper = new PropertyMapper<Video, VideoHandler>(ViewHandler.ViewMapper)
    {
        [nameof(Video.AreTransportControlsEnabled)] = MapAreTransportControlsEnabled,
        [nameof(Video.Source)] = MapSource,
        [nameof(Video.IsLooping)] = MapIsLooping,
        [nameof(Video.Position)] = MapPosition
    };

    public static CommandMapper<Video, VideoHandler> CommandMapper = new(ViewCommandMapper)
    {
        [nameof(Video.UpdateStatus)] = MapUpdateStatus,
        [nameof(Video.PlayRequested)] = MapPlayRequested,
        [nameof(Video.PauseRequested)] = MapPauseRequested,
        [nameof(Video.StopRequested)] = MapStopRequested
    };

    public VideoHandler() : base(PropertyMapper, CommandMapper)
    {
    }
}
