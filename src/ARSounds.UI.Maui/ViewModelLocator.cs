using ARSounds.UI.Maui.ViewModels;
using CommonServiceLocator;

namespace ARSounds.UI.Maui;

public class ViewModelLocator
{
    static ViewModelLocator()
    {
        SettingsViewModel = ServiceLocator.Current.GetInstance<SettingsViewModel>();
    }

    #region Settings

    public static SettingsViewModel SettingsViewModel { get; }

    #endregion
}
