using ARSounds.UI.Maui.ViewModels;

namespace ARSounds.UI.Maui.Services;

public class NavigationMapping
{
    #region Fields/Consts

    private readonly Dictionary<Type, Type> _mappings = new Dictionary<Type, Type>();

    #endregion

    #region Properties

    public IReadOnlyDictionary<Type, Type> Mappings => _mappings;

    #endregion

    #region Methods

    public void Add<TViewModel, TPage>() where TViewModel : ViewModelBase where TPage : Page
    {
        _mappings.Add(typeof(TViewModel), typeof(TPage));
    }

    #endregion
}