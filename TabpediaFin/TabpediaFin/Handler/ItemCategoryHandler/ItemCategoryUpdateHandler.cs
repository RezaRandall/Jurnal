namespace TabpediaFin.Handler.ItemCategoryHandler;

public class ItemCategoryUpdateHandler : IRequestHandler<ItemCategoryUpdateDto, RowResponse<ItemCategoryFetchDto>>
{
    private readonly FinContext _context;

    public ItemCategoryUpdateHandler(FinContext db)
    {
        _context = db;
    }

    public async Task<RowResponse<ItemCategoryFetchDto>> Handle(ItemCategoryUpdateDto request, CancellationToken cancellationToken)
    {
        var result = new RowResponse<ItemCategoryFetchDto>();

        try
        {
            var ItemCategory = await _context.ItemCategory.FirstAsync(x => x.Id == request.Id, cancellationToken);
            ItemCategory.Name = request.Name;
            ItemCategory.Description = request.Description;

            await _context.SaveChangesAsync(cancellationToken);

            var row = new ItemCategoryFetchDto()
            {
                Name = ItemCategory.Name,
                Description = ItemCategory.Description,
            };

            result.IsOk= true;
            result.ErrorMessage = string.Empty;
            result.Row = row;
        }
        catch (Exception ex)
        {
            result.IsOk= false;
            result.ErrorMessage = ex.Message;
        }

        return result;
    }
}


public class ItemCategoryUpdateDto : IRequest<RowResponse<ItemCategoryFetchDto>>
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;
}
