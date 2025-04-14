using System.Windows.Input;

namespace ARSounds.UI.Wpf.Windows.ViewModels;

public record PluginButtonViewModel(object Content, ICommand Command, bool IsDefault, bool IsCancel);