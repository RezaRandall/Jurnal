namespace TabpediaFin.Handler.PurchaseBillingHandler
{
    public class PurchaseBillingCloseHandler : IRequestHandler<PurchaseBillingCloseDto, RowResponse<PurchaseBillingFetchDto>>
    {
        private readonly FinContext _context;
        private readonly IPurchaseBillingCacheRemover _cacheRemover;

        public PurchaseBillingCloseHandler(FinContext db, IPurchaseBillingCacheRemover cacheRemover)
        {
            _context = db;
            _cacheRemover = cacheRemover;
        }

        public async Task<RowResponse<PurchaseBillingFetchDto>> Handle(PurchaseBillingCloseDto request, CancellationToken cancellationToken)
        {
            var result = new RowResponse<PurchaseBillingFetchDto>();

            try
            {
                var PurchaseBilling = await _context.PurchaseBilling.FirstAsync(x => x.Id == request.Id, cancellationToken);

                if (PurchaseBilling == null || PurchaseBilling.Id == 0)
                {
                    throw new HttpException(HttpStatusCode.NotFound, "Data not found");
                }

                PurchaseBilling.Status = 1;
                

                await _context.SaveChangesAsync(cancellationToken);

                _cacheRemover.RemoveCache();

                var row = new PurchaseBillingFetchDto();

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

    public class PurchaseBillingCloseDto : IRequest<RowResponse<PurchaseBillingFetchDto>>
    {
        public int Id { get; set; }
        public int status { get; set; }
    }

    public class IPurchaseBillingCloseValidator : AbstractValidator<PurchaseBillingCloseDto>
    {

        public IPurchaseBillingCloseValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty();
        }
    }

}

