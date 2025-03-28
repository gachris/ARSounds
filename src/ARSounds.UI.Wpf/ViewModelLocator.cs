﻿using ARSounds.UI.Common.ViewModels;
using ARSounds.UI.Wpf.ViewModels;
using CommonServiceLocator;

namespace ARSounds.UI.Wpf;

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
