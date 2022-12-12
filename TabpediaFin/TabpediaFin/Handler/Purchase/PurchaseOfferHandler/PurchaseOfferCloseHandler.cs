namespace TabpediaFin.Handler.PurchaseOfferHandler
{
    public class PurchaseOfferCloseHandler : IRequestHandler<PurchaseOfferCloseDto, RowResponse<PurchaseOfferFetchDto>>
    {
        private readonly FinContext _context;
        private readonly IPurchaseOfferCacheRemover _cacheRemover;

        public PurchaseOfferCloseHandler(FinContext db, IPurchaseOfferCacheRemover cacheRemover)
        {
            _context = db;
            _cacheRemover = cacheRemover;
        }

        public async Task<RowResponse<PurchaseOfferFetchDto>> Handle(PurchaseOfferCloseDto request, CancellationToken cancellationToken)
        {
            var result = new RowResponse<PurchaseOfferFetchDto>();

            try
            {
                var PurchaseOffer = await _context.PurchaseOffer.FirstAsync(x => x.Id == request.Id, cancellationToken);

                if (PurchaseOffer == null || PurchaseOffer.Id == 0)
                {
                    throw new HttpException(HttpStatusCode.NotFound, "Data not found");
                }

                PurchaseOffer.Status = 1;
                

                await _context.SaveChangesAsync(cancellationToken);

                _cacheRemover.RemoveCache();

                var row = new PurchaseOfferFetchDto();

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

    public class PurchaseOfferCloseDto : IRequest<RowResponse<PurchaseOfferFetchDto>>
    {
        public int Id { get; set; }
        public int status { get; set; }
    }

    public class IPurchaseOfferCloseValidator : AbstractValidator<PurchaseOfferCloseDto>
    {

        public IPurchaseOfferCloseValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty();
        }
    }

}

