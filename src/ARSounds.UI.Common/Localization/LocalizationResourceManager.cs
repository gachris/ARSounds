using System.ComponentModel;
using System.Globalization;
using ARSounds.Localization.Properties;

namespace ARSounds.UI.Common.Localization;

public class LocalizationResourceManager : INotifyPropertyChanged
{
    #region Fields/Consts

    private static readonly LocalizationResourceManager _current = new();

    public event PropertyChangedEventHandler? PropertyChanged;

    #endregion

    #region Properties

    public static LocalizationResourceManager Current => _current;

    public object? this[string resourceKey] => Resources.ResourceManager.GetObject(resourceKey, Resources.Culture);

    #endregion

    private LocalizationResourceManager()
    {
        Resources.Culture = CultureInfo.CurrentCulture;
    }

    #region Methods

    public void SetCulture(CultureInfo culture)
    {
        Resources.Culture = culture;
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(null));
    }

    #endregion
}