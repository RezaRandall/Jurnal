namespace TabpediaFin.Handler.Item;

public class ItemDeleteHandler : IDeleteByIdHandler<ItemDto>
{
    private readonly FinContext _context;
    private readonly IPaymentMethodCacheRemover _cacheRemover;

    public ItemDeleteHandler(FinContext db, IPaymentMethodCacheRemover cacheRemover)
    {
        _context = db;
        _cacheRemover = cacheRemover;
    }

    public async Task<RowResponse<ItemDto>> Handle(DeleteByIdRequestDto<ItemDto> request, CancellationToken cancellationToken)
    {
        var result = new RowResponse<ItemDto>();
        try
        {
            var itemData = await _context.Item.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
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
                _cacheRemover.RemoveCache();
        }
        catch (Exception ex)
        {
            result.IsOk = false;
            result.ErrorMessage = ex.Message;
        }

        return result;
    }
}

//[Table("Item")]
//public class ItemDeleteDto : IRequest<RowResponse<bool>>
//{
//    public int Id { get; set; } = 0;
//}

