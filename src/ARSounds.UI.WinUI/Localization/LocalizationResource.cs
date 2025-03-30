using ARSounds.UI.Common.Localization;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Markup;

namespace ARSounds.UI.WinUI.Localization;

[ContentProperty(Name = nameof(Text))]
[MarkupExtensionReturnType(ReturnType = typeof(object))]
public class LocalizationResourceExtension : MarkupExtension
{
    #region Properties

    public string? Text { get; set; }

    public IValueConverter? Converter { get; set; }

    #endregion

    public LocalizationResourceExtension()
    {
    }

    public LocalizationResourceExtension(string text)
    {
        Text = text;
    }

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
            Path = new PropertyPath($"[{Text}]"),
            Source = LocalizationResourceManager.Current,
            Converter = Converter
        };
    }

    protected override object ProvideValue(IXamlServiceProvider serviceProvider)
    {
        return ProvideValue((IServiceProvider)serviceProvider);
    }
}