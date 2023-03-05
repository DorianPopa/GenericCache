using GenericCachePoC.Caching.Abstractions;
using GenericCachePoC.Domain.Entities;
using Microsoft.Extensions.Logging;
using Redis.OM.Contracts;

namespace GenericCachePoC.Caching.Caches;

public sealed class OrderCache : RedisCache<Guid, Order>
{
    public OrderCache(ILogger<RedisCache<Guid, Order>> logger, IRedisConnectionProvider redisConnectionProvider)
        : base(logger, redisConnectionProvider)
    {
    }
}
