using CommunityToolkit.Mvvm.ComponentModel;

namespace ARSounds.UI.Maui.Camera.ViewModels;

public partial class CameraButton : ObservableObject
{
    [ObservableProperty]
    private string _text;

    [ObservableProperty]
    private string _icon;

    [ObservableProperty]
    private bool _isCurrent;
}
