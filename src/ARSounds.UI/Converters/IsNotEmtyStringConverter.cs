using Microsoft.Maui.Controls;
using System;

namespace ARSounds.MauiApp.Converters;

public class IsNotEmptyStringConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    {
        return String.IsNullOrEmpty(parameter.ToString()) ? false : (object)true;
    }

    public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    {
        throw new NotSupportedException();
    }
}
