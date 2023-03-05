using GenericCachePoC.Domain.Entities;
using Microsoft.Extensions.Logging;
using Redis.OM.Contracts;
using Redis.OM.Searching;

namespace GenericCachePoC.Caching.Abstractions;

/// <summary>
/// Generic Redis Cache implementation that supports the operations of ICache
/// </summary>
/// <typeparam name="TKey">Type of BaseEntity Id</typeparam>
/// <typeparam name="TValue">Type of stored Entity object</typeparam>
public abstract class RedisCache<TKey, TValue> : ICache<TKey, TValue>
    where TKey : notnull
    where TValue : BaseEntity<TKey>
{
    private readonly ILogger<RedisCache<TKey, TValue>> _logger;
    private readonly IRedisConnectionProvider _redisConnectionProvider;

    private readonly IRedisCollection<TValue> _cache;

    protected RedisCache(ILogger<RedisCache<TKey, TValue>> logger, IRedisConnectionProvider redisConnectionProvider)
    {
        _logger = logger;
        _redisConnectionProvider = redisConnectionProvider;

        _cache = _redisConnectionProvider.RedisCollection<TValue>();
    }

    /// <inheritdoc/>
    public async Task<bool> AddAsync(TValue entity)
    {
        try
        {
            _ = await _cache.InsertAsync(entity);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return false;
        }
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<TValue>> GetAllAsync()
    {
        try
        {
            var result = await _cache.ToListAsync();
            return result;
        }
        catch (Exception ex) 
        {
            _logger.LogError(ex.Message);
            return new List<TValue>();
        }
    }

    /// <inheritdoc/>
    public async Task<bool> RemoveAsync(TValue entity)
    {
        try
        {
            await _cache.DeleteAsync(entity);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return false;
        }
    }

    /// <inheritdoc/>
    public async Task<TValue> TryGetByKeyAsync(TKey key)
    {
        try
        {
            var result = await _cache.FindByIdAsync(key.ToString());
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return null;
        }
    }
}
