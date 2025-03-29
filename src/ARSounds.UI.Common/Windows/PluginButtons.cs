namespace ARSounds.UI.Common.Windows;

[Flags]
public enum PluginButtons
{
    None = 0,
    Yes = 1 << 0,
    OK = 1 << 1,
    No = 1 << 2,
    Cancel = 1 << 3
}