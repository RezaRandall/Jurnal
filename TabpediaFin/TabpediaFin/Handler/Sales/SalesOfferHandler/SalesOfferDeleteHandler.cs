namespace TabpediaFin.Handler.SalesOfferHandler
{
    public class SalesOfferDeleteHandler : IDeleteByIdHandler<SalesOfferFetchDto>
    {
        private readonly FinContext _context;
        private readonly ISalesOfferCacheRemover _cacheRemover;

        public SalesOfferDeleteHandler(FinContext db, ISalesOfferCacheRemover cacheRemover)
        {
            _context = db;
            _cacheRemover = cacheRemover;
        }

        public async Task<RowResponse<SalesOfferFetchDto>> Handle(DeleteByIdRequestDto<SalesOfferFetchDto> request, CancellationToken cancellationToken)
        {
            var result = new RowResponse<SalesOfferFetchDto>();

            try
            {
                var SalesOffer = await _context.SalesOffer.FirstAsync(x => x.Id == request.Id, cancellationToken);
                if (SalesOffer == null || SalesOffer.Id == 0)
                {
                    throw new HttpException(HttpStatusCode.NotFound, "Data not found");
                }

                _context.SalesOffer.Remove(SalesOffer);
                await _context.SaveChangesAsync(cancellationToken);

                List<SalesOfferItem> SalesOfferItem = _context.SalesOfferItem.Where<SalesOfferItem>(x => x.TransId == request.Id).ToList();
                if (SalesOfferItem.Count > 0)
                {
                    _context.SalesOfferItem.RemoveRange(SalesOfferItem);
                    await _context.SaveChangesAsync(cancellationToken);
                }

                List<SalesOfferTag> SalesOfferTag = _context.SalesOfferTag.Where<SalesOfferTag>(x => x.TransId == request.Id).ToList();
                if (SalesOfferTag.Count > 0)
                {
                    _context.SalesOfferTag.RemoveRange(SalesOfferTag);
                    await _context.SaveChangesAsync(cancellationToken);
                }

                List<SalesOfferAttachment> SalesOfferAttachment = _context.SalesOfferAttachment.Where<SalesOfferAttachment>(x => x.TransId == request.Id).ToList();
                if (SalesOfferAttachment.Count > 0)
                {
                    foreach(SalesOfferAttachment item in SalesOfferAttachment)
                    {
                        FileInfo file = new FileInfo(item.FileUrl.Replace("https://localhost:7030/", "../TabpediaFin/"));
                        if (file.Exists)
                        {
                            file.Delete();
                        }
                    }
                    _context.SalesOfferAttachment.RemoveRange(SalesOfferAttachment);
                    await _context.SaveChangesAsync(cancellationToken);
                }

                _cacheRemover.RemoveCache();

                result.IsOk = true;
                result.ErrorMessage = string.Empty;
                result.Row = new SalesOfferFetchDto();
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
