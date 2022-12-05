namespace TabpediaFin.Domain.Base;

public interface IEntity
{

}


public interface IEntity<TId> : IEntity
{
    TId Id { get; }
}


public interface IHasTenant
{
    int TenantId { get; }
}
