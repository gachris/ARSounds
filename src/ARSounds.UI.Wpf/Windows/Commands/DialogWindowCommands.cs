using System.Windows.Input;

namespace ARSounds.UI.Wpf.Windows.Commands;

public class DialogWindowCommands
{
    public static readonly RoutedCommand Yes = new(nameof(Yes), typeof(DialogWindowCommands));

    public static readonly RoutedCommand No = new(nameof(No), typeof(DialogWindowCommands));

    public static readonly RoutedCommand OK = new(nameof(OK), typeof(DialogWindowCommands));

    public static readonly RoutedCommand Cancel = new(nameof(Cancel), typeof(DialogWindowCommands));
}