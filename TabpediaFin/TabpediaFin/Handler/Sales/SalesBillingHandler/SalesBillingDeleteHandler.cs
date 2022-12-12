namespace TabpediaFin.Handler.SalesBillingHandler
{
    public class SalesBillingDeleteHandler : IDeleteByIdHandler<SalesBillingFetchDto>
    {
        private readonly FinContext _context;
        private readonly ISalesBillingCacheRemover _cacheRemover;

        public SalesBillingDeleteHandler(FinContext db, ISalesBillingCacheRemover cacheRemover)
        {
            _context = db;
            _cacheRemover = cacheRemover;
        }

        public async Task<RowResponse<SalesBillingFetchDto>> Handle(DeleteByIdRequestDto<SalesBillingFetchDto> request, CancellationToken cancellationToken)
        {
            var result = new RowResponse<SalesBillingFetchDto>();

            try
            {
                var SalesBilling = await _context.SalesBilling.FirstAsync(x => x.Id == request.Id, cancellationToken);
                if (SalesBilling == null || SalesBilling.Id == 0)
                {
                    throw new HttpException(HttpStatusCode.NotFound, "Data not found");
                }

                _context.SalesBilling.Remove(SalesBilling);
                await _context.SaveChangesAsync(cancellationToken);

                List<SalesBillingItem> SalesBillingItem = _context.SalesBillingItem.Where<SalesBillingItem>(x => x.TransId == request.Id).ToList();
                if (SalesBillingItem.Count > 0)
                {
                    _context.SalesBillingItem.RemoveRange(SalesBillingItem);
                    await _context.SaveChangesAsync(cancellationToken);
                }

                List<SalesBillingTag> SalesBillingTag = _context.SalesBillingTag.Where<SalesBillingTag>(x => x.TransId == request.Id).ToList();
                if (SalesBillingTag.Count > 0)
                {
                    _context.SalesBillingTag.RemoveRange(SalesBillingTag);
                    await _context.SaveChangesAsync(cancellationToken);
                }

                List<SalesBillingAttachment> SalesBillingAttachment = _context.SalesBillingAttachment.Where<SalesBillingAttachment>(x => x.TransId == request.Id).ToList();
                if (SalesBillingAttachment.Count > 0)
                {
                    foreach(SalesBillingAttachment item in SalesBillingAttachment)
                    {
                        FileInfo file = new FileInfo(item.FileUrl.Replace("https://localhost:7030/", "../TabpediaFin/"));
                        if (file.Exists)
                        {
                            file.Delete();
                        }
                    }
                    _context.SalesBillingAttachment.RemoveRange(SalesBillingAttachment);
                    await _context.SaveChangesAsync(cancellationToken);
                }

                _cacheRemover.RemoveCache();

                result.IsOk = true;
                result.ErrorMessage = string.Empty;
                result.Row = new SalesBillingFetchDto();
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
