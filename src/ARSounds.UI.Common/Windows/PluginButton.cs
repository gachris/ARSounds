namespace ARSounds.UI.Common.Windows;

public class PluginButton
{
    public object? Content { get; set; }

    public bool IsDefault { get; set; }

    public bool IsCancel { get; set; }

    public PluginButtonType ButtonType { get; set; }

    public PluginButtonPosition ButtonPosition { get; set; }

    public int ButtonOrder { get; set; }
}