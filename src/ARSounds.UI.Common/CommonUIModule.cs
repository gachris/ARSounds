using System.Reflection;
using ARSounds.UI.Common.Contracts;
using Microsoft.Extensions.DependencyInjection;
using OpenVision.Core.Configuration;

namespace ARSounds.UI.Common;

public static class CommonUIModule
{
    #region Methods

    public static void ConfigureOpenVision(this IServiceCollection services, string wsUrl)
    {
        VisionSystemConfig.WebSocketUrl = wsUrl;

        VisionSystemConfig.ImageRequestBuilder = new OpenVision.Core.DataTypes.ImageRequestBuilder()
            .WithGrayscale()
            .WithGaussianBlur(new System.Drawing.Size(5, 5), 0)
            .WithLowResolution(320);
    }

    public static void AddIUIServices(this IServiceCollection services, Assembly assembly)
    {
        var uiServiceType = typeof(IUIService);

        var uiServiceInterfaces = assembly.GetTypes()
            .Where(t => t.IsInterface && !t.IsGenericType && t.GetInterfaces().Contains(uiServiceType))
            .ToList();

        var types = assembly.GetTypes()
            .Where(t => !t.IsAbstract && !t.IsInterface)
            .ToList();

        foreach (var type in types)
        {
            var implementedInterfaces = type.GetInterfaces()
                .Where(uiServiceInterfaces.Contains)
                .ToList();

            foreach (var interfaceType in implementedInterfaces)
            {
                services.AddSingleton(interfaceType, type);
            }
        }
    }

    #endregion
}
