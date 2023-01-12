namespace TabpediaFin.Handler.CashAndBank;

public class AccountUpdateHandler : IRequestHandler<AccountUpdateDto, RowResponse<AccountFetchDto>>
{
    private readonly FinContext _context;
    private readonly ICurrentUser _currentUser;

    public AccountUpdateHandler(FinContext db, ICurrentUser currentUser)
    {
        _context = db;
        _currentUser = currentUser;
    }

    public async Task<RowResponse<AccountFetchDto>> Handle(AccountUpdateDto request, CancellationToken cancellationToken)
    {
        var result = new RowResponse<AccountFetchDto>();

        try
        {
            var accountCashAndBank = await _context.Account.FirstAsync(x => x.Id == request.Id && x.TenantId == _currentUser.TenantId, cancellationToken);
            accountCashAndBank.Name = request.Name;
            accountCashAndBank.AccountNumber = request.AccountNumber;
            accountCashAndBank.CategoryId = request.CategoryId;
            accountCashAndBank.AccountParentId = request.AccountParentId;
            accountCashAndBank.TaxId = request.TaxId;
            accountCashAndBank.BankId = request.BankId;
            accountCashAndBank.Description = request.Description;
            accountCashAndBank.Balance = request.Balance;
            accountCashAndBank.IsLocked = request.IsLocked;

            await _context.SaveChangesAsync(cancellationToken);

            var row = new AccountFetchDto()
            {
                Id = request.Id,
                Name = accountCashAndBank.Name,
                AccountNumber = accountCashAndBank.AccountNumber,
                CategoryId = accountCashAndBank.CategoryId,
                AccountParentId = accountCashAndBank.AccountParentId,
                TaxId = accountCashAndBank.TaxId,
                BankId = accountCashAndBank.BankId,
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

public class AccountUpdateDto : IRequest<RowResponse<AccountFetchDto>>
{
    public int Id { get; set; }
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
