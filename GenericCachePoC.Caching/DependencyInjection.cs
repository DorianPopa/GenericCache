using GenericCachePoC.Caching.Abstractions;
using GenericCachePoC.Caching.Caches;
using GenericCachePoC.Domain.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Redis.OM;
using Redis.OM.Contracts;
using StackExchange.Redis;
using Order = GenericCachePoC.Domain.Entities.Order;

namespace GenericCachePoC.Caching;

/// <summary>
/// Extension methods for setting up the services from the caching layer in an <see cref="IServiceCollection" />.
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    /// Adds services for the caching layer to the specified <see cref="IServiceCollection"/>.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection" /> to add services to.</param>
    /// <param name="configuration">The <see cref="IConfiguration" /> used to access connections strings.</param>
    /// <returns>The <see cref="IServiceCollection"/> that can be used to further configure the required services.</returns>
    public static IServiceCollection AddCacheLayer(this IServiceCollection services, IConfiguration configuration)
    {
        services.SetupRedisConnection(configuration);
        services.SetupCaches();

        return services;
    }

    private static IServiceCollection SetupRedisConnection(this IServiceCollection services, IConfiguration configuration)
    {
        var redisConnectionString = configuration.GetConnectionString("Redis");
        ConfigurationOptions option = new ConfigurationOptions
        {
            EndPoints = { redisConnectionString }
        };
        var connection = new RedisConnectionProvider(option);

        services.AddSingleton<IRedisConnectionProvider>(connection);
        

        return services;
    }

    private static IServiceCollection SetupCaches(this IServiceCollection services)
    {
        services.AddSingleton<ICache<Guid, Order>, OrderCache>();
        services.AddSingleton<ICache<int, Trader>, TraderCache>();

        services.AddHostedService<RedisIndexCreationService>();

        return services;
    }
}
