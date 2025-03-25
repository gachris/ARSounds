using CommonServiceLocator;

namespace ARSounds.Application;

public class AppServiceLocator : IServiceLocator
{
    #region Fields/Consts

    private readonly IServiceProvider _host;

    #endregion

    public AppServiceLocator(IServiceProvider host)
    {
        _host = host;
    }

    #region IServiceLocator Implementation

    public IEnumerable<object?> GetAllInstances(Type serviceType)
    {
        return _host.GetServices(serviceType);
    }

    public IEnumerable<TService> GetAllInstances<TService>()
    {
        return _host.GetServices<TService>();
    }

    public object GetInstance(Type serviceType)
    {
        return _host.GetRequiredService(serviceType);
    }

    public object GetInstance(Type serviceType, string key)
    {
        return _host.GetRequiredKeyedService(serviceType, key);
    }

    public TService GetInstance<TService>() where TService : notnull
    {
        return _host.GetRequiredService<TService>();
    }

    public TService? GetInstance<TService>(string key) where TService : notnull
    {
        return _host.GetRequiredKeyedService<TService>(key);
    }

    public object? GetService(Type serviceType)
    {
        return _host.GetService(serviceType);
    }

    #endregion
}