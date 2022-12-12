namespace TabpediaFin.Handler.PurchaseOrderHandler
{
    public class PurchaseOrderCloseHandler : IRequestHandler<PurchaseOrderCloseDto, RowResponse<PurchaseOrderFetchDto>>
    {
        private readonly FinContext _context;
        private readonly IPurchaseOrderCacheRemover _cacheRemover;

        public PurchaseOrderCloseHandler(FinContext db, IPurchaseOrderCacheRemover cacheRemover)
        {
            _context = db;
            _cacheRemover = cacheRemover;
        }

        public async Task<RowResponse<PurchaseOrderFetchDto>> Handle(PurchaseOrderCloseDto request, CancellationToken cancellationToken)
        {
            var result = new RowResponse<PurchaseOrderFetchDto>();

            try
            {
                var PurchaseOrder = await _context.PurchaseOrder.FirstAsync(x => x.Id == request.Id, cancellationToken);

                if (PurchaseOrder == null || PurchaseOrder.Id == 0)
                {
                    throw new HttpException(HttpStatusCode.NotFound, "Data not found");
                }

                PurchaseOrder.Status = 1;
                

                await _context.SaveChangesAsync(cancellationToken);

                _cacheRemover.RemoveCache();

                var row = new PurchaseOrderFetchDto();

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

    public class PurchaseOrderCloseDto : IRequest<RowResponse<PurchaseOrderFetchDto>>
    {
        public int Id { get; set; }
        public int status { get; set; }
    }

    public class IPurchaseOrderCloseValidator : AbstractValidator<PurchaseOrderCloseDto>
    {

        public IPurchaseOrderCloseValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty();
        }
    }

}

