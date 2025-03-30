using ARSounds.UI.Common.ViewModels;
using ARSounds.UI.Maui.ViewModels;
using CommonServiceLocator;

namespace ARSounds.UI.Maui;

public class ViewModelLocator
{
    static ViewModelLocator()
    {
        AccountViewModel = ServiceLocator.Current.GetInstance<AccountViewModel>();
        ARCameraViewModel = ServiceLocator.Current.GetInstance<ARCameraViewModel>();
        SettingsViewModel = ServiceLocator.Current.GetInstance<SettingsViewModel>();
    }

    public static AccountViewModel AccountViewModel { get; }

    public static ARCameraViewModel ARCameraViewModel { get; }

    public static SettingsViewModel SettingsViewModel { get; }
}
