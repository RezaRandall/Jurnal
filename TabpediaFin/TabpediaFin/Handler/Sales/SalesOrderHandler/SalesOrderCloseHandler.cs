namespace TabpediaFin.Handler.SalesOrderHandler
{
    public class SalesOrderCloseHandler : IRequestHandler<SalesOrderCloseDto, RowResponse<SalesOrderFetchDto>>
    {
        private readonly FinContext _context;
        private readonly ISalesOrderCacheRemover _cacheRemover;

        public SalesOrderCloseHandler(FinContext db, ISalesOrderCacheRemover cacheRemover)
        {
            _context = db;
            _cacheRemover = cacheRemover;
        }

        public async Task<RowResponse<SalesOrderFetchDto>> Handle(SalesOrderCloseDto request, CancellationToken cancellationToken)
        {
            var result = new RowResponse<SalesOrderFetchDto>();

            try
            {
                var SalesOrder = await _context.SalesOrder.FirstAsync(x => x.Id == request.Id, cancellationToken);

                if (SalesOrder == null || SalesOrder.Id == 0)
                {
                    throw new HttpException(HttpStatusCode.NotFound, "Data not found");
                }

                SalesOrder.Status = 1;
                

                await _context.SaveChangesAsync(cancellationToken);

                _cacheRemover.RemoveCache();

                var row = new SalesOrderFetchDto();

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

    public class SalesOrderCloseDto : IRequest<RowResponse<SalesOrderFetchDto>>
    {
        public int Id { get; set; }
        public int status { get; set; }
    }

    public class ISalesOrderCloseValidator : AbstractValidator<SalesOrderCloseDto>
    {

        public ISalesOrderCloseValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty();
        }
    }

}

