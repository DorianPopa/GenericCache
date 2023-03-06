using GenericCache.Domain.Entities;
using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;

namespace GenericCache.Caching.Abstractions;

/// <summary>
/// Generic Dictionary backed, in-memory cache implementation that supports the operations of ICache
/// </summary>
/// <typeparam name="TKey">Type of BaseEntity Id</typeparam>
/// <typeparam name="TValue">Type of stored Entity object</typeparam>
public abstract class InMemoryCache<TKey, TValue> : ICache<TKey, TValue>
    where TKey : notnull
    where TValue : BaseEntity<TKey>
{
    private readonly ILogger<InMemoryCache<TKey, TValue>> _logger;

    private readonly ConcurrentDictionary<TKey, TValue> _cache = new();

    protected InMemoryCache(ILogger<InMemoryCache<TKey, TValue>> logger)
    {
        _logger = logger;
    }

    /// <inheritdoc/>
    public async Task<bool> AddAsync(TValue entity)
    {
        var result = await Task.Run(() =>
            _cache.TryAdd(entity.Id, entity)
        );
        return result;
    }

    /// <inheritdoc/>
    public async Task<bool> RemoveAsync(TValue entity)
    {
        var result = await Task.Run(() =>
            _cache.TryRemove(entity.Id, out _)
        );
        return result;
    }

    /// <inheritdoc/>
    public async Task<TValue> TryGetByKeyAsync(TKey key)
    {
        var result = await Task.Run(() =>
        {
            _cache.TryGetValue(key, out TValue cacheEntity);
            return cacheEntity;
        });
        return result;
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<TValue>> GetAllAsync()
    {
        var result = await Task.Run(() =>
            _cache.Values.ToList()
        );
        return result;
    }
}
