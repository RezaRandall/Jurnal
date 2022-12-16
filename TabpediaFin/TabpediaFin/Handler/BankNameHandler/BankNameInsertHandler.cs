using TabpediaFin.Handler.CashAndBank;

namespace TabpediaFin.Handler.BankNameHandler;

public class BankNameInsertHandler : IRequestHandler<BankNameInsertDto, RowResponse<BankNameFetchDto>>
{
    private readonly FinContext _context;

    public BankNameInsertHandler(FinContext db)
    {
        _context = db;
    }

    public async Task<RowResponse<BankNameFetchDto>> Handle(BankNameInsertDto request, CancellationToken cancellationToken)
    {
        var result = new RowResponse<BankNameFetchDto>();

        var accountCashAndBank = new BankName()
        {
            Name = request.Name,
        };

        try
        {
            await _context.BankName.AddAsync(accountCashAndBank, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            var row = new BankNameFetchDto()
            {
                Id = accountCashAndBank.Id,
                Name = accountCashAndBank.Name,
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


public class BankNameInsertDto : IRequest<RowResponse<BankNameFetchDto>>
{
    public string Name { get; set; } = string.Empty;
}