using TabpediaFin.Handler.CashAndBank;

namespace TabpediaFin.Handler.CashAndBankCategoryHandler;

public class AccountCashAndBankCategoryUpdateHandler : IRequestHandler<AccountCashAndBankCategoryUpdateDto, RowResponse<AccountCashAndBankCategoryFetchDto>>
{
    private readonly FinContext _context;
    private readonly ICurrentUser _currentUser;

    public AccountCashAndBankCategoryUpdateHandler(FinContext db, ICurrentUser currentUser)
    {
        _context = db;
        _currentUser = currentUser;
    }

    public async Task<RowResponse<AccountCashAndBankCategoryFetchDto>> Handle(AccountCashAndBankCategoryUpdateDto request, CancellationToken cancellationToken)
    {
        var result = new RowResponse<AccountCashAndBankCategoryFetchDto>();

        try
        {
            var accountCashAndBank = await _context.AccountCashAndBankCategory.FirstAsync(x => x.Id == request.Id && x.TenantId == _currentUser.TenantId, cancellationToken);
            accountCashAndBank.Name = request.Name;

            await _context.SaveChangesAsync(cancellationToken);

            var row = new AccountCashAndBankCategoryFetchDto()
            {
                Id = request.Id,
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


public class AccountCashAndBankCategoryUpdateDto : IRequest<RowResponse<AccountCashAndBankCategoryFetchDto>>
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
}
