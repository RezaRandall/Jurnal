namespace TabpediaFin.Handler.SalesBillingHandler
{
    public class SalesBillingCloseHandler : IRequestHandler<SalesBillingCloseDto, RowResponse<SalesBillingFetchDto>>
    {
        private readonly FinContext _context;
        private readonly ISalesBillingCacheRemover _cacheRemover;

        public SalesBillingCloseHandler(FinContext db, ISalesBillingCacheRemover cacheRemover)
        {
            _context = db;
            _cacheRemover = cacheRemover;
        }

        public async Task<RowResponse<SalesBillingFetchDto>> Handle(SalesBillingCloseDto request, CancellationToken cancellationToken)
        {
            var result = new RowResponse<SalesBillingFetchDto>();

            try
            {
                var SalesBilling = await _context.SalesBilling.FirstAsync(x => x.Id == request.Id, cancellationToken);

                if (SalesBilling == null || SalesBilling.Id == 0)
                {
                    throw new HttpException(HttpStatusCode.NotFound, "Data not found");
                }

                SalesBilling.Status = 1;
                

                await _context.SaveChangesAsync(cancellationToken);

                _cacheRemover.RemoveCache();

                var row = new SalesBillingFetchDto();

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

    public class SalesBillingCloseDto : IRequest<RowResponse<SalesBillingFetchDto>>
    {
        public int Id { get; set; }
        public int status { get; set; }
    }

    public class ISalesBillingCloseValidator : AbstractValidator<SalesBillingCloseDto>
    {

        public ISalesBillingCloseValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty();
        }
    }

}

