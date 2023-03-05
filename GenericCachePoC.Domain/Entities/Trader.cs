using Redis.OM.Modeling;

namespace GenericCachePoC.Domain.Entities
{
    [Document(StorageType = StorageType.Json, Prefixes = new[] { "Trader" })]
    public sealed class Trader : BaseEntity<int>
    {
        [Indexed]
        public string? Name { get; set; }

        public Trader(int id) : base(id)
        {
        }
    }
}
