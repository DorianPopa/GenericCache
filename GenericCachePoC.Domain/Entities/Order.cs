using Redis.OM.Modeling;

namespace GenericCachePoC.Domain.Entities;

[Document(StorageType = StorageType.Json, Prefixes = new[] { "Order" })]
public sealed class Order : BaseEntity<Guid>
{
    [Indexed]
    public int Price { get; set; }

    [Indexed]
    public string? TradeableItem { get; set; }

    public Order(Guid id) : base(id)
    {
    }
}
