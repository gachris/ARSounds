using Microsoft.Extensions.Configuration;

namespace ARSounds.Maui.Host.Helpers;

public static class MauiProgramHelper
{
    public static IConfigurationBuilder AddJsonFromPackageFile(this IConfigurationBuilder configuration, string fileName)
    {
        using var stream = FileSystem.OpenAppPackageFileAsync(fileName).ConfigureAwait(false).GetAwaiter().GetResult();
        return configuration.AddJsonStream(stream);
    }
}