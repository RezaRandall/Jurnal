namespace TabpediaFin.Domain.Base;

public interface IAuditableEntity
{
    public int CreatedUid { get; set; }
    public DateTime CreatedUtc { get; }
    public int UpdatedUid { get; set; }
    public DateTime? UpdatedUtc { get; set; }
}
