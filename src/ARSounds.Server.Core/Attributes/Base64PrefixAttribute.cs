using System.ComponentModel;

namespace ARSounds.Server.Core.Attributes;

public sealed class Base64PrefixAttribute : DescriptionAttribute
{
    public Base64PrefixAttribute(string description) : base(description)
    {
    }
}
