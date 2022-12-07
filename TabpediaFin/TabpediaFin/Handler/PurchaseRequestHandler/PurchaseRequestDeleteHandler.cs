namespace TabpediaFin.Handler.PurchaseRequestHandler
{
    public class PurchaseRequestDeleteHandler : IDeleteByIdHandler<PurchaseRequestFetchDto>
    {
        private readonly FinContext _context;
        private readonly IPurchaseRequestCacheRemover _cacheRemover;

        public PurchaseRequestDeleteHandler(FinContext db, IPurchaseRequestCacheRemover cacheRemover)
        {
            _context = db;
            _cacheRemover = cacheRemover;
        }

        public async Task<RowResponse<PurchaseRequestFetchDto>> Handle(DeleteByIdRequestDto<PurchaseRequestFetchDto> request, CancellationToken cancellationToken)
        {
            var result = new RowResponse<PurchaseRequestFetchDto>();

            try
            {
                var PurchaseRequest = await _context.PurchaseRequest.FirstAsync(x => x.Id == request.Id, cancellationToken);
                if (PurchaseRequest == null || PurchaseRequest.Id == 0)
                {
                    throw new HttpException(HttpStatusCode.NotFound, "Data not found");
                }

                _context.PurchaseRequest.Remove(PurchaseRequest);
                await _context.SaveChangesAsync(cancellationToken);

                List<PurchaseRequestItem> PurchaseRequestItem = _context.PurchaseRequestItem.Where<PurchaseRequestItem>(x => x.TransId == request.Id).ToList();
                if (PurchaseRequestItem.Count > 0)
                {
                    _context.PurchaseRequestItem.RemoveRange(PurchaseRequestItem);
                    await _context.SaveChangesAsync(cancellationToken);
                }

                List<PurchaseRequestTag> PurchaseRequestTag = _context.PurchaseRequestTag.Where<PurchaseRequestTag>(x => x.TransId == request.Id).ToList();
                if (PurchaseRequestTag.Count > 0)
                {
                    _context.PurchaseRequestTag.RemoveRange(PurchaseRequestTag);
                    await _context.SaveChangesAsync(cancellationToken);
                }

                List<PurchaseRequestAttachment> PurchaseRequestAttachment = _context.PurchaseRequestAttachment.Where<PurchaseRequestAttachment>(x => x.TransId == request.Id).ToList();
                if (PurchaseRequestAttachment.Count > 0)
                {
                    foreach(PurchaseRequestAttachment item in PurchaseRequestAttachment)
                    {
                        FileInfo file = new FileInfo(item.FileUrl.Replace("https://localhost:7030/", "../TabpediaFin/"));
                        if (file.Exists)
                        {
                            file.Delete();
                        }
                    }
                    _context.PurchaseRequestAttachment.RemoveRange(PurchaseRequestAttachment);
                    await _context.SaveChangesAsync(cancellationToken);
                }

                _cacheRemover.RemoveCache();

                result.IsOk = true;
                result.ErrorMessage = string.Empty;
                result.Row = new PurchaseRequestFetchDto();
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
