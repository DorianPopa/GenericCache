using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Redis.OM.Contracts;
using Redis.OM.Searching;

namespace GenericCachePoc.Caching.Test.Caches;

public class OrderCacheTests
{
    private readonly ILogger<OrderCache> _logger;
    private readonly Mock<IRedisConnectionProvider> _connectionProvider;

    private readonly Mock<IRedisCollection<Order>> redisCollectionMock;

    private readonly OrderCache _orderCache;

    // SetUp
    public OrderCacheTests()
    {
        // Setup SUT dependencies
        _logger = new NullLogger<OrderCache>();
        _connectionProvider = new Mock<IRedisConnectionProvider>();

        // Setup mock behaviours
        redisCollectionMock = new Mock<IRedisCollection<Order>>();
        _connectionProvider.Setup(x => x.RedisCollection<Order>(It.IsAny<int>()))
            .Returns(redisCollectionMock.Object);

        // Setup SUT
        _orderCache = new OrderCache(_logger, _connectionProvider.Object);
    }

    [Fact]
    public async void AddAsync_ReturnsTrue_WhenCacheInsertSucceeds()
    {
        // Arrange
        var entity = new Order(Guid.NewGuid());

        // Act
        var result = await _orderCache.AddAsync(entity);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async void AddAsync_ReturnsFalse_WhenCacheInsertThrows()
    {
        // Arrange
        var entity = new Order(Guid.NewGuid());
        redisCollectionMock.Setup(x => x.InsertAsync(entity)).ThrowsAsync(new Exception());

        // Act
        var result = await _orderCache.AddAsync(entity);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async void GetAllAsync_ReturnsAllCacheEntries_WhenToListAsyncSucceeds()
    {
        // Arrange
        var order1 = new Order(Guid.NewGuid());
        var order2 = new Order(Guid.NewGuid());
        var entities = new List<Order> { order1, order2 };
        redisCollectionMock.Setup(x => x.ToListAsync())
            .ReturnsAsync(entities);

        // Act
        var result = await _orderCache.GetAllAsync();

        // Assert
        Assert.Equal(2, result.Count());
        Assert.Equal(order1.Id, result.First().Id);
        Assert.Equal(order2.Id, result.Last().Id);
    }

    [Fact]
    public async void GetAllAsync_ReturnsEmptyList_WhenToListAsyncThrows()
    {
        // Arrange
        redisCollectionMock.Setup(x => x.ToListAsync()).ThrowsAsync(new Exception());

        // Act
        var result = await _orderCache.GetAllAsync();

        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public async void RemoveAsync_ReturnsTrue_WhenDeleteAsyncSucceeds()
    {
        // Arrange
        var entity = new Order(Guid.NewGuid());

        // Act
        var result = await _orderCache.RemoveAsync(entity);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async void RemoveAsync_ReturnsFalse_WhenDeleteAsyncThrows()
    {
        // Arrange
        var entity = new Order(Guid.NewGuid());
        redisCollectionMock.Setup(x => x.DeleteAsync(entity)).ThrowsAsync(new Exception());

        // Act
        var result = await _orderCache.RemoveAsync(entity);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async void TryGetByKeyAsync_ReturnsEntity_WhenFindByIdAsyncSucceeds()
    {
        // Arrange
        var entity = new Order(Guid.NewGuid());
        redisCollectionMock.Setup(x => x.FindByIdAsync(entity.Id.ToString())).ReturnsAsync(entity);

        // Act
        var result = await _orderCache.TryGetByKeyAsync(entity.Id);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(entity.Id, result.Id);
    }

    [Fact]
    public async void TryGetByKeyAsync_ReturnsNull_WhenFindByIdAsyncThrows()
    {
        // Arrange
        var entity = new Order(Guid.NewGuid());
        redisCollectionMock.Setup(x => x.FindByIdAsync(entity.Id.ToString())).ThrowsAsync(new Exception());

        // Act
        var result = await _orderCache.TryGetByKeyAsync(entity.Id);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async void TryGetByKeyAsync_ReturnsNull_WhenFindByIdAsyncDoesNotFindEntry()
    {
        // Arrange
        var entity = new Order(Guid.NewGuid());
        Order returnedEntity = null!;
        redisCollectionMock.Setup(x => x.FindByIdAsync(entity.Id.ToString())).ReturnsAsync(returnedEntity);

        // Act
        var result = await _orderCache.TryGetByKeyAsync(entity.Id);

        // Assert
        Assert.Null(result);
    }
}
