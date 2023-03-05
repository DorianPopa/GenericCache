using GenericCachePoC.Caching.Abstractions;
using GenericCachePoC.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace GenericCachePoC.Caching.Caches;

public sealed class TraderCache : InMemoryCache<int, Trader>
{
    public TraderCache(ILogger<InMemoryCache<int, Trader>> logger) 
        : base(logger)
    {
    }
}
