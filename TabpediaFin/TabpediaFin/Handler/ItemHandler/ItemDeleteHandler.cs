namespace TabpediaFin.Handler.Item;

public class ItemDeleteHandler : IRequestHandler<ItemDeleteDto, RowResponse<bool>>
{
    private readonly FinContext _context;
    private readonly ICurrentUser _currentUser;

    public ItemDeleteHandler(FinContext db, ICurrentUser currentUser)
    {
        _context = db;
        _currentUser = currentUser;
    }

    public async Task<RowResponse<bool>> Handle(ItemDeleteDto request, CancellationToken cancellationToken)
    {
        var result = new RowResponse<bool>();
        try
        {
            var itemData = await _context.Item.FirstOrDefaultAsync(x => x.Id == request.Id && x.TenantId == _currentUser.TenantId, cancellationToken);
            if (itemData != null)
            {
                _context.Item.Remove(itemData);
                result.IsOk = true;
                result.ErrorMessage = "Item with id " + request.Id + " has been deleted";
            }
            if (itemData == null)
            {
                result.IsOk = false;
                result.ErrorMessage = "Data not found";
            }
                await _context.SaveChangesAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            result.IsOk = false;
            result.ErrorMessage = ex.Message;
        }

        return result;
    }
}

[Table("Item")]
public class ItemDeleteDto : IRequest<RowResponse<bool>>
{
    public int Id { get; set; } = 0;
    public int TenantId { get; set; } = 0;
}

