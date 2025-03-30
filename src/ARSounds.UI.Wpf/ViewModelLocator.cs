using System.ComponentModel;
using System.Windows;
using ARSounds.UI.Common.ViewModels;
using CommonServiceLocator;

namespace ARSounds.UI.Wpf;

public class ViewModelLocator
{
    public static ShellViewModel ShellViewModel { get; } = null!;

    public static AccountViewModel AccountViewModel { get; } = null!;

    public static ARCameraViewModel ARCameraViewModel { get; } = null!;

    public static SettingsViewModel SettingsViewModel { get; } = null!;

    static ViewModelLocator()
    {
        if (DesignerProperties.GetIsInDesignMode(new DependencyObject()))
        {
            return;
        }

        ShellViewModel = ServiceLocator.Current.GetInstance<ShellViewModel>();
        AccountViewModel = ServiceLocator.Current.GetInstance<AccountViewModel>();
        ARCameraViewModel = ServiceLocator.Current.GetInstance<ARCameraViewModel>();
        SettingsViewModel = ServiceLocator.Current.GetInstance<SettingsViewModel>();
    }
}
