using TabpediaFin.Handler.ContactAddressHandler;

namespace TabpediaFin.Handler.ItemItemCategoryHandler;

public class ItemItemCategoryInsertHandler
{

}

public class ItemItemCategoryInsertDto : IRequest<RowResponse<ItemItemCategoryFetchDto>>
{
    public int Id { get; set; } = 0;
    public int ItemCategoryId { get; set; } = 0;

}
