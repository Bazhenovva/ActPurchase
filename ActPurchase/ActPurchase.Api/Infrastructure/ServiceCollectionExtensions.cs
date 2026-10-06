using System.Reflection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace ActPurchase.Api.Infrastructure;

/// <summary>
/// Зарегистрировать несколько реализаций одного интерфейса
/// </summary>

public static class ServiceCollectionExtensions
{
    public static void RegisterImplementationsOf<TService>(this IServiceCollection services,
        Assembly implementationsAssembly,
        ServiceLifetime lifetime)
    {
        var serviceType = typeof(TService);

        var types = implementationsAssembly.GetTypes()
            .Where(type => type != serviceType &&
                        serviceType.IsAssignableFrom(type) &&
                        type is
                        {
                            IsAbstract: false,
                            IsPublic: true,
                            IsInterface: false
                        });

        services.TryAddEnumerable(types.Select(type => new ServiceDescriptor(serviceType, type, lifetime)));
    }
}
