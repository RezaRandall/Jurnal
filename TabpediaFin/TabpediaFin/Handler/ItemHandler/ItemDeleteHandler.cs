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
            if (itemData == null || itemData.Id == 0)
            {
                throw new HttpException(HttpStatusCode.NotFound, "Data not found");
            }
            _context.Item.Remove(itemData);
            await _context.SaveChangesAsync(cancellationToken);

            // ITEM ATTACHMENT
            List<ItemAttachment> ItemAttachmentList = _context.ItemAttachment.Where<ItemAttachment>(x => x.ItemId == request.Id).ToList();
            if (ItemAttachmentList.Count > 0)
            {
                foreach (ItemAttachment item in ItemAttachmentList)
                {
                    FileInfo file = new FileInfo(item.FileUrl.Replace("https://localhost:7030/", "../TabpediaFin/"));
                    if (file.Exists)
                    {
                        file.Delete();
                    }
                }
                _context.ItemAttachment.RemoveRange(ItemAttachmentList);
                await _context.SaveChangesAsync(cancellationToken);
            }
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

