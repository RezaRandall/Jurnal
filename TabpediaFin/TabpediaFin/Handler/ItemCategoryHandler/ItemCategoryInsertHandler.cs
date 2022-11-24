using TabpediaFin.Handler.ItemCategoryHandler;

namespace TabpediaFin.Handler.ItemCategoryHandler;

public class ItemCategoryInsertHandler : IRequestHandler<ItemCategoryInsertDto, RowResponse<ItemCategoryFetchDto>>
{
    private readonly FinContext _context;

    public ItemCategoryInsertHandler(FinContext db)
    {
        _context = db;
    }

    public async Task<RowResponse<ItemCategoryFetchDto>> Handle(ItemCategoryInsertDto request, CancellationToken cancellationToken)
    {
        var result = new RowResponse<ItemCategoryFetchDto>();

        var ItemCategory = new ItemCategory()
        {
            Name = request.Name,
            Description = request.Description,
        };

        try
        {
            await _context.ItemCategory.AddAsync(ItemCategory, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            var row = new ItemCategoryFetchDto()
            {
                Id = ItemCategory.Id,
                Name = ItemCategory.Name,
                Description = ItemCategory.Description,
            };

            result.IsOk = true;
            result.ErrorMessage = string.Empty;
            result.Row = row;
        }
        catch (Exception ex)
        {
            result.IsOk = false;
            result.ErrorMessage = ex.Message;
        }

        return result;
    }
}



public class ItemCategoryInsertDto : IRequest<RowResponse<ItemCategoryFetchDto>>
{
    public string Name { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

}
