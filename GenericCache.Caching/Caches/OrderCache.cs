using GenericCache.Caching.Abstractions;
using GenericCache.Domain.Entities;
using Microsoft.Extensions.Logging;
using Redis.OM.Contracts;

namespace GenericCache.Caching.Caches;

public sealed class OrderCache : RedisCache<Guid, Order>
{
    public OrderCache(ILogger<RedisCache<Guid, Order>> logger, IRedisConnectionProvider redisConnectionProvider)
        : base(logger, redisConnectionProvider)
    {
    }
}
