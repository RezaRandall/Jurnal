using TabpediaFin.Handler.ContactAddressHandler;

namespace TabpediaFin.Handler.ItemItemCategoryHandler;

public class ItemItemCategoryUpdateHandler
{
}
public class ItemItemCategoryUpdateDto : IRequest<RowResponse<ItemItemCategoryFetchDto>>
{
    public int Id { get; set; } = 0;
    public int ItemId { get; set; } = 0;
    public int ItemCategoryId { get; set; } = 0;
}
