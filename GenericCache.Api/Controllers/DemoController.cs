using GenericCache.Caching.Abstractions;
using GenericCache.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace GenericCache.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DemoController : ControllerBase
{
    private readonly ILogger<DemoController> _logger;

    private readonly ICache<Guid, Order> _orderCache;
    private readonly ICache<int, Trader> _traderCache;

    public DemoController(ILogger<DemoController> logger, ICache<Guid, Order> orderCache, ICache<int, Trader> traderCache)
    {
        _logger = logger;
        _orderCache = orderCache;
        _traderCache = traderCache;
    }

    [HttpGet("getOrders")]
    public async Task<IActionResult> GetOrders()
    {
        var result = await _orderCache.GetAllAsync();

        return Ok(result);
    }

    [HttpGet("postOrder")]
    public async Task<IActionResult> CreateOrder()
    {
        var newOrder = new Order(Guid.NewGuid()) { TradeableItem = "TESLA 5Y", Price = 1337 };
        await _orderCache.AddAsync(newOrder);

        var result = await _orderCache.TryGetByKeyAsync(newOrder.Id);

        return Ok(result);
    }

    [HttpGet("getTraders")]
    public async Task<IActionResult> GetTraders()
    {
        var result = await _traderCache.GetAllAsync();

        return Ok(result);
    }

    [HttpGet("postTrader")]
    public async Task<IActionResult> CreateTrader()
    {
        var newId = new Random().Next(0, int.MaxValue);
        var newTrader = new Trader(newId) { Name = $"Johny-{newId}" };

        await _traderCache.AddAsync(newTrader);

        var result = await _traderCache.TryGetByKeyAsync(newTrader.Id);

        return Ok(result);
    }
}