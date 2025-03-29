using CommunityToolkit.Mvvm.ComponentModel;

namespace ARSounds.UI.Common.ViewModels;

public abstract partial class BaseSettingsViewModel : ObservableObject
{
    #region Properties

    public string VersionDescription { get; }

    #endregion

    public BaseSettingsViewModel()
    {
        VersionDescription = GetVersionDescription();
    }

    #region Methods

    protected abstract string GetVersionDescription();

    #endregion
}