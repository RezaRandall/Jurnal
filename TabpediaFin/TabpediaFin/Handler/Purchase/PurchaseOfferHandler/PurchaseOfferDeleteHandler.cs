namespace TabpediaFin.Handler.PurchaseOfferHandler
{
    public class PurchaseOfferDeleteHandler : IDeleteByIdHandler<PurchaseOfferFetchDto>
    {
        private readonly FinContext _context;
        private readonly IPurchaseOfferCacheRemover _cacheRemover;

        public PurchaseOfferDeleteHandler(FinContext db, IPurchaseOfferCacheRemover cacheRemover)
        {
            _context = db;
            _cacheRemover = cacheRemover;
        }

        public async Task<RowResponse<PurchaseOfferFetchDto>> Handle(DeleteByIdRequestDto<PurchaseOfferFetchDto> request, CancellationToken cancellationToken)
        {
            var result = new RowResponse<PurchaseOfferFetchDto>();

            try
            {
                var PurchaseOffer = await _context.PurchaseOffer.FirstAsync(x => x.Id == request.Id, cancellationToken);
                if (PurchaseOffer == null || PurchaseOffer.Id == 0)
                {
                    throw new HttpException(HttpStatusCode.NotFound, "Data not found");
                }

                _context.PurchaseOffer.Remove(PurchaseOffer);
                await _context.SaveChangesAsync(cancellationToken);

                List<PurchaseOfferItem> PurchaseOfferItem = _context.PurchaseOfferItem.Where<PurchaseOfferItem>(x => x.TransId == request.Id).ToList();
                if (PurchaseOfferItem.Count > 0)
                {
                    _context.PurchaseOfferItem.RemoveRange(PurchaseOfferItem);
                    await _context.SaveChangesAsync(cancellationToken);
                }

                List<PurchaseOfferTag> PurchaseOfferTag = _context.PurchaseOfferTag.Where<PurchaseOfferTag>(x => x.TransId == request.Id).ToList();
                if (PurchaseOfferTag.Count > 0)
                {
                    _context.PurchaseOfferTag.RemoveRange(PurchaseOfferTag);
                    await _context.SaveChangesAsync(cancellationToken);
                }

                List<PurchaseOfferAttachment> PurchaseOfferAttachment = _context.PurchaseOfferAttachment.Where<PurchaseOfferAttachment>(x => x.TransId == request.Id).ToList();
                if (PurchaseOfferAttachment.Count > 0)
                {
                    foreach(PurchaseOfferAttachment item in PurchaseOfferAttachment)
                    {
                        FileInfo file = new FileInfo(item.FileUrl.Replace("https://localhost:7030/", "../TabpediaFin/"));
                        if (file.Exists)
                        {
                            file.Delete();
                        }
                    }
                    _context.PurchaseOfferAttachment.RemoveRange(PurchaseOfferAttachment);
                    await _context.SaveChangesAsync(cancellationToken);
                }

                _cacheRemover.RemoveCache();

                result.IsOk = true;
                result.ErrorMessage = string.Empty;
                result.Row = new PurchaseOfferFetchDto();
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
