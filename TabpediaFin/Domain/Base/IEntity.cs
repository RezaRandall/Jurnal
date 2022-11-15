using System.Security.Cryptography;

namespace TabpediaFin.Domain.Base;

public interface IEntity
{

}


public interface IEntity<TId> : IEntity
{
    TId Id { get; }
}
