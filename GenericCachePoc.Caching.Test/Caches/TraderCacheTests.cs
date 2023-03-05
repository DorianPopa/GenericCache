using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace GenericCachePoc.Caching.Test.Caches;

public class TraderCacheTests
{
    private readonly ILogger<TraderCache> _logger;

    private readonly TraderCache _cache;

    public TraderCacheTests()
    {
        // Setup SUT dependencies
        _logger = new NullLogger<TraderCache>();

        // Setup SUT
        _cache = new TraderCache(_logger);
    }

    [Fact]
    public async void AddAsync_ReturnsTrue_WhenEntityIsInsertedIntoCache()
    {
        // Arrange
        var entity = new Trader(1337);

        // Act
        var result = await _cache.AddAsync(entity);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async void AddAsync_ReturnsFalse_WhenEntityNotInsertedIntoCache()
    {
        // Arrange
        var entity = new Trader(1337);
        await _cache.AddAsync(entity);

        // Act
        var result = await _cache.AddAsync(entity);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async void RemoveAsync_ReturnsTrue_WhenEntityIsRemovedFromCache()
    {
        // Arrange
        var entity = new Trader(1337);
        await _cache.AddAsync(entity);

        // Act
        var result = await _cache.RemoveAsync(entity);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async void RemoveAsync_ReturnsFalse_WhenEntityNotRemovedFromCache()
    {
        // Arrange
        var entity = new Trader(1337);

        // Act
        var result = await _cache.RemoveAsync(entity);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async void TryGetByKeyAsync_ReturnsEntity_WhenEntityIsFoundInCache()
    {
        // Arrange
        var entity = new Trader(1337);
        await _cache.AddAsync(entity);

        // Act
        var result = await _cache.TryGetByKeyAsync(entity.Id);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(entity.Id, result.Id);
    }

    [Fact]
    public async void TryGetByKeyAsync_ReturnsNull_WhenEntityIsNotFoundInCache()
    {
        // Arrange
        var entity = new Trader(1337);

        // Act
        var result = await _cache.TryGetByKeyAsync(entity.Id);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async void GetAllAsync_ReturnsAllEntities_WhenCacheContainsEntities()
    {
        // Arrange
        var entity1 = new Trader(1337);
        var entity2 = new Trader(1338);
        await _cache.AddAsync(entity1);
        await _cache.AddAsync(entity2);

        // Act
        var result = await _cache.GetAllAsync();

        // Assert
        Assert.Equal(2, result.Count());
        Assert.Equal(entity1.Id, result.First().Id);
        Assert.Equal(entity2.Id, result.Last().Id);
    }

    [Fact]
    public async void GetAllAsync_ReturnsNull_WhenEntityIsNotFoundInCache()
    {
        // Arrange

        // Act
        var result = await _cache.GetAllAsync();

        // Assert
        Assert.Empty(result);
    }
}
