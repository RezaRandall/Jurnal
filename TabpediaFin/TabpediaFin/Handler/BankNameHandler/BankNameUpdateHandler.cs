using TabpediaFin.Handler.CashAndBank;

namespace TabpediaFin.Handler.BankNameHandler;

public class BankNameUpdateHandler : IRequestHandler<BankNameUpdateDto, RowResponse<BankNameFetchDto>>
{
    private readonly FinContext _context;
    private readonly ICurrentUser _currentUser;

    public BankNameUpdateHandler(FinContext db, ICurrentUser currentUser)
    {
        _context = db;
        _currentUser = currentUser;
    }

    public async Task<RowResponse<BankNameFetchDto>> Handle(BankNameUpdateDto request, CancellationToken cancellationToken)
    {
        var result = new RowResponse<BankNameFetchDto>();

        try
        {
            var bankName = await _context.BankName.FirstAsync(x => x.Id == request.Id && x.TenantId == _currentUser.TenantId, cancellationToken);
            bankName.Name = request.Name;


            await _context.SaveChangesAsync(cancellationToken);

            var row = new BankNameFetchDto()
            {
                Id = request.Id,
                Name = bankName.Name,
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

public class BankNameUpdateDto : IRequest<RowResponse<BankNameFetchDto>>
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
}
