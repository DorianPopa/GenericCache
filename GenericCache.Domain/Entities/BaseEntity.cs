using Redis.OM.Modeling;

namespace GenericCache.Domain.Entities;

public abstract class BaseEntity<T> where T : notnull
{
    [RedisIdField]
    [Indexed]
    public T Id { get; set; }

    protected BaseEntity(T id)
    {
        Id = id;
    }
}
