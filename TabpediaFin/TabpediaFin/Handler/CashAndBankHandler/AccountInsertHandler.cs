namespace TabpediaFin.Handler.CashAndBank;

public class AccountInsertHandler : IRequestHandler<AccountInsertDto, RowResponse<AccountFetchDto>>
{
    private readonly FinContext _context;

    public AccountInsertHandler(FinContext db)
    {
        _context = db;
    }

    public async Task<RowResponse<AccountFetchDto>> Handle(AccountInsertDto request, CancellationToken cancellationToken)
    {
        var result = new RowResponse<AccountFetchDto>();

        var accountCashAndBank = new Account()
        {
            Name = request.Name,
            AccountNumber = request.AccountNumber,
            CategoryId = request.CategoryId,
            AccountParentId = request.AccountParentId,
            BankId = request.BankId,
            TaxId = request.TaxId,
            Description = request.Description,
            Balance = request.Balance,
            IsLocked = request.IsLocked
        };

        try
        {
            await _context.Account.AddAsync(accountCashAndBank, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            var row = new AccountFetchDto()
            {
                Id = accountCashAndBank.Id,
                Name = accountCashAndBank.Name,
                AccountNumber = accountCashAndBank.AccountNumber,
                CategoryId = accountCashAndBank.CategoryId,
                AccountParentId = accountCashAndBank.AccountParentId,
                BankId = accountCashAndBank.BankId,
                TaxId = accountCashAndBank.TaxId,
                Description = accountCashAndBank.Description,
                Balance = accountCashAndBank.Balance,
                IsLocked = accountCashAndBank.IsLocked
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

public class AccountInsertDto : IRequest<RowResponse<AccountFetchDto>>
{
    public string Name { get; set; } = string.Empty;
    public string AccountNumber { get; set; } = string.Empty;
    public int CategoryId { get; set; } = 0;
    public int AccountParentId { get; set; } = 0;
    public int BankId { get; set; } = 0;
    public int TaxId { get; set; } = 0;
    public string Description { get; set; } = string.Empty;
    public double Balance { get; set; } = 0;
    public Boolean IsLocked { get; set; } = false;
}

