using System.Globalization;

namespace ARSounds.UI.Maui.Converters;

public class EnumToBooleanConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo cultureInfo)
    {
        if (value is null) return Binding.DoNothing;

        if (parameter is not string parameterString)
            return Binding.DoNothing;

        if (Enum.IsDefined(value.GetType(), value) == false)
            return Binding.DoNothing;

        var parameterValue = Enum.Parse(value.GetType(), parameterString);

        return parameterValue.Equals(value);
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo cultureInfo)
    {
        if (parameter is not string parameterString) return Binding.DoNothing;

        if (targetType.IsEnum)
            return Enum.Parse(targetType, parameterString);

        Type? nullableType = Nullable.GetUnderlyingType(targetType);

        return nullableType is null
            ? throw new ArgumentException($"Provided type {targetType.Name} must be either an enum or a nullable enum")
            : Enum.Parse(nullableType, parameterString);
    }
}