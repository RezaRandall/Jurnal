namespace TabpediaFin.Domain.Base;

public interface IAuditableEntity
{
    int CreatedUid { get; set; }
    DateTime CreatedUtc { get; }
    int UpdatedUid { get; set; }
    DateTime? UpdatedUtc { get; set; }
}
