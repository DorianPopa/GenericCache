using GenericCache.Domain.Entities;
using Microsoft.Extensions.Hosting;
using Redis.OM;
using Redis.OM.Contracts;

namespace GenericCache.Caching;

public class RedisIndexCreationService : IHostedService
{
    private readonly IRedisConnectionProvider _provider;

    public RedisIndexCreationService(IRedisConnectionProvider provider)
    {
        _provider = provider;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        //await _provider.Connection.CreateIndexAsync(typeof(Trader));
        await _provider.Connection.CreateIndexAsync(typeof(Order));
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}
