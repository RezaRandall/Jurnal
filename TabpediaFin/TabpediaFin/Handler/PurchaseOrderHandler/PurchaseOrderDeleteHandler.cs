namespace TabpediaFin.Handler.PurchaseOrderHandler
{
    public class PurchaseOrderDeleteHandler : IDeleteByIdHandler<PurchaseOrderFetchDto>
    {
        private readonly FinContext _context;
        private readonly IPurchaseOrderCacheRemover _cacheRemover;

        public PurchaseOrderDeleteHandler(FinContext db, IPurchaseOrderCacheRemover cacheRemover)
        {
            _context = db;
            _cacheRemover = cacheRemover;
        }

        public async Task<RowResponse<PurchaseOrderFetchDto>> Handle(DeleteByIdRequestDto<PurchaseOrderFetchDto> request, CancellationToken cancellationToken)
        {
            var result = new RowResponse<PurchaseOrderFetchDto>();

            try
            {
                var PurchaseOrder = await _context.PurchaseOrder.FirstAsync(x => x.Id == request.Id, cancellationToken);
                if (PurchaseOrder == null || PurchaseOrder.Id == 0)
                {
                    throw new HttpException(HttpStatusCode.NotFound, "Data not found");
                }

                _context.PurchaseOrder.Remove(PurchaseOrder);
                await _context.SaveChangesAsync(cancellationToken);

                List<PurchaseOrderItem> PurchaseOrderItem = _context.PurchaseOrderItem.Where<PurchaseOrderItem>(x => x.TransId == request.Id).ToList();
                if (PurchaseOrderItem.Count > 0)
                {
                    _context.PurchaseOrderItem.RemoveRange(PurchaseOrderItem);
                    await _context.SaveChangesAsync(cancellationToken);
                }

                List<PurchaseOrderTag> PurchaseOrderTag = _context.PurchaseOrderTag.Where<PurchaseOrderTag>(x => x.TransId == request.Id).ToList();
                if (PurchaseOrderTag.Count > 0)
                {
                    _context.PurchaseOrderTag.RemoveRange(PurchaseOrderTag);
                    await _context.SaveChangesAsync(cancellationToken);
                }

                List<PurchaseOrderAttachment> PurchaseOrderAttachment = _context.PurchaseOrderAttachment.Where<PurchaseOrderAttachment>(x => x.TransId == request.Id).ToList();
                if (PurchaseOrderAttachment.Count > 0)
                {
                    foreach(PurchaseOrderAttachment item in PurchaseOrderAttachment)
                    {
                        FileInfo file = new FileInfo(item.FileUrl.Replace("https://localhost:7030/", "../TabpediaFin/"));
                        if (file.Exists)
                        {
                            file.Delete();
                        }
                    }
                    _context.PurchaseOrderAttachment.RemoveRange(PurchaseOrderAttachment);
                    await _context.SaveChangesAsync(cancellationToken);
                }

                _cacheRemover.RemoveCache();

                result.IsOk = true;
                result.ErrorMessage = string.Empty;
                result.Row = new PurchaseOrderFetchDto();
            }
            catch (Exception ex)
            {
                result.IsOk = false;
                result.ErrorMessage = ex.Message;
            }

            return result;
        }
    }
}
