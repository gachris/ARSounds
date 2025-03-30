using Microsoft.UI.Xaml.Markup;

namespace ARSounds.UI.WinUI.Helpers;

[MarkupExtensionReturnType(ReturnType = typeof(object))]
public class EnumValueExtension : MarkupExtension
{
    public Type? Type { get; set; }

    public string? Member { get; set; }

    protected override object ProvideValue()
    {
        ArgumentNullException.ThrowIfNull(Type, nameof(Type));
        ArgumentNullException.ThrowIfNull(Member, nameof(Member));

        return Enum.Parse(Type, Member);
    }
}