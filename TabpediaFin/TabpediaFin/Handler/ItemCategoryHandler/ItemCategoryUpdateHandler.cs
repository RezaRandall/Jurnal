namespace TabpediaFin.Handler.ItemCategoryHandler;

public class ItemCategoryUpdateHandler : IRequestHandler<ItemCategoryUpdateDto, RowResponse<ItemCategoryFetchDto>>
{
    private readonly FinContext _context;
    private readonly ICurrentUser _currentUser;

    public ItemCategoryUpdateHandler(FinContext db, ICurrentUser currentUser)
    {
        _context = db;
        _currentUser = currentUser;
    }

    public async Task<RowResponse<ItemCategoryFetchDto>> Handle(ItemCategoryUpdateDto request, CancellationToken cancellationToken)
    {
        var result = new RowResponse<ItemCategoryFetchDto>();

        try
        {
            var ItemCategory = await _context.ItemCategory.FirstAsync(x => x.Id == request.Id && x.TenantId == _currentUser.TenantId, cancellationToken);
            ItemCategory.Name = request.Name;
            ItemCategory.Description = request.Description;

            await _context.SaveChangesAsync(cancellationToken);

            var row = new ItemCategoryFetchDto()
            {
                Id = request.Id,
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
