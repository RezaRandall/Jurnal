namespace TabpediaFin.Handler.ItemItemCategoryHandler;

public class ItemItemCategoryFetchHandler
{
}

[Table("ItemItemCategory")]
public class ItemItemCategoryFetchDto : BaseDto
{
    public int ItemId { get; set; } = 0;
    public int ItemCategoryId { get; set; } = 0;
}
