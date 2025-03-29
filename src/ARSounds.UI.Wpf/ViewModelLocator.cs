using System.ComponentModel;
using System.Windows;
using ARSounds.UI.Common.ViewModels;
using ARSounds.UI.Wpf.ViewModels;
using CommonServiceLocator;

namespace ARSounds.UI.Wpf;

public class ViewModelLocator
{
    static ViewModelLocator()
    {
        if (DesignerProperties.GetIsInDesignMode(new DependencyObject()))
        {
            return;
        }

        AccountViewModel = ServiceLocator.Current.GetInstance<AccountViewModel>();
        ARCameraViewModel = ServiceLocator.Current.GetInstance<ARCameraViewModel>();
        SettingsViewModel = ServiceLocator.Current.GetInstance<SettingsViewModel>();
    }

    public static AccountViewModel AccountViewModel { get; } = null!;

    public static ARCameraViewModel ARCameraViewModel { get; } = null!;

    public static SettingsViewModel SettingsViewModel { get; } = null!;
}
