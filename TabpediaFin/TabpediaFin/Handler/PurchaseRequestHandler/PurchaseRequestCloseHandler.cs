namespace TabpediaFin.Handler.PurchaseRequestHandler
{
    public class PurchaseRequestCloseHandler : IRequestHandler<PurchaseRequestCloseDto, RowResponse<PurchaseRequestFetchDto>>
    {
        private readonly FinContext _context;
        private readonly IPurchaseRequestCacheRemover _cacheRemover;

        public PurchaseRequestCloseHandler(FinContext db, IPurchaseRequestCacheRemover cacheRemover)
        {
            _context = db;
            _cacheRemover = cacheRemover;
        }

        public async Task<RowResponse<PurchaseRequestFetchDto>> Handle(PurchaseRequestCloseDto request, CancellationToken cancellationToken)
        {
            var result = new RowResponse<PurchaseRequestFetchDto>();

            try
            {
                var PurchaseRequest = await _context.PurchaseRequest.FirstAsync(x => x.Id == request.Id, cancellationToken);

                if (PurchaseRequest == null || PurchaseRequest.Id == 0)
                {
                    throw new HttpException(HttpStatusCode.NotFound, "Data not found");
                }

                PurchaseRequest.Status = 1;
                

                await _context.SaveChangesAsync(cancellationToken);

                _cacheRemover.RemoveCache();

                var row = new PurchaseRequestFetchDto();

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

    public class PurchaseRequestCloseDto : IRequest<RowResponse<PurchaseRequestFetchDto>>
    {
        public int Id { get; set; }
        public int status { get; set; }
    }

    public class IPurchaseRequestCloseValidator : AbstractValidator<PurchaseRequestCloseDto>
    {

        public IPurchaseRequestCloseValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty();
        }
    }

}

