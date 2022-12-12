namespace TabpediaFin.Handler.PurchaseBillingHandler
{
    public class PurchaseBillingDeleteHandler : IDeleteByIdHandler<PurchaseBillingFetchDto>
    {
        private readonly FinContext _context;
        private readonly IPurchaseBillingCacheRemover _cacheRemover;

        public PurchaseBillingDeleteHandler(FinContext db, IPurchaseBillingCacheRemover cacheRemover)
        {
            _context = db;
            _cacheRemover = cacheRemover;
        }

        public async Task<RowResponse<PurchaseBillingFetchDto>> Handle(DeleteByIdRequestDto<PurchaseBillingFetchDto> request, CancellationToken cancellationToken)
        {
            var result = new RowResponse<PurchaseBillingFetchDto>();

            try
            {
                var PurchaseBilling = await _context.PurchaseBilling.FirstAsync(x => x.Id == request.Id, cancellationToken);
                if (PurchaseBilling == null || PurchaseBilling.Id == 0)
                {
                    throw new HttpException(HttpStatusCode.NotFound, "Data not found");
                }

                _context.PurchaseBilling.Remove(PurchaseBilling);
                await _context.SaveChangesAsync(cancellationToken);

                List<PurchaseBillingItem> PurchaseBillingItem = _context.PurchaseBillingItem.Where<PurchaseBillingItem>(x => x.TransId == request.Id).ToList();
                if (PurchaseBillingItem.Count > 0)
                {
                    _context.PurchaseBillingItem.RemoveRange(PurchaseBillingItem);
                    await _context.SaveChangesAsync(cancellationToken);
                }

                List<PurchaseBillingTag> PurchaseBillingTag = _context.PurchaseBillingTag.Where<PurchaseBillingTag>(x => x.TransId == request.Id).ToList();
                if (PurchaseBillingTag.Count > 0)
                {
                    _context.PurchaseBillingTag.RemoveRange(PurchaseBillingTag);
                    await _context.SaveChangesAsync(cancellationToken);
                }

                List<PurchaseBillingAttachment> PurchaseBillingAttachment = _context.PurchaseBillingAttachment.Where<PurchaseBillingAttachment>(x => x.TransId == request.Id).ToList();
                if (PurchaseBillingAttachment.Count > 0)
                {
                    foreach(PurchaseBillingAttachment item in PurchaseBillingAttachment)
                    {
                        FileInfo file = new FileInfo(item.FileUrl.Replace("https://localhost:7030/", "../TabpediaFin/"));
                        if (file.Exists)
                        {
                            file.Delete();
                        }
                    }
                    _context.PurchaseBillingAttachment.RemoveRange(PurchaseBillingAttachment);
                    await _context.SaveChangesAsync(cancellationToken);
                }

                _cacheRemover.RemoveCache();

                result.IsOk = true;
                result.ErrorMessage = string.Empty;
                result.Row = new PurchaseBillingFetchDto();
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
