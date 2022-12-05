namespace TabpediaFin.Domain.Base;

public abstract class BaseEntity<TId> : IEntity<TId>, IHasTenant
{
    public TId Id { get; protected set; } = default!;

    public int TenantId { get; set; }
}


public abstract class BaseEntity : BaseEntity<int>, IAuditableEntity
{
    public int CreatedUid { get; set; }

    public DateTime CreatedUtc { get; set; } = DateTime.UtcNow;

    public int UpdatedUid { get; set; }

    public DateTime? UpdatedUtc { get; set; }
}
