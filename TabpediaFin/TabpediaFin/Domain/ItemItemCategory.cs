namespace TabpediaFin.Domain;

public class ItemItemCategory : BaseEntity
{
    public int Id { get; set; }
    public int ItemId { get; set; }
    public int ItemCategoryId { get; set; }
}
