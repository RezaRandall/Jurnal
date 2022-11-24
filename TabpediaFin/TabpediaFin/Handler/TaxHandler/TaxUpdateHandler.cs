namespace TabpediaFin.Handler.TaxHandler;

public class TaxUpdateHandler : IRequestHandler<TaxUpdateDto, RowResponse<TaxFetchDto>>
{
    private readonly FinContext _context;
    private readonly ICurrentUser _currentUser;

    public TaxUpdateHandler(FinContext db, ICurrentUser currentUser)
    {
        _context = db;
        _currentUser = currentUser;
    }

    public async Task<RowResponse<TaxFetchDto>> Handle(TaxUpdateDto request, CancellationToken cancellationToken)
    {
        var result = new RowResponse<TaxFetchDto>();

        try
        {
            var Tax = await _context.Tax.FirstAsync(x => x.Id == request.Id && x.TenantId == _currentUser.TenantId, cancellationToken);
            Tax.Name = request.Name;
            Tax.Description = request.Description;
            Tax.RatePercent = request.RatePercent;

            await _context.SaveChangesAsync(cancellationToken);

            var row = new TaxFetchDto()
            {
                Id = request.Id,
                Name = Tax.Name,
                Description = Tax.Description,
                RatePercent = Tax.RatePercent,
            };

            result.IsOk= true;
            result.ErrorMessage = string.Empty;
            result.Row = row;
        }
        catch (Exception ex)
        {
            result.IsOk= false;
            result.ErrorMessage = ex.Message;
        }

        return result;
    }
}


public class TaxUpdateDto : IRequest<RowResponse<TaxFetchDto>>
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;
    public double RatePercent { get; set; }
}
