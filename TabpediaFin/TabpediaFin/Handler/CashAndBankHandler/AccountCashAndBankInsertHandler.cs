using TabpediaFin.Handler.ExpenseHandler;

namespace TabpediaFin.Handler.CashAndBank;

public class AccountCashAndBankInsertHandler : IRequestHandler<AccountCashAndBankInsertDto, RowResponse<AccountCashAndBankFetchDto>>
{
    private readonly FinContext _context;

    public AccountCashAndBankInsertHandler(FinContext db)
    {
        _context = db;
    }

    public async Task<RowResponse<AccountCashAndBankFetchDto>> Handle(AccountCashAndBankInsertDto request, CancellationToken cancellationToken)
    {
        var result = new RowResponse<AccountCashAndBankFetchDto>();

        var accountCashAndBank = new AccountCashAndBank()
        {
            Name = request.Name,
            AccountNumber = request.AccountNumber,
            CashAndBankCategoryId = request.CashAndBankCategoryId,
            DetailAccountId = request.DetailAccountId,
            TaxId = request.TaxId,
            BankId = request.BankId,
            Description = request.Description,
            Balance = request.Balance,
        };

        try
        {
            await _context.AccountCashAndBank.AddAsync(accountCashAndBank, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            var row = new AccountCashAndBankFetchDto()
            {
                Id = accountCashAndBank.Id,
                Name = accountCashAndBank.Name,
                AccountNumber = accountCashAndBank.AccountNumber,
                CashAndBankCategoryId = accountCashAndBank.CashAndBankCategoryId,
                DetailAccountId = accountCashAndBank.DetailAccountId,
                TaxId = accountCashAndBank.TaxId,
                BankId = accountCashAndBank.BankId,
                Description = accountCashAndBank.Description,
                Balance = accountCashAndBank.Balance,
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

public class AccountCashAndBankInsertDto : IRequest<RowResponse<AccountCashAndBankFetchDto>>
{
    public string Name { get; set; } = string.Empty;
    public string AccountNumber { get; set; } = string.Empty;
    public int CashAndBankCategoryId { get; set; } = 0;
    public int DetailAccountId { get; set; } = 0;
    public int TaxId { get; set; } = 0;
    public int BankId { get; set; } = 0;
    public string Description { get; set; } = string.Empty;
    public Int64 Balance { get; set; } = 0;
}