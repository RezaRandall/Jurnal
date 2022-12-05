using TabpediaFin.Handler.ExpenseCategoryHandler;

namespace TabpediaFin.Handler.CashAndBank;

public class AccountCashAndBankDeleteHandler : IRequestHandler<AccountCashAndBankDeleteDto, RowResponse<bool>>
{
    private readonly FinContext _context;
    private readonly ICurrentUser _currentUser;

    public AccountCashAndBankDeleteHandler(FinContext db, ICurrentUser currentUser)
    {
        _context = db;
        _currentUser = currentUser;
    }

    public async Task<RowResponse<bool>> Handle(AccountCashAndBankDeleteDto request, CancellationToken cancellationToken)
    {
        var result = new RowResponse<bool>();
        try
        {
            var ExpenseCategory = await _context.AccountCashAndBank.FirstAsync(x => x.Id == request.Id && x.TenantId == _currentUser.TenantId, cancellationToken);

            _context.AccountCashAndBank.Attach(ExpenseCategory);
            _context.AccountCashAndBank.Remove(ExpenseCategory);

            await _context.SaveChangesAsync(cancellationToken);

            result.IsOk = true;
            result.ErrorMessage = string.Empty;
            result.Row = true;
        }
        catch (Exception ex)
        {
            result.IsOk = false;
            result.ErrorMessage = ex.Message;
        }

        return result;
    }


}


public class AccountCashAndBankDeleteDto : IRequest<RowResponse<bool>>
{
    public int Id { get; set; }

}