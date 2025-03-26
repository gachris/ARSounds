using System.Globalization;
using OpenVision.Maui.Controls;

namespace ARSounds.UI.Maui.Converters;

public class TargetMatchingEventArgsConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return (TargetMatchingEventArgs)value!;
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
