using ARSounds.UI.Common.Contracts;
using CommunityToolkit.Mvvm.ComponentModel;

namespace ARSounds.UI.Common.ViewModel;

public abstract partial class BaseSettingsViewModel : ObservableObject, IViewModelAware
{
    #region Properties

    public string VersionDescription { get; }

    #endregion

    public BaseSettingsViewModel()
    {
        VersionDescription = GetVersionDescription();
    }

    #region Methods

    public void OnNavigated()
    {
    }

    public void OnNavigatedAway()
    {
    }

    protected abstract string GetVersionDescription();

    #endregion
}