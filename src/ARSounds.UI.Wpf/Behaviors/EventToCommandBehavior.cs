using Microsoft.Xaml.Behaviors;
using System.Reflection;
using System.Windows;
using System.Windows.Input;

namespace ARSounds.UI.Wpf.Behaviors;

public class EventToCommandBehavior : Behavior<DependencyObject>
{
    private Delegate? _handler;

    public static readonly DependencyProperty EventNameProperty =
        DependencyProperty.Register(nameof(EventName), typeof(string), typeof(EventToCommandBehavior));

    public string EventName
    {
        get => (string)GetValue(EventNameProperty);
        set => SetValue(EventNameProperty, value);
    }

    public static readonly DependencyProperty CommandProperty =
        DependencyProperty.Register(nameof(Command), typeof(ICommand), typeof(EventToCommandBehavior));

    public ICommand Command
    {
        get => (ICommand)GetValue(CommandProperty);
        set => SetValue(CommandProperty, value);
    }

    protected override void OnAttached()
    {
        base.OnAttached();

        if (AssociatedObject == null || string.IsNullOrEmpty(EventName))
            return;

        var eventInfo = AssociatedObject.GetType().GetEvent(EventName);
        if (eventInfo == null)
            throw new ArgumentException($"Event '{EventName}' not found on type '{AssociatedObject.GetType().Name}'");

        var handlerType = eventInfo.EventHandlerType;
        if (handlerType == null)
            throw new InvalidOperationException("Handler type could not be determined.");

        var invokeMethod = handlerType.GetMethod("Invoke");
        var parameters = invokeMethod?.GetParameters();

        if (parameters?.Length == 2)
        {
            var argsType = parameters[1].ParameterType;

            var method = typeof(EventToCommandBehavior).GetMethod(
                nameof(GenericEventHandler),
                BindingFlags.NonPublic | BindingFlags.Instance);

            var genericMethod = method?.MakeGenericMethod(argsType);
            _handler = Delegate.CreateDelegate(handlerType, this, genericMethod!);
            eventInfo.AddEventHandler(AssociatedObject, _handler);
        }
        else
        {
            throw new NotSupportedException("Only events with two parameters (object sender, args) are supported.");
        }
    }

    protected override void OnDetaching()
    {
        base.OnDetaching();

        if (_handler != null && AssociatedObject != null)
        {
            var eventInfo = AssociatedObject.GetType().GetEvent(EventName);
            eventInfo?.RemoveEventHandler(AssociatedObject, _handler);
        }
    }

    // 💡 Remove the EventArgs constraint so it works with custom event args
    private void GenericEventHandler<TEventArgs>(object sender, TEventArgs e)
    {
        if (Command?.CanExecute(e) == true)
        {
            Command.Execute(e);
        }
    }
}
