using GenericCachePoC.Caching;
using GenericCachePoC.Caching.Abstractions;
using GenericCachePoC.Caching.Caches;
using GenericCachePoC.Domain.Entities;
using Redis.OM;
using Redis.OM.Contracts;
using StackExchange.Redis;
using Order = GenericCachePoC.Domain.Entities.Order;

namespace GenericCachePoC;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddControllers();

        builder.Services.SetupDIContainer();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}

public static class ServiceExtensions
{
    public static IServiceCollection SetupDIContainer(this IServiceCollection services)
    {
        services.SetupRedis();
        services.SetupCaches();

        return services;
    }

    private static IServiceCollection SetupRedis(this IServiceCollection services)
    {
        // Configuration for the Redis connection
        ConfigurationOptions option = new ConfigurationOptions
        {
            EndPoints = { "127.0.0.1:6379" }
        };
        var connection = new RedisConnectionProvider(option);

        services.AddSingleton<IRedisConnectionProvider>(connection);
        services.AddHostedService<RedisIndexCreationService>();

        return services;
    }

    private static IServiceCollection SetupCaches(this IServiceCollection services)
    {
        services.AddSingleton<ICache<Guid, Order>, OrderCache>();
        services.AddSingleton<ICache<int, Trader>, TraderCache>();

        return services;
    }
}