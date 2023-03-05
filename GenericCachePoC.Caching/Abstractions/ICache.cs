using GenericCachePoC.Domain.Entities;

namespace GenericCachePoC.Caching.Abstractions;

// TODO: Find a way to remove the need of specifying the TKey as this leaks responsability because classes referencing this must also specify it

/// <summary>
/// Generic interface for cache accessor implementations
/// </summary>
/// <typeparam name="TKey"></typeparam>
/// <typeparam name="TValue"></typeparam>
public interface ICache<TKey, TValue>
    where TKey : notnull
    where TValue : BaseEntity<TKey>
{
    /// <summary>
    /// Adds an entry to cache
    /// </summary>
    /// <param name="entity">Entity entry to be added to cache</param>
    /// <returns>Returns true if operation succeded, false otherwise</returns>
    public Task<bool> AddAsync(TValue entity);

    /// <summary>
    /// Removes an entry from cache
    /// </summary>
    /// <param name="entity">Entity entry to be removed from cache</param>
    /// <returns>Returns true if operation succeded, false otherwise</returns>
    public Task<bool> RemoveAsync(TValue entity);

    /// <summary>
    /// Try to get an entry from the cache
    /// </summary>
    /// <param name="key">Key of the stored entry</param>
    /// <returns>The entry corresponding to the key if found, null otherwise</returns>
    public Task<TValue> TryGetByKeyAsync(TKey key);

    /// <summary>
    /// Get all entries stored in the cache
    /// </summary>
    /// <returns>All entries stored in the cache</returns>
    public Task<IEnumerable<TValue>> GetAllAsync();
}
