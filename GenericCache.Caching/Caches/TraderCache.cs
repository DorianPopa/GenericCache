using GenericCache.Caching.Abstractions;
using GenericCache.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace GenericCache.Caching.Caches;

public sealed class TraderCache : InMemoryCache<int, Trader>
{
    public TraderCache(ILogger<InMemoryCache<int, Trader>> logger)
        : base(logger)
    {
    }
}
