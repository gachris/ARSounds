using System.Globalization;
using ARSounds.UI.Camera;
using OpenVision.Maui.Controls;

namespace ARSounds.UI.Converters;

public class TargetMatchingEventArgsConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        var eventArgs = ((TargetMatchingEventArgs)value!).TargetMatchResults.FirstOrDefault()!;
        var result = new TargetMatchingResult(eventArgs.Id,
                                              eventArgs.ProjectedRegion,
                                              eventArgs.Size,
                                              eventArgs.CenterX,
                                              eventArgs.CenterY,
                                              eventArgs.Angle);
        return result;
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
