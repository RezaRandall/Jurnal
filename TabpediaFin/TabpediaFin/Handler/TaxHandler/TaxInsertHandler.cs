using TabpediaFin.Handler.TaxHandler;

namespace TabpediaFin.Handler.TaxHandler;

public class TaxInsertHandler : IRequestHandler<TaxInsertDto, RowResponse<TaxFetchDto>>
{
    private readonly FinContext _context;

    public TaxInsertHandler(FinContext db)
    {
        _context = db;
    }

    public async Task<RowResponse<TaxFetchDto>> Handle(TaxInsertDto request, CancellationToken cancellationToken)
    {
        var result = new RowResponse<TaxFetchDto>();

        var Tax = new Tax()
        {
            Name = request.Name,
            Description = request.Description,
        };

        try
        {
            await _context.Tax.AddAsync(Tax, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            var row = new TaxFetchDto()
            {
                Id = Tax.Id,
                Name = Tax.Name,
                Description = Tax.Description,
                RatePercent = Tax.RatePercent,
            };

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



public class TaxInsertDto : IRequest<RowResponse<TaxFetchDto>>
{
    public string Name { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;
    
    public double RatePercent { get; set; }

}
