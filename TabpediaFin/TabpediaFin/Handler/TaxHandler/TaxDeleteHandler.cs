namespace TabpediaFin.Handler.TaxHandler
{
    public class TaxDeleteHandler : IDeleteByIdHandler<TaxFetchDto>
    {
        private readonly FinContext _context;
        private readonly ITaxCacheRemover _cacheRemover;

        public TaxDeleteHandler(FinContext db, ITaxCacheRemover cacheRemover)
        {
            _context = db;
            _cacheRemover = cacheRemover;
        }

        public async Task<RowResponse<TaxFetchDto>> Handle(DeleteByIdRequestDto<TaxFetchDto> request, CancellationToken cancellationToken)
        {
            var result = new RowResponse<TaxFetchDto>();

            try
            {
                var Tax = await _context.Tax.FirstAsync(x => x.Id == request.Id, cancellationToken);
                if (Tax == null || Tax.Id == 0)
                {
                    throw new HttpException(HttpStatusCode.NotFound, "Data not found");
                }

                _context.Tax.Remove(Tax);
                await _context.SaveChangesAsync(cancellationToken);

                _cacheRemover.RemoveCache();

                result.IsOk = true;
                result.ErrorMessage = string.Empty;
                result.Row = new TaxFetchDto();
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
