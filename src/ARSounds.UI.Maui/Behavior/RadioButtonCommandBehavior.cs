using System.Windows.Input;

namespace ARSounds.UI.Maui.Behavior;

public static class RadioButtonCommandBehavior
{
    public static readonly BindableProperty CommandProperty =
        BindableProperty.CreateAttached(
            "Command",
            typeof(ICommand),
            typeof(RadioButtonCommandBehavior),
            null,
            propertyChanged: OnCommandChanged);

    public static ICommand GetCommand(BindableObject view) =>
        (ICommand)view.GetValue(CommandProperty);

    public static void SetCommand(BindableObject view, ICommand value) =>
        view.SetValue(CommandProperty, value);

    public static readonly BindableProperty CommandParameterProperty =
        BindableProperty.CreateAttached(
            "CommandParameter",
            typeof(object),
            typeof(RadioButtonCommandBehavior),
            null);

    public static object GetCommandParameter(BindableObject view) =>
        view.GetValue(CommandParameterProperty);

    public static void SetCommandParameter(BindableObject view, object value) =>
        view.SetValue(CommandParameterProperty, value);

    private static void OnCommandChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is RadioButton radioButton)
        {
            radioButton.CheckedChanged -= OnCheckedChanged;
            if (newValue is ICommand)
            {
                radioButton.CheckedChanged += OnCheckedChanged;
            }
        }
    }

    private static void OnCheckedChanged(object? sender, CheckedChangedEventArgs e)
    {
        if (sender is RadioButton radioButton && e.Value) // Only fire when checked
        {
            var command = GetCommand(radioButton);
            var parameter = GetCommandParameter(radioButton);

            if (command?.CanExecute(parameter) == true)
            {
                command.Execute(parameter);
            }
        }
    }
}