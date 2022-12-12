namespace TabpediaFin.Handler.SalesOfferHandler
{
    public class SalesOfferCloseHandler : IRequestHandler<SalesOfferCloseDto, RowResponse<SalesOfferFetchDto>>
    {
        private readonly FinContext _context;
        private readonly ISalesOfferCacheRemover _cacheRemover;

        public SalesOfferCloseHandler(FinContext db, ISalesOfferCacheRemover cacheRemover)
        {
            _context = db;
            _cacheRemover = cacheRemover;
        }

        public async Task<RowResponse<SalesOfferFetchDto>> Handle(SalesOfferCloseDto request, CancellationToken cancellationToken)
        {
            var result = new RowResponse<SalesOfferFetchDto>();

            try
            {
                var SalesOffer = await _context.SalesOffer.FirstAsync(x => x.Id == request.Id, cancellationToken);

                if (SalesOffer == null || SalesOffer.Id == 0)
                {
                    throw new HttpException(HttpStatusCode.NotFound, "Data not found");
                }

                SalesOffer.Status = 1;
                

                await _context.SaveChangesAsync(cancellationToken);

                _cacheRemover.RemoveCache();

                var row = new SalesOfferFetchDto();

                result.IsOk = true;
                result.ErrorMessage = string.Empty;
                result.Row = row;
            }
            catch (Exception ex)
            {
                result.IsOk = false;
                result.ErrorMessage = ex.Message;
            }

            return result;
        }
    }

    public class SalesOfferCloseDto : IRequest<RowResponse<SalesOfferFetchDto>>
    {
        public int Id { get; set; }
        public int status { get; set; }
    }

    public class ISalesOfferCloseValidator : AbstractValidator<SalesOfferCloseDto>
    {

        public ISalesOfferCloseValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty();
        }
    }

}

