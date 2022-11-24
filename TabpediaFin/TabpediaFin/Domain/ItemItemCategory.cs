namespace TabpediaFin.Domain;

public class ItemItemCategory : BaseEntity
{
    public int Id { get; set; } = 0;
    public int ItemId { get; set; } = 0;
    public int ItemCategoryId { get; set; } = 0;
}
