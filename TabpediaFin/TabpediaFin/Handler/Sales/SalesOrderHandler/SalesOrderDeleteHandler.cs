namespace TabpediaFin.Handler.SalesOrderHandler
{
    public class SalesOrderDeleteHandler : IDeleteByIdHandler<SalesOrderFetchDto>
    {
        private readonly FinContext _context;
        private readonly ISalesOrderCacheRemover _cacheRemover;

        public SalesOrderDeleteHandler(FinContext db, ISalesOrderCacheRemover cacheRemover)
        {
            _context = db;
            _cacheRemover = cacheRemover;
        }

        public async Task<RowResponse<SalesOrderFetchDto>> Handle(DeleteByIdRequestDto<SalesOrderFetchDto> request, CancellationToken cancellationToken)
        {
            var result = new RowResponse<SalesOrderFetchDto>();

            try
            {
                var SalesOrder = await _context.SalesOrder.FirstAsync(x => x.Id == request.Id, cancellationToken);
                if (SalesOrder == null || SalesOrder.Id == 0)
                {
                    throw new HttpException(HttpStatusCode.NotFound, "Data not found");
                }

                _context.SalesOrder.Remove(SalesOrder);
                await _context.SaveChangesAsync(cancellationToken);

                List<SalesOrderItem> SalesOrderItem = _context.SalesOrderItem.Where<SalesOrderItem>(x => x.TransId == request.Id).ToList();
                if (SalesOrderItem.Count > 0)
                {
                    _context.SalesOrderItem.RemoveRange(SalesOrderItem);
                    await _context.SaveChangesAsync(cancellationToken);
                }

                List<SalesOrderTag> SalesOrderTag = _context.SalesOrderTag.Where<SalesOrderTag>(x => x.TransId == request.Id).ToList();
                if (SalesOrderTag.Count > 0)
                {
                    _context.SalesOrderTag.RemoveRange(SalesOrderTag);
                    await _context.SaveChangesAsync(cancellationToken);
                }

                List<SalesOrderAttachment> SalesOrderAttachment = _context.SalesOrderAttachment.Where<SalesOrderAttachment>(x => x.TransId == request.Id).ToList();
                if (SalesOrderAttachment.Count > 0)
                {
                    foreach(SalesOrderAttachment item in SalesOrderAttachment)
                    {
                        FileInfo file = new FileInfo(item.FileUrl.Replace("https://localhost:7030/", "../TabpediaFin/"));
                        if (file.Exists)
                        {
                            file.Delete();
                        }
                    }
                    _context.SalesOrderAttachment.RemoveRange(SalesOrderAttachment);
                    await _context.SaveChangesAsync(cancellationToken);
                }

                _cacheRemover.RemoveCache();

                result.IsOk = true;
                result.ErrorMessage = string.Empty;
                result.Row = new SalesOrderFetchDto();
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
