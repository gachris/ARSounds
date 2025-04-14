using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Data;

namespace ARSounds.UI.WinUI.Converters;

public class EnumToBooleanConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        if (value is null) return DependencyProperty.UnsetValue;

        if (parameter is not string parameterString)
            return DependencyProperty.UnsetValue;

        if (Enum.IsDefined(value.GetType(), value) == false)
            return DependencyProperty.UnsetValue;

        var parameterValue = Enum.Parse(value.GetType(), parameterString);

        return parameterValue.Equals(value);
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        if (parameter is not string parameterString) return DependencyProperty.UnsetValue;

        if (targetType.IsEnum)
            return Enum.Parse(targetType, parameterString);

        Type? nullableType = Nullable.GetUnderlyingType(targetType);

        return nullableType is null
            ? throw new ArgumentException($"Provided type {targetType.Name} must be either an enum or a nullable enum")
            : Enum.Parse(nullableType, parameterString);
    }
}