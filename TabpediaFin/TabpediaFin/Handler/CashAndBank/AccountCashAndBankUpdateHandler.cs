using TabpediaFin.Handler.ExpenseCategoryHandler;

namespace TabpediaFin.Handler.CashAndBank;

public class AccountCashAndBankUpdateHandler : IRequestHandler<AccountCashAndBankUpdateDto, RowResponse<AccountCashAndBankFetchDto>>
{
    private readonly FinContext _context;
    private readonly ICurrentUser _currentUser;

    public AccountCashAndBankUpdateHandler(FinContext db, ICurrentUser currentUser)
    {
        _context = db;
        _currentUser = currentUser;
    }

    public async Task<RowResponse<AccountCashAndBankFetchDto>> Handle(AccountCashAndBankUpdateDto request, CancellationToken cancellationToken)
    {
        var result = new RowResponse<AccountCashAndBankFetchDto>();

        try
        {
            var accountCashAndBank = await _context.AccountCashAndBank.FirstAsync(x => x.Id == request.Id && x.TenantId == _currentUser.TenantId, cancellationToken);
            accountCashAndBank.Name = request.Name;
            accountCashAndBank.AccountNumber = request.AccountNumber;
            accountCashAndBank.CashAndBankCategoryId = request.CashAndBankCategoryId;
            accountCashAndBank.DetailAccountId = request.DetailAccountId;
            accountCashAndBank.TaxId = request.TaxId;
            accountCashAndBank.BankId = request.BankId;
            accountCashAndBank.Description = request.Description;
            accountCashAndBank.Balance = request.Balance;

            await _context.SaveChangesAsync(cancellationToken);

            var row = new AccountCashAndBankFetchDto()
            {
                Id = request.Id,
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

public class AccountCashAndBankUpdateDto : IRequest<RowResponse<AccountCashAndBankFetchDto>>
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string AccountNumber { get; set; } = string.Empty;
    public int CashAndBankCategoryId { get; set; } = 0;
    public int DetailAccountId { get; set; } = 0;
    public int TaxId { get; set; } = 0;
    public int BankId { get; set; } = 0;
    public string Description { get; set; } = string.Empty;
    public Int64 Balance { get; set; } = 0;
}
