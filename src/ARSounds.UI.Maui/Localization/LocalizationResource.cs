using ARSounds.UI.Common.Localization;

namespace ARSounds.UI.Maui.Localization;

[ContentProperty(nameof(Text))]
[RequireService([typeof(IProvideValueTarget)])]
public class LocalizationResourceExtension : IMarkupExtension<BindingBase>
{
    #region Properties

    public string? Text { get; set; }

    public required IValueConverter Converter { get; set; }

    #endregion

    /// <summary>
    /// No need to create an additional property for LocalizationResourceManager, we can hide it in MarkupExtension 
    /// The Title value can be updated to:
    /// Title="{loc:LocalizationResource MainPageTitle}"
    /// </summary>
    /// <param name="serviceProvider"></param>
    /// <returns></returns>
    public BindingBase ProvideValue(IServiceProvider serviceProvider)
    {
        return new Binding
        {
            Mode = BindingMode.OneWay,
            Path = $"[{Text}]",
            Source = LocalizationResourceManager.Current,
            Converter = Converter
        };
    }

    object IMarkupExtension.ProvideValue(IServiceProvider serviceProvider)
    {
        return ProvideValue(serviceProvider);
    }
}